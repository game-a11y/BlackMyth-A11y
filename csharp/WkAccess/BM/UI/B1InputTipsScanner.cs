using WkAccess.A11y;
using WkAccess.A11y.UI;
using WkAccess.A11y.UE;
using WkAccess.BM;

namespace WkAccess.BM.UI;

/// <summary>
/// 页面按键提示扫描器 — F1 时扫描当前页面 InputRight/InputLeft 容器及 BUI_InputTipsOne。
/// </summary>
public static class B1InputTipsScanner
{
    /// <summary>当按键提示扫描完成时触发，由 A11yMod 层订阅并朗读</summary>
    public static event Action<string>? OnInputTipsScanned;

    static bool _initialized;

    public static void Init()
    {
        if (_initialized) return;
        _initialized = true;
        A11yLog.Debug("[InputTipsScanner] 已初始化 (F1 手动触发)");
    }

    public static void ScanAndSpeak(int pageId)
    {
        var pageWidget = GetPageWidget(pageId);
        if (pageWidget != null && pageWidget.IsValidLowLevel())
        {
            var tips = new HashSet<(string desc, string key)>();
            FindInputTipsRecursive(pageWidget, tips, depth: 0, maxDepth: 10);
            FindInputTipsInContainer(pageWidget, "InputRight", tips);
            FindInputTipsInContainer(pageWidget, "InputLeft", tips);

            if (tips.Count > 0)
            {
                var summary = BuildSummary(tips, "提示文本 ");
                A11yLog.Info($"[InputTipsScanner] {summary}");
                OnInputTipsScanned?.Invoke(summary);
            }
            else
            {
                A11yLog.Debug($"[InputTipsScanner] {(EnPageID)pageId} 未找到按键提示");
            }
        }
    }

    // ── BUI_InputTipsOne 控件树扫描 ──

    /// <summary>通过 GSPageOP 直接获取页面根 widget</summary>
    static UUserWidget? GetPageWidget(int pageId)
    {
        try
        {
            return GSG.GSPageOP.FindUIPage(pageId)?.GetRootBUIWidget() as UUserWidget;
        }
        catch { return null; }
    }

    /// <summary>在控件树中按名称查找容器（如 InputRight），读取其所有子节点作为按键提示</summary>
    static void FindInputTipsInContainer(UWidget root, string containerName, HashSet<(string desc, string key)> tips)
    {
        try
        {
            root = WidgetTreeDumper.Dereference(root);

            var container = WidgetTreeDumper.FindChildByName(root, containerName);
            if (container is UPanelWidget panel)
            {
                var childCount = panel.GetChildrenCount();
                for (int i = 0; i < childCount; i++)
                {
                    var child = panel.GetChildAt(i);
                    if (child == null || !child.IsValidLowLevel()) continue;
                    CollectTipsFromWidget(child, tips);
                }
            }
        }
        catch (Exception ex)
        {
            A11yLog.Debug($"[InputTipsScanner] FindInputTipsInContainer({containerName}) 异常: {ex.Message}");
        }
    }

    /// <summary>递归遍历 widget 树，收集 BUI_InputTipsOne 的按键提示文本</summary>
    static void FindInputTipsRecursive(UWidget widget, HashSet<(string desc, string key)> results, int depth, int maxDepth)
    {
        if (depth > maxDepth || widget == null || !widget.IsValidLowLevel())
            return;

        try
        {
            var cn = WkUtils.GetClassName(widget);

            if (cn is "BUI_InputTipsOne_C" or "BI_InputOne_C")
            {
                CollectTipsFromWidget(widget, results);
                return;
            }

            // UUserWidget：进入 WidgetTree.RootWidget
            var deref = WidgetTreeDumper.Dereference(widget);
            if (deref != widget)
            {
                FindInputTipsRecursive(deref!, results, depth + 1, maxDepth);
                return;
            }

            if (widget is UPanelWidget panel)
            {
                var childCount = panel.GetChildrenCount();
                for (int i = 0; i < childCount; i++)
                {
                    var child = panel.GetChildAt(i);
                    FindInputTipsRecursive(child, results, depth + 1, maxDepth);
                }
            }
        }
        catch { }
    }

    /// <summary>读取单个输入提示控件，向 tips 添加 (desc, key) 对</summary>
    static void CollectTipsFromWidget(UWidget tipsWidget, HashSet<(string desc, string key)> tips)
    {
        try
        {
            if (tipsWidget is not UUserWidget uw)
                return;

            // BI_InputOne_C 特殊处理
            if (WkUtils.GetClassName(uw) == "BI_InputOne_C")
            {
                string? ikey = null, idesc = null;
                CollectKeyAndText(uw, ref ikey, ref idesc);
                if (!string.IsNullOrEmpty(idesc))
                    tips.Add((idesc!, ikey ?? "??"));
                return;
            }

            // 通用路径：进入根 Panel，收集所有 Icon+Text 对
            if (WidgetTreeDumper.Dereference(uw) is UPanelWidget rootPanel)
            {
                CollectIconTextPairs(rootPanel, tips);
                return;
            }

            // 回退：按名称搜索
            var iconWidget = GSUIUtil.FindChildWidget(uw, "InputIcon")
                          ?? GSUIUtil.FindChildWidget(uw, "ImgKeyIcon");
            var keyName = GSInputKeyReader.ReadKeyNameFromIcon(iconWidget);

            var desc = B1WidgetResolvers.FindTextByName(uw, "TxtDesc")
                    ?? B1WidgetResolvers.FindTextByName(uw, "TxtName")
                    ?? UIScreenTextProvider.FindAnyText(uw);

            if (!string.IsNullOrEmpty(desc))
                tips.Add((desc!, keyName ?? "??"));
        }
        catch { }
    }

