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

    #region 通用辅助

    /// <summary>在指定根控件下按名称查找文本控件并返回其文本。</summary>
    static string? FindTextByName(UUserWidget? root, string childName)
    {
        if (root == null || !root.IsValidLowLevel()) return null;
        try
        {
            var w = GSUIUtil.FindChildWidget(root, childName);
            if (w == null || !w.IsValidLowLevel()) return null;
            if (w is UTextBlock tb)
                return tb.GetText()?.ToString();
            // 反射 GetText() 处理 GSRichScaleText / GSScaleText 等非 UTextBlock 文本控件
            var m = w.GetType().GetMethod("GetText", Type.EmptyTypes);
            if (m != null)
            {
                var t = m.Invoke(w, null)?.ToString();
                if (!string.IsNullOrEmpty(t)) return t;
            }
            if (w is UUserWidget uw)
            {
                var p = uw.GetType().GetProperty("Content");
                if (p?.GetValue(uw) is UTextBlock tb2)
                    return tb2.GetText()?.ToString();
                return FindAnyText(uw);
            }
        }
        catch { }
        return null;
    }

    /// <summary>清理 EquipName：占位符检测 + ToFText 解析本地化</summary>
    static string? CleanEquipName(string? raw)
    {
        if (string.IsNullOrEmpty(raw)) return null;
        if (raw.Contains("名字名字")) return null;
        try
        {
            // ToFText() 解析本地化 key（如 EquipDesc.15005.EquipName → "柳木棍"）
            var text = raw.ToFText().ToString();
            if (string.IsNullOrEmpty(text) || text.Contains("名字名字")) return null;
            // 剥离 ruby 注音标签
            var cleaned = System.Text.RegularExpressions.Regex.Replace(text, "<[^>]+>", "");
            return string.IsNullOrEmpty(cleaned) ? null : cleaned;
        }
        catch { return raw; }
    }

    /// <summary>从控件树确定 QuickItem 的位置（在同级兄弟中的序号）</summary>
    static int GetQuickItemPosition(UUserWidget w)
    {
        try
        {
            var parent = w.GetParent();
            if (parent != null)
            {
                var count = parent.GetChildrenCount();
                for (int i = 0; i < count; i++)
                {
                    if (parent.GetChildAt(i) == w)
                        return i;
                }
            }
        }
        catch { }
        return -1;
    }

    /// <summary>从快捷物品数据解析物品名</summary>
    static string? ResolveQuickItemNameByPos(int position)
    {
        try
        {
            if (GSG.GamePlayer == null) return null;
            if (position < 0) return null;
            foreach (var s in GSG.GamePlayer.Actor.Wear.ShortcutsList.ValueList)
            {
                if (s.Position == position && s.ItemId > 0)
                {
                    var desc = GameDBRuntime.GetItemDesc(s.ItemId);
                    return CleanEquipName(desc?.Name);
                }
            }
        }
        catch { }
        return null;
    }

    /// <summary>从玩家装备数据解析物品名（绕过 UI 时序问题）</summary>
    static string? ResolveEquipName(int slotIdx)
    {
        try
        {
            var slotType = (EEquipSlotType)slotIdx;
            var position = DSEquipMain.GetEquipTypeBySlotType(slotType);
            if (GSG.GamePlayer == null) return null;
            var wearList = GSG.GamePlayer.Actor.Wear.EquipList.ValueList;
            foreach (var w in wearList)
            {
                if (w.Position == position)
                {
                    var desc = GameDBRuntime.GetEquipDesc(w.Id);
                    return CleanEquipName(desc?.EquipName);
                }
            }
        }
        catch { }
        return null;
    }

    /// <summary>从 slot FName 解析索引（"BI_EquipSlotItem_5" → 5）</summary>
    static int ParseSlotIndex(string fname)
    {
        var i = fname.LastIndexOf('_');
        if (i >= 0 && int.TryParse(fname.Substring(i + 1), out var idx))
            return idx;
        return -1;
    }

    #endregion
}

/// <summary>挂钩 BUI_Widget.Construct，缓存全局唯一页面引用</summary>
[HarmonyPatch(typeof(BUI_Widget), "Construct_Implementation")]
static class H_PageConstruct
{
    static readonly string[] _targetPages = {
        "BUI_EquipMain_C", "BUI_BagMain_C", "BUI_TalentMain_C",
        "BUI_LearnTalent_C", "BUI_TravelNotesMain_C", "BUI_RoleMain_C"
    };

    static void Postfix(BUI_Widget __instance)
    {
        var cn = __instance.GetClass().GetFName().ToString();
        foreach (var p in _targetPages)
        {
            if (cn == p)
            {
                UIScreenTextProvider._pageCache[cn] = __instance;
                A11yLog.Info($"[PageCache] 缓存页面: {cn}");
                return;
            }
        }
    }
}
