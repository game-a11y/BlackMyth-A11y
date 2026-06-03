namespace WkAccess.B1.UI;

/// <summary>
/// 控件树遍历/转储工具。
/// 供 DebugCommands 和 B1InputTipsScanner 共用。
/// </summary>
internal static class WidgetTreeDumper
{
    /// <summary>通过反射获取 UUserWidget 的根 Panel，绕过 C# 绑定缺失</summary>
    public static UWidget? GetWidgetTreeRoot(UUserWidget uw)
    {
        try
        {
            // 方法 1：反射 GetRootWidget()
            var m = uw.GetType().GetMethod("GetRootWidget", Type.EmptyTypes);
            if (m != null)
            {
                var root = m.Invoke(uw, null) as UWidget;
                if (root != null) return root;
            }

            // 方法 2：反射 WidgetTree 属性
            var wtProp = uw.GetType().GetProperty("WidgetTree");
            if (wtProp != null)
            {
                var widgetTree = wtProp.GetValue(uw);
                if (widgetTree != null)
                {
                    var rootProp = widgetTree.GetType().GetProperty("RootWidget");
                    var root = rootProp?.GetValue(widgetTree) as UWidget;
                    if (root != null) return root;
                }
            }

            // 方法 3：找任意已知子控件，从它的父节点向上走到根 Panel
            var knownNames = new[] { "InputIcon", "TxtDesc", "InputRight", "InputLeft", "BI_InputRoot", "ImgSdTcBg", "BI_Btn", "BI_ScrollBox", "RootCon", "CanvasPanel_0" };
            foreach (var name in knownNames)
            {
                var child = GSUIUtil.FindChildWidget(uw, name);
                if (child != null)
                {
                    var parent = child.GetParent();
                    while (parent != null && parent.GetParent() != null && parent.GetParent() != uw)
                        parent = parent.GetParent();
                    if (parent is UPanelWidget)
                        return parent;
                }
            }
        }
        catch { }
        return null;
    }

    /// <summary>
    /// 递归转储控件树到日志。
    /// </summary>
    /// <param name="widget">起始控件</param>
    /// <param name="indent">缩进字符串</param>
    /// <param name="depth">当前深度</param>
    /// <param name="maxDepth">最大深度</param>
    /// <param name="maxChildren">每节点最大子节点数</param>
    /// <param name="log">日志输出 Action（默认 A11yLog.Warning）</param>
    public static void DumpRecursive(
        UWidget widget,
        string indent,
        int depth,
        int maxDepth,
        int maxChildren = 60,
        Action<string>? log = null)
    {
        log ??= A11yLog.Warning;

        if (widget == null || !widget.IsValidLowLevel())
            return;

        if (depth > maxDepth)
        {
            log($"{indent}(max depth {maxDepth})");
            return;
        }

        try
        {
            var cn = WkUtils.GetClassName(widget);
            var name = widget.GetFName().ToString();
            if (name.Length > 0 && char.IsDigit(name[0]))
                name = "";
            var nameStr = name.Length > 0 ? $"  [{name}]" : "";
            var status = "";

            // UUserWidget：进入 WidgetTree.RootWidget
            if (widget is UUserWidget uw)
            {
                var root = GetWidgetTreeRoot(uw);
                if (root != null)
                {
                    log($"{indent}[{depth}] {cn}{nameStr}");
                    DumpRecursive(root, indent + "  ", depth + 1, maxDepth, maxChildren, log);
                    return;
                }
                status = " [no tree root]";
            }

            if (widget is UPanelWidget panel)
            {
                var childCount = panel.GetChildrenCount();
                if (childCount == 0)
                {
                    log($"{indent}[{depth}] {cn}{nameStr} (0 children)");
                }
                else
                {
                    log($"{indent}[{depth}] {cn}{nameStr} ({childCount} children)");
                    var nextIndent = indent + "  ";
                    for (int i = 0; i < childCount && i < maxChildren; i++)
                    {
                        var child = panel.GetChildAt(i);
                        DumpRecursive(child, nextIndent, depth + 1, maxDepth, maxChildren, log);
                    }
                    if (childCount > maxChildren)
                        log($"{nextIndent}... (+{childCount - maxChildren} more)");
                }
                return;
            }

            // 非 Panel、非 UserWidget 的叶子节点
            log($"{indent}[{depth}] {cn}{nameStr} (leaf){status}");
        }
        catch (Exception ex)
        {
            log($"{indent}[{depth}] (error: {ex.Message})");
        }
    }
}
