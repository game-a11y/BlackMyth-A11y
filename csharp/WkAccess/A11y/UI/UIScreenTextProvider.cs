using CommB1;
using WkAccess.A11y.UE;

namespace WkAccess.A11y;

/// <summary>
/// UI 屏幕文本提供器 — 等效于 Lua 的 GetTextFuncMap。
/// 直接按名称搜索已知的文本控件。
/// 控件提取器通过 Register() 注入（由 B1 层的 B1WidgetExtractors.RegisterAll 完成）。
/// </summary>
public static class UIScreenTextProvider
{
    static readonly Dictionary<string, Func<UUserWidget, string?>> _providers = new();

    public static void Register(string className, Func<UUserWidget, string?> extractor)
    {
        _providers[className] = extractor;
    }

    public static string? Extract(UUserWidget? rootWidget)
    {
        if (rootWidget == null || !rootWidget.IsValidLowLevel())
            return null;

        var cn = WkUtils.GetClassName(rootWidget);
        if (_providers.TryGetValue(cn, out var func))
            return func(rootWidget);
        foreach (var kv in _providers)
            if (cn.StartsWith(kv.Key, StringComparison.Ordinal))
                return kv.Value(rootWidget);

        return FindAnyText(rootWidget);
    }

    /// <summary>在 UUserWidget 中搜索常见文本控件，取第一个有内容的。</summary>
    public static string? FindAnyText(UUserWidget root)
    {
        foreach (var name in new[] { "TxtName", "TxtTab", "Content", "TxtDesc", "BI_TextLoop",
            "TxtTips", "TxtNum", "TxtLevel", "TxtSpellType", "TxtSpellDesc",
            "TxtAbilityTitle", "TxtAbilityDesc", "TxtAbilityTypeTitle" })
        {
            try
            {
                var w = GSUIUtil.FindChildWidget(root, name);
                if (w == null || !w.IsValidLowLevel()) continue;

                if (w is UTextBlock tb)
                {
                    var t = tb.GetText().ToString();
                    if (!string.IsNullOrEmpty(t)) return t;
                }
                if (w is UUserWidget uw)
                {
                    var p = uw.GetType().GetProperty("Content");
                    if (p != null && p.GetValue(uw) is UTextBlock tb2)
                    {
                        var t = tb2.GetText().ToString();
                        if (!string.IsNullOrEmpty(t)) return t;
                    }
                    var nested = FindAnyText(uw);
                    if (nested != null) return nested;
                }
            }
            catch { }
        }
        return null;
    }
}
