using GSE.GSUI;
using UnrealEngine.UMG;
using UnrealEngine.Runtime;

namespace WkAccess.A11y.UI;

/// <summary>
/// UI 屏幕文本提供器 — 等效于 Lua 的 GetTextFuncMap。
/// 每个 UI 按钮类型注册自己的文本提取回调。
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

    /// <summary>根据按钮实例提取文本。返回 null 表示无法提取。</summary>
    public static string? Extract(UUserWidget? rootWidget)
    {
        if (rootWidget == null || !rootWidget.IsValidLowLevel())
            return null;

        var cn = rootWidget.GetType().Name;

        // 精确匹配
        if (_providers.TryGetValue(cn, out var func))
            return func(rootWidget);

        // 前缀匹配（如 BI_StartGameBtn_0 ~ 12）
        foreach (var kv in _providers)
        {
            if (cn.StartsWith(kv.Key, StringComparison.Ordinal))
                return kv.Value(rootWidget);
        }

        return null;
    }

    // ── 辅助函数 ──

    static string? ChildText(UUserWidget root, params string[] path)
    {
        try
        {
            UWidget? cur = root;
            foreach (var seg in path)
            {
                if (cur is UUserWidget uw)
                    cur = GSUIUtil.FindChildWidget(uw, seg);
                else
                    return null;
            }
            if (cur is UTextBlock tb)
                return tb.GetText().ToString();
            // GSScaleText / GSInputRichTextBlock
            var prop = cur?.GetType().GetMethod("GetText");
            if (prop != null)
                return prop.Invoke(cur, null)?.ToString();
            return null;
        }
        catch { return null; }
    }


    // ── 各 UI 类型的文本提取 ──

    // 主菜单/首页按钮
    // Lua: UIHooks/HomeScreen.lua → BI_FirstStartBtn_C
    static string? Extract_FirstStartBtn(UUserWidget w) => ChildText(w, "RootWidget", "RootCon", "BtnCon", "TxtName");

    // Lua: BI_StartGame_C → BI_StartGameBtn_N
    static string? Extract_StartGame(UUserWidget w) => ChildText(w, "RootWidget", "MainCon", "ButtonList", "TxtName");
    static string? Extract_StartGameBtn(UUserWidget w) => ChildText(w, "RootWidget", "Content", "BI_TextLoop", "Content");

    // Lua: BI_ArchivesBtnV2_C 存档按钮
    static string? Extract_ArchivesBtn(UUserWidget w) => ChildText(w, "RootWidget", "CanvasPanel_0", "CanvasPanel_28", "InfoCon", "HorizontalBox_0", "TxtName");

    // 设置菜单
    // Lua: UIHooks/SettingMenu.lua
    static string? Extract_SettingTab(UUserWidget w) => ChildText(w, "RootWidget", "BtnCon", "TxtName");
    static string? Extract_SettingFixedItem(UUserWidget w) => ChildText(w, "RootWidget", "BtnCon", "BI_Btn", "RootWidget", "RootCon", "BtnCon", "HorizontalBox_0", "TxtName");
    static string? Extract_SettingMenuItem(UUserWidget w) => ChildText(w, "RootWidget", "CanvasPanel_0", "BtnCon", "BI_Btn", "RootWidget", "RootCon", "BtnCon", "HorizontalBox_0", "TxtName");
    static string? Extract_ModeBtnItem(UUserWidget w) => ChildText(w, "RootWidget", "BtnCon", "TxtName");
    static string? Extract_SettingSliderItem(UUserWidget w) => ChildText(w, "RootWidget", "CanvasPanel_71", "BI_Btn", "RootWidget", "RootCon", "BtnCon", "HorizontalBox_0", "TxtName");
    static string? Extract_SettingIconItem(UUserWidget w) => ChildText(w, "RootWidget", "BtnCon", "BI_Btn", "RootWidget", "RootCon", "BtnCon", "HorizontalBox_0", "TxtName");
    static string? Extract_SettingMainBtn(UUserWidget w) => ChildText(w, "RootWidget", "BtnCon", "HorizontalBox_0", "TxtName");
    static string? Extract_SettingKeyItem(UUserWidget w) => ChildText(w, "RootWidget", "BtnCon", "BI_Btn", "RootWidget", "RootCon", "BtnCon", "HorizontalBox_0", "TxtName");

    // 土地庙
    // Lua: UIHooks/Tudi.lua
    static string? Extract_ShrineMenu(UUserWidget w) => ChildText(w, "RootWidget", "Root", "BtnCon", "ResizeName", "BI_TextLoop", "Content");

    // 技能
    // Lua: UIHooks/Ability.lua
    static string? Extract_SpellPanelTitle(UUserWidget w) => ChildText(w, "RootWidget", "Root", "BtnCon", "SizeBoxName", "TxtName");
    static string? Extract_TalentItem(UUserWidget w) => "根基技能";
    static string? Extract_AbilityIcon_KB(UUserWidget w) => ChildText(w, "RootWidget", "Root", "ImgIcon");
    static string? Extract_AbilityIcon_GP(UUserWidget w) => ChildText(w, "RootWidget", "Root", "TxtName");

    // 道具
    // Lua: UIHooks/InventoryItem.lua
    static string? Extract_InventoryItem(UUserWidget w) => ChildText(w, "RootWidget", "Root", "ResizeCon", "TxtName");

    // 装备
    // Lua: UIHooks/EquipItem.lua
    static string? Extract_EquipItem(UUserWidget w) => ChildText(w, "RootWidget", "Root", "ResizeCon", "TxtName");

    // 交互提示
    // Lua: UIHooks/InteractIcon.lua
    static string? Extract_Interact(UUserWidget w) => ChildText(w, "RootWidget", "InteractIcon", "TxtTips");
}
