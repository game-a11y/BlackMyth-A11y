using b1.UI;
using GSE.GSUI;
using UnrealEngine.UMG;

namespace WkAccess.A11y.UI;

/// <summary>
/// 按钮文本提取 — 从 BUI_Button 的 WidgetTree 中读取显示的文本。
///
/// C# 可通过 USharp 的 UPanelWidget.GetChildAt() 和
/// GSUIUtil.FindChildWidget() 遍历 WidgetTree，等效于 UE4SS Lua
/// 的 Button.WidgetTree.RootWidget:GetChildAt(n):GetText()。
///
/// 用法: ButtonTextProvider.TryGetText(button, out string text)
///
/// SKIP: 完整的分屏适配（如 BI_StartGame_C 的复杂树遍历）需要逐类
/// 型处理，对应 Lua 的 GetTextFuncMap 模式。此处只实现通用兜底逻辑。
/// </summary>
public static class ButtonTextProvider
{
    /// <summary>尝试读取按钮上显示的文字。返回是否能读取到。</summary>
    public static bool TryGetText(UUserWidget? rootWidget, out string text)
    {
        text = "";

        if (rootWidget == null || !rootWidget.IsValidLowLevel())
            return false;

        // 1. 尝试直接找 TxtName（多数按钮的通用命名）
        var widgets = new[] { "TxtName", "Content", "BI_TextLoop" };
        foreach (var name in widgets)
        {
            try
            {
                var w = GSUIUtil.FindChildWidget(rootWidget, name) as UTextBlock;
                if (w != null && w.IsValidLowLevel())
                {
                    text = w.GetText().ToString();
                    return !string.IsNullOrEmpty(text);
                }
            }
            catch { }
        }

        // 2. 尝试通过 BUI_Button 内部结构读取
        try
        {
            var btn = GSUIUtil.FindChildWidget(rootWidget, "BI_TextLoop") as UUserWidget;
            if (btn != null)
            {
                // BI_TextLoop -> 通常有 Content 属性
                var prop = btn.GetType().GetProperty("Content");
                if (prop != null)
                {
                    var content = prop.GetValue(btn);
                    if (content is UTextBlock tb)
                    {
                        text = tb.GetText().ToString();
                        return !string.IsNullOrEmpty(text);
                    }
                }
            }
        }
        catch { }

        return false;
    }
}
