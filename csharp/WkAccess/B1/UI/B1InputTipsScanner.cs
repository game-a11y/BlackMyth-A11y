using UnrealEngine.Engine;
using UnrealEngine.Plugins.EnhancedInput;
using b1.Plugins.GSInput;

namespace WkAccess.B1.UI;

/// <summary>
/// 页面按键提示扫描器 — F1 时扫描当前页面 InputRight/InputLeft 容器及 BUI_InputTipsOne。
/// </summary>
public static class B1InputTipsScanner
{
    static bool _initialized;

    public static void Init()
    {
        if (_initialized) return;
        _initialized = true;

        A11yLog.Debug("[InputTipsScanner] 已初始化 (F1 手动触发)");
    }

    private static string BuildSummary(HashSet<(string desc, string key)> tips, string prefix)
    {
        var parts = tips.Select(t => FormatTip(t.key, t.desc));
        return prefix + string.Join("；", parts);
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
                A11yTolk.Speak(summary, false);
                return;
            }
        }

        // 回退：从 InputTipsConfig 数据资产读取
        ScanViaInputTipsConfig(pageId);
    }

    // ── 回退：InputTipsConfig 数据资产读取 ──

    static void ScanViaInputTipsConfig(int pageId)
    {
        try
        {
            var tips = ReadTipsFromConfig(pageId);
            if (tips.Count == 0)
            {
                A11yLog.Debug($"[InputTipsScanner] {(EnPageID)pageId} 配置中也未找到按键提示");
                return;
            }
            var summary = BuildSummary(tips, "操作提示：");
            A11yLog.Info($"[InputTipsScanner] (配置) {summary}");
            A11yTolk.Speak(summary, false);
        }
        catch (Exception ex)
        {
            A11yLog.Warning($"[InputTipsScanner] 读取配置失败: {ex.Message}");
        }
    }

    static HashSet<(string desc, string key)> ReadTipsFromConfig(int pageId)
    {
        var tips = new HashSet<(string desc, string key)>();

        try
        {
            var pageType = GSEUtil.GetPageTypebyPageID(pageId);
            if (pageType == EUIPageType.None) return tips;

            var world = WkUtils.GetWorld();
            if (world == null) return tips;
            var preloadMgr = BGW_PreloadAssetMgr.Get(world);
            var config = preloadMgr?.UIConfigDataAsset?.InputTipsConfig;
            if (config == null) return tips;

            if (!config.TryGetValue(pageType, out var leftRightCfg))
                return tips;

            var pc = UGSE_EngineFuncLib.GetFirstLocalPlayerController(world);
            CollectTipsFromCfg(leftRightCfg.LeftInputTipsCfg, pc, tips);
            CollectTipsFromCfg(leftRightCfg.RightInputTipsCfg, pc, tips);

            A11yLog.Debug($"[InputTipsScanner] 配置 {(EnPageID)pageId} 共 {tips.Count} 条");
        }
        catch (Exception ex)
        {
            A11yLog.Debug($"[InputTipsScanner] ReadTipsFromConfig 异常: {ex.Message}");
        }

        return tips;
    }

    static void CollectTipsFromCfg(FInputTipsCfg cfg, APlayerController? pc, HashSet<(string desc, string key)> tips)
    {
        if (cfg.AwalysShowInput.InputActionList == null)
            return;

        foreach (var entry in cfg.AwalysShowInput.InputActionList)
        {
            var desc = entry.TxtDesc?.ToString();
            if (string.IsNullOrEmpty(desc)) continue;

            var keyName = QueryKeyForAction(pc, entry.InputAction);

            tips.Add((desc!, keyName ?? "??"));
        }
    }

    static string? QueryKeyForAction(APlayerController? pc, UInputAction? action)
    {
        if (pc == null || action == null) return null;

        try
        {
            var keys = UGSE_InputFuncLib.QueryKeysMappedToAction(pc, action);
            if (keys is { Count: > 0 })
                return FKeyToName(keys[0]);
        }
        catch { }

        return null;
    }

    static string? FKeyToName(object key)
    {
        try
        {
            var fname = key.GetType().GetMethod("GetFName")?.Invoke(key, null);
            var name = fname?.ToString() ?? "";
            if (string.IsNullOrEmpty(name)) return null;

            return MapFKeyName(name);
        }
        catch { return null; }
    }

    /// <summary>UE4 FKey 名 → 中文可读按键名</summary>
    static string MapFKeyName(string keyName)
    {
        // 常见映射
        return keyName switch
        {
            "SpaceBar" => "空格键",
            "Enter" => "回车键",
            "Escape" => "Esc键",
            "BackSpace" => "退格键",
            "Delete" => "Delete键",
            "Tab" => "Tab键",
            "CapsLock" => "CapsLock键",
            "LeftShift" => "左Shift键",
            "RightShift" => "右Shift键",
            "LeftControl" => "左Ctrl键",
            "RightControl" => "右Ctrl键",
            "LeftAlt" => "左Alt键",
            "RightAlt" => "右Alt键",
            "Up" => "方向上键",
            "Down" => "方向下键",
            "Left" => "方向左键",
            "Right" => "方向右键",
            "LeftMouseButton" => "鼠标左键",
            "RightMouseButton" => "鼠标右键",
            "MiddleMouseButton" => "鼠标中键",
            "MouseScrollUp" => "滚轮上",
            "MouseScrollDown" => "滚轮下",
            "Gamepad_FaceButton_Bottom" => "A键(手柄)",
            "Gamepad_FaceButton_Right" => "B键(手柄)",
            "Gamepad_FaceButton_Left" => "X键(手柄)",
            "Gamepad_FaceButton_Top" => "Y键(手柄)",
            "Gamepad_LeftShoulder" => "LB键(手柄)",
            "Gamepad_RightShoulder" => "RB键(手柄)",
            "Gamepad_LeftTrigger" => "LT键(手柄)",
            "Gamepad_RightTrigger" => "RT键(手柄)",
            "Gamepad_LeftThumbstick" => "左摇杆按下(手柄)",
            "Gamepad_RightThumbstick" => "右摇杆按下(手柄)",
            "Gamepad_DPad_Up" => "十字键上(手柄)",
            "Gamepad_DPad_Down" => "十字键下(手柄)",
            "Gamepad_DPad_Left" => "十字键左(手柄)",
            "Gamepad_DPad_Right" => "十字键右(手柄)",
            "Gamepad_Special_Left" => "View键(手柄)",
            "Gamepad_Special_Right" => "Menu键(手柄)",
            _ => keyName.Length switch
            {
                1 when char.IsLetterOrDigit(keyName[0]) => $"{keyName}键",
                _ => keyName
            }
        };
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
            if (root is UUserWidget uw)
            {
                var treeRoot = WidgetTreeDumper.GetWidgetTreeRoot(uw);
                if (treeRoot != null) root = treeRoot;
            }

            var container = FindChildWidgetByName(root, containerName);
            if (container is UPanelWidget panel)
            {
                var childCount = panel.GetChildrenCount();
                for (int i = 0; i < childCount; i++)
                {
                    var child = panel.GetChildAt(i);
                    if (child == null || !child.IsValidLowLevel()) continue;
                    ReadInputTipsOne(child, tips);
                }
            }
        }
        catch (Exception ex)
        {
            A11yLog.Debug($"[InputTipsScanner] FindInputTipsInContainer({containerName}) 异常: {ex.Message}");
        }
    }

    /// <summary>递归按名称查找子控件，穿透 UUserWidget.WidgetTree</summary>
    static UWidget? FindChildWidgetByName(UWidget root, string name)
    {
        if (root == null || !root.IsValidLowLevel())
            return null;

        // 先穿透 UUserWidget
        if (root is UUserWidget uw)
        {
            var treeRoot = WidgetTreeDumper.GetWidgetTreeRoot(uw);
            if (treeRoot != null) return FindChildWidgetByName(treeRoot, name);
        }

        // 检查自身
        if (root.GetFName().ToString() == name)
            return root;

        // 遍历子节点
        if (root is UPanelWidget panel)
        {
            var childCount = panel.GetChildrenCount();
            for (int i = 0; i < childCount; i++)
            {
                var found = FindChildWidgetByName(panel.GetChildAt(i), name);
                if (found != null) return found;
            }
        }

        return null;
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
                ReadInputTipsOne(widget, results);
                return;
            }

            // UUserWidget：进入 WidgetTree.RootWidget
            if (widget is UUserWidget uw)
            {
                var root = WidgetTreeDumper.GetWidgetTreeRoot(uw);
                if (root != null)
                {
                    FindInputTipsRecursive(root, results, depth + 1, maxDepth);
                    return;
                }
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
    static void ReadInputTipsOne(UWidget tipsWidget, HashSet<(string desc, string key)> tips)
    {
        try
        {
            if (tipsWidget is not UUserWidget uw)
                return;

            // BI_InputOne_C 特殊处理：穿透 WidgetTree 后递归找 GSInputActionIcon + Text
            if (WkUtils.GetClassName(uw) == "BI_InputOne_C")
            {
                var iroot = WidgetTreeDumper.GetWidgetTreeRoot(uw);
                string? ikey = null, idesc = null;
                CollectKeyAndText(iroot, ref ikey, ref idesc);
                if (!string.IsNullOrEmpty(idesc))
                    tips.Add((idesc!, ikey ?? "??"));
                return;
            }

            // 通用路径：进入根 Panel，收集所有 Icon+Text 对
            var root = WidgetTreeDumper.GetWidgetTreeRoot(uw);
            if (root is UPanelWidget rootPanel)
            {
                CollectIconTextPairs(rootPanel, tips);
                return;
            }

            // 回退：按名称搜索
            var iconWidget = GSUIUtil.FindChildWidget(uw, "InputIcon")
                          ?? GSUIUtil.FindChildWidget(uw, "ImgKeyIcon");
            var keyName = Input.GSInputKeyReader.ReadKeyNameFromIcon(iconWidget);

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

        if (widget is UUserWidget uw)
        {
            var root = WidgetTreeDumper.GetWidgetTreeRoot(uw);
            if (root != null) { CollectKeyAndText(root, ref ikey, ref idesc, depth + 1); return; }
        }

        var cn = WkUtils.GetClassName(widget);
        if (cn == "GSInputActionIcon" && ikey == null)
            ikey = Input.GSInputKeyReader.ReadKeyNameFromIcon(widget);
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
                var iconKey = Input.GSInputKeyReader.ReadKeyNameFromIcon(child);
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
}
