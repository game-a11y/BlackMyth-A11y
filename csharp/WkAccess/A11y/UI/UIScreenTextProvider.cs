using GSE.GSUI;
using HarmonyLib;
using UnrealEngine.UMG;
using UnrealEngine.Runtime;
using b1.UI;
using B1UI;
using B1UI.GSUI;
using CommB1;
using b1.Localization;

namespace WkAccess.A11y.UI;

/// <summary>
/// UI 屏幕文本提供器 — 等效于 Lua 的 GetTextFuncMap。
/// 直接按名称搜索已知的文本控件。
/// </summary>
public static partial class UIScreenTextProvider
{
    static readonly Dictionary<string, Func<UUserWidget, string?>> _providers = new();

    /// <summary>全局唯一页面的缓存，由 H_PageConstruct 钩子填充</summary>
    internal static readonly Dictionary<string, UUserWidget> _pageCache = new();

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
        Register("BI_SettingMenuBtn_C",       Extract_SettingMenuBtn);
        Register("BI_SettingKeyItem_C",       Extract_SettingKeyItem);
        Register("BI_ShrineMenuParent_C",     Extract_ShrineMenu);
        Register("BI_ShrineMenuChild_C",      Extract_ShrineMenu);
        Register("BI_SpellPanelTitle_Btn_C",  Extract_SpellPanelTitle);
        Register("BI_TalentItem_",           Extract_TalentItem);
        Register("BI_AbilityIcon_KB_Basic_C", Extract_AbilityIcon_KB);
        Register("BI_AbilityIcon_KB_Advance_C",Extract_AbilityIcon_KB);
        Register("BI_AbilityIcon_GP_Basic_C", Extract_AbilityIcon_GP);
        Register("BI_AbilityIcon_GP_Advance_C",Extract_AbilityIcon_GP);
        Register("BI_InventoryItem_C",        Extract_InventoryItem);
        Register("BI_EquipItem_C",            Extract_EquipItem);
        Register("BI_EquipItem_Slot_C",       Extract_EquipItem);
        Register("BI_GearItem_Slot_C",        Extract_GearItem);
        Register("BI_QuickItem_C",            Extract_QuickItem);
        Register("BI_InteractIcon",           Extract_Interact);
        Register("BI_ReconfirmBtn_C",          Extract_ReconfirmBtn);
    }

    public static void Register(string className, Func<UUserWidget, string?> extractor)
    {
        _providers[className] = extractor;
    }

    public static string? Extract(UUserWidget? rootWidget)
    {
        if (rootWidget == null || !rootWidget.IsValidLowLevel())
            return null;

        var cn = UIFocusTracker.GetClassName(rootWidget);
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

    // ── 各 UI 类型的专用提取器 ──
}
