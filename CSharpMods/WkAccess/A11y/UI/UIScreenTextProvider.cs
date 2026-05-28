using GSE.GSUI;
using UnrealEngine.UMG;
using UnrealEngine.Runtime;

namespace WkAccess.A11y.UI;

/// <summary>
/// UI 屏幕文本提供器 — 等效于 Lua 的 GetTextFuncMap。
/// 直接按名称搜索已知的文本控件。
/// </summary>
public static class UIScreenTextProvider
{
    static readonly Dictionary<string, Func<UUserWidget, string?>> _providers = new();

    static UIScreenTextProvider()
    {
        Register("BI_StartGame_C",            Extract_StartGame);
        Register("BI_StartGameBtn_",          Extract_StartGameBtn);
        Register("BI_ArchivesBtnV2_C",        Extract_ArchivesBtn);
        Register("BI_FirstStartBtn_C",        Extract_FirstStartBtn);
        Register("BI_SettingTab_C",           Extract_SettingTab);
        Register("BI_SettingFixedItem_C",     Extract_SettingFixedItem);
        Register("BI_SettingMenuItem_C",      Extract_SettingMenuItem);
        Register("BI_ModeBtnItem_C",          Extract_ModeBtnItem);
        Register("BI_SettingSliderItem_C",    Extract_SettingSliderItem);
        Register("BI_SettingIconItem_C",      Extract_SettingIconItem);
        Register("BI_SettingMainBtn_C",       Extract_SettingMainBtn);
        Register("BI_SettingKeyItem_C",       Extract_SettingKeyItem);
        Register("BI_ShrineMenuParent_C",     Extract_ShrineMenu);
        Register("BI_ShrineMenuChild_C",      Extract_ShrineMenu);
        Register("BI_SpellPanelTitle_Btn_C",  Extract_SpellPanelTitle);
        Register("BI_TalentItem_1_1_C",       Extract_TalentItem);
        Register("BI_AbilityIcon_KB_Basic_C", Extract_AbilityIcon_KB);
        Register("BI_AbilityIcon_KB_Advance_C",Extract_AbilityIcon_KB);
        Register("BI_AbilityIcon_GP_Basic_C", Extract_AbilityIcon_GP);
        Register("BI_AbilityIcon_GP_Advance_C",Extract_AbilityIcon_GP);
        Register("BI_InventoryItem_C",        Extract_InventoryItem);
        Register("BI_EquipItem_Slot_C",       Extract_EquipItem);
        Register("BI_InteractIcon",           Extract_Interact);
    }

    public static void Register(string className, Func<UUserWidget, string?> extractor)
    {
        _providers[className] = extractor;
    }

    public static string? Extract(UUserWidget? rootWidget)
    {
        if (rootWidget == null || !rootWidget.IsValidLowLevel())
            return null;

        // 先尝试专用提取器
        var cn = rootWidget.GetType().Name;
        if (_providers.TryGetValue(cn, out var func))
            return func(rootWidget);
        foreach (var kv in _providers)
            if (cn.StartsWith(kv.Key, StringComparison.Ordinal))
                return kv.Value(rootWidget);

        // 通用兜底：搜索所有常见的文本子控件名称
        return FindAnyText(rootWidget);
    }

    /// <summary>在 UUserWidget 中搜索常见文本控件，取第一个有内容的。</summary>
    public static string? FindAnyText(UUserWidget root)
    {
        foreach (var name in new[] { "TxtName", "Content", "TxtDesc", "BI_TextLoop",
            "TxtTips", "TxtNum", "TxtLevel", "TxtSpellType", "TxtSpellDesc",
            "TxtAbilityTitle", "TxtAbilityDesc", "TxtAbilityTypeTitle" })
        {
            try
            {
                var w = GSUIUtil.FindChildWidget(root, name);
                if (w == null || !w.IsValidLowLevel()) continue;

                // UTextBlock
                if (w is UTextBlock tb)
                {
                    var t = tb.GetText().ToString();
                    if (!string.IsNullOrEmpty(t)) return t;
                }
                // UUserWidget (如 BI_TextLoop 是 UserWidget 含 Content 属性)
                if (w is UUserWidget uw)
                {
                    var p = uw.GetType().GetProperty("Content");
                    if (p != null && p.GetValue(uw) is UTextBlock tb2)
                    {
                        var t = tb2.GetText().ToString();
                        if (!string.IsNullOrEmpty(t)) return t;
                    }
                    // 递归
                    var nested = FindAnyText(uw);
                    if (nested != null) return nested;
                }
            }
            catch { }
        }
        return null;
    }

    // ── 各 UI 类型的专用提取器 ──

    static string? Extract_StartGame(UUserWidget w) => FindAnyText(w);
    static string? Extract_StartGameBtn(UUserWidget w) => FindAnyText(w);
    static string? Extract_ArchivesBtn(UUserWidget w) => FindAnyText(w);
    static string? Extract_FirstStartBtn(UUserWidget w) => FindAnyText(w);
    static string? Extract_SettingTab(UUserWidget w) => FindAnyText(w);
    static string? Extract_SettingFixedItem(UUserWidget w) => FindAnyText(w);
    static string? Extract_SettingMenuItem(UUserWidget w) => FindAnyText(w);
    static string? Extract_ModeBtnItem(UUserWidget w) => FindAnyText(w);
    static string? Extract_SettingSliderItem(UUserWidget w) => FindAnyText(w);
    static string? Extract_SettingIconItem(UUserWidget w) => FindAnyText(w);
    static string? Extract_SettingMainBtn(UUserWidget w) => FindAnyText(w);
    static string? Extract_SettingKeyItem(UUserWidget w) => FindAnyText(w);
    static string? Extract_ShrineMenu(UUserWidget w) => FindAnyText(w);
    static string? Extract_SpellPanelTitle(UUserWidget w) => FindAnyText(w);
    static string? Extract_TalentItem(UUserWidget w) => "根基技能";
    static string? Extract_AbilityIcon_KB(UUserWidget w) => FindAnyText(w);
    static string? Extract_AbilityIcon_GP(UUserWidget w) => FindAnyText(w);
    static string? Extract_InventoryItem(UUserWidget w) => FindAnyText(w);
    static string? Extract_EquipItem(UUserWidget w) => FindAnyText(w);
    static string? Extract_Interact(UUserWidget w) => FindAnyText(w);
}