    /// <summary>递归收集第一个 GSInputActionIcon 和 Text/ScaleText，存入 ref 参数</summary>
    static void CollectKeyAndText(UWidget? widget, ref string? ikey, ref string? idesc, int depth = 0)
    {
        if (depth > 8 || widget == null || !widget.IsValidLowLevel()) return;

        var deref = WidgetTreeDumper.Dereference(widget);
        if (deref != widget) { CollectKeyAndText(deref!, ref ikey, ref idesc, depth + 1); return; }

        var cn = WkUtils.GetClassName(widget);
        if (cn == "GSInputActionIcon" && ikey == null)
            ikey = GSInputKeyReader.ReadKeyNameFromIcon(widget);
        else if ((cn is "TextBlock" or "GSScaleText" or "GSRichScaleText") && idesc == null)
            idesc = ReadTextFromWidget(widget);

        // 两个都找到就停
        if (ikey != null && idesc != null) return;

        if (widget is UPanelWidget panel)
        {
            var cc = panel.GetChildrenCount();
            for (int i = 0; i < cc; i++)
            {
                CollectKeyAndText(panel.GetChildAt(i), ref ikey, ref idesc, depth + 1);
                if (ikey != null && idesc != null) return;
            }
        }
    }

    static void CollectIconTextPairs(UPanelWidget panel, HashSet<(string desc, string key)> tips, int maxPairs = 20)
    {
        var childCount = panel.GetChildrenCount();
        // 先收集所有 Icon+Text 直接子节点对
        for (int i = 0; i < childCount && tips.Count < maxPairs; i++)
        {
            var child = panel.GetChildAt(i);
            if (child == null || !child.IsValidLowLevel()) continue;

            var cn = WkUtils.GetClassName(child);

            // 每个子节点可能是:
            // a) 直接的 Icon（GSInputActionIcon）→ 下一个兄弟是 Text
            if (cn == "GSInputActionIcon")
            {
                var iconKey = GSInputKeyReader.ReadKeyNameFromIcon(child);
                var text = FindNextSiblingText(panel, i, childCount);
                if (!string.IsNullOrEmpty(text) || !string.IsNullOrEmpty(iconKey))
                {
                    tips.Add((text ?? "", iconKey ?? "??"));
                }
                continue;
            }

            // b) UUserWidget — 递归进入
            if (child is UUserWidget childUw)
            {
                var childRoot = WidgetTreeDumper.GetWidgetTreeRoot(childUw);
                if (childRoot is UPanelWidget childPanel)
                    CollectIconTextPairs(childPanel, tips, maxPairs);
                continue;
            }

            // c) 其他 Panel（如 HorizontalBox 包裹一对）→ 递归
            if (child is UPanelWidget childPnl)
            {
                CollectIconTextPairs(childPnl, tips, maxPairs);
            }
        }
    }

    /// <summary>找最近的相邻文本控件（前/后各一个位置）</summary>
    static string? FindNextSiblingText(UPanelWidget panel, int iconIndex, int childCount)
    {
        // 先看后面
        for (int i = iconIndex + 1; i < childCount && i <= iconIndex + 2; i++)
        {
            var text = TryReadTextChild(panel.GetChildAt(i));
            if (text != null) return text;
        }
        // 再看前面
        for (int i = iconIndex - 1; i >= 0 && i >= iconIndex - 2; i--)
        {
            var text = TryReadTextChild(panel.GetChildAt(i));
            if (text != null) return text;
        }
        return null;
    }

    static string? TryReadTextChild(UWidget? child)
    {
        if (child == null || !child.IsValidLowLevel()) return null;
        var cn = WkUtils.GetClassName(child);
        if (cn is "TextBlock" or "GSScaleText" or "GSRichScaleText" or "RichTextBlock"
            or "GSInputRichTextBlock" or "UTextBlock")
        {
            return ReadTextFromWidget(child);
        }
        return null;
    }

    static string? ReadTextFromWidget(UWidget widget)
    {
        try
        {
            var m = widget.GetType().GetMethod("GetText", Type.EmptyTypes);
            var text = m?.Invoke(widget, null)?.ToString();
            if (!string.IsNullOrEmpty(text)) return text;

            var prop = widget.GetType().GetProperty("Text");
            text = prop?.GetValue(widget)?.ToString();
            if (!string.IsNullOrEmpty(text)) return text;
        }
        catch { }
        return null;
    }

    /// <summary>格式化 "描述 按键名"。desc 为空时返回 null。</summary>
    static string? FormatTip(string? keyName, string? desc)
    {
        if (string.IsNullOrEmpty(desc))
            return null;
        var key = string.IsNullOrEmpty(keyName) ? "??" : keyName;
        return $"{desc} {key}";
    }

    private static string BuildSummary(HashSet<(string desc, string key)> tips, string prefix)
    {
        var parts = tips.Select(t => FormatTip(t.key, t.desc));
        return prefix + string.Join("；", parts);
    }
}
