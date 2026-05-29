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
public static class UIScreenTextProvider
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
            var found = new List<string>();
            foreach (var s in GSG.GamePlayer.Actor.Wear.ShortcutsList.ValueList)
            {
                found.Add($"P{s.Position}=I{s.ItemId}");
                if (s.Position == position && s.ItemId > 0)
                {
                    var desc = GameDBRuntime.GetItemDesc(s.ItemId);
                    return CleanEquipName(desc?.Name);
                }
            }
            A11yLog.Debug($"[QuickItem] pos={position} shortcuts=[{string.Join(", ", found)}]");
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

    #region 设置菜单提取器

    /// <summary>标签页: 标签页 - {Tab名称}</summary>
    static string? Extract_SettingTab(UUserWidget w)
    {
        var label = FindTextByName(w, "TxtName");
        if (label != null) return $"标签页 - {label}";
        return FindAnyText(w);
    }

    /// <summary>
    /// 多选按钮: 多选按钮 - {设置项名称} - {当前值}
    /// 通过检测子控件 BI_Btn 是否存在来排除子控件焦点事件。
    /// </summary>
    static string? Extract_SettingFixedItem(UUserWidget w)
    {
        var biBtn = GSUIUtil.FindChildWidget(w, "BI_Btn") as UUserWidget;
        if (biBtn == null) return null; // 子控件焦点事件，抑制

        var label = FindTextByName(biBtn, "TxtName");
        var value = FindTextByName(w, "TxtDesc");
        if (label != null && value != null) return $"多选按钮 - {label} - {value}";
        if (label != null) return $"多选按钮 - {label}";
        return FindAnyText(w);
    }

    /// <summary>
    /// 下拉框: 下拉框 - {设置项名称} - {当前值}
    /// 路径: root → BI_Btn(1) → BI_Btn(2) → TxtName
    /// </summary>
    static string? Extract_SettingMenuItem(UUserWidget w)
    {
        var biBtn1 = GSUIUtil.FindChildWidget(w, "BI_Btn") as UUserWidget;
        if (biBtn1 == null) return null;

        var biBtn2 = GSUIUtil.FindChildWidget(biBtn1, "BI_Btn") as UUserWidget;
        var label = FindTextByName(biBtn2, "TxtName");
        var value = FindTextByName(biBtn1, "TxtDesc");
        if (label != null && value != null) return $"下拉框 - {label} - {value}";
        if (label != null) return $"下拉框 - {label}";
        return FindAnyText(w);
    }

    /// <summary>下拉选项: 下拉项 - {选项名}</summary>
    static string? Extract_ModeBtnItem(UUserWidget w)
    {
        var label = FindAnyText(w);
        if (label != null) return $"下拉项 - {label}";
        return null;
    }

    /// <summary>滑块: 拖动条 - {设置项名称} - {当前值}</summary>
    static string? Extract_SettingSliderItem(UUserWidget w)
    {
        var biBtn = GSUIUtil.FindChildWidget(w, "BI_Btn") as UUserWidget;
        if (biBtn == null) return null;

        var label = FindTextByName(biBtn, "TxtName");
        if (label != null)
        {
            var biSlider = GSUIUtil.FindChildWidget(w, "BI_Slider") as UUserWidget;
            if (biSlider != null)
            {
                var value = FindTextByName(biSlider, "TxtNum");
                if (value != null) return $"拖动条 - {label} - {value}";
                value = FindAnyText(biSlider);
                if (value != null) return $"拖动条 - {label} - {value}";
            }
            return $"拖动条 - {label}";
        }
        return FindAnyText(w);
    }

    /// <summary>图标按钮: 图标按钮 - {按钮名}</summary>
    static string? Extract_SettingIconItem(UUserWidget w)
    {
        var biBtn = GSUIUtil.FindChildWidget(w, "BI_Btn") as UUserWidget;
        if (biBtn == null) return null;

        var label = FindTextByName(biBtn, "TxtName");
        if (label != null) return $"图标按钮 - {label}";
        return FindAnyText(w);
    }

    /// <summary>文本按钮: 文本按钮 - {按钮名}</summary>
    static string? Extract_SettingMainBtn(UUserWidget w)
    {
        var label = FindTextByName(w, "TxtName");
        if (label != null) return $"文本按钮 - {label}";
        return FindAnyText(w);
    }

    /// <summary>设置菜单按钮: 包含嵌套 BI_SettingMainBtn (BI_Btn)，委托读取</summary>
    static string? Extract_SettingMenuBtn(UUserWidget w)
    {
        var biBtn = GSUIUtil.FindChildWidget(w, "BI_Btn") as UUserWidget;
        if (biBtn != null)
            return Extract_SettingMainBtn(biBtn);
        return FindAnyText(w);
    }

    /// <summary>按键配置: 按键配置 - {按钮名} [??]</summary>
    /// <remarks>TODO: 真实按键名需从 BGW_SettingMgrV2 数据层读取，当前无法从控件树获取。</remarks>
    static string? Extract_SettingKeyItem(UUserWidget w)
    {
        var biBtn = GSUIUtil.FindChildWidget(w, "BI_Btn") as UUserWidget;
        if (biBtn == null) return null;

        var label = FindTextByName(biBtn, "TxtName");
        if (label == null) return FindAnyText(w);
        return $"按键配置 - {label} [??]";
    }

    #endregion

    #region 其他 UI 提取器

    /// <summary>
    /// 主菜单按钮。
    /// 继续游戏（BI_StartGameBtn_0）额外读取关卡信息：TxtMainName / TxtSubName。
    /// </summary>
    static string? Extract_StartGame(UUserWidget w)
    {
        var text = FindAnyText(w);

        try
        {
            var name = w.GetFName().ToString();
            if (name == "BI_StartGameBtn_0")
            {
                // Outer 链：Widget -> WidgetTree -> UUserWidget(根控件)
                var root = w.GetOuter()?.GetOuter() as UUserWidget;
                if (root != null)
                {
                    var m = FindTextByName(root, "TxtMainName");
                    var s = FindTextByName(root, "TxtSubName");
                    if (m != null && s != null)
                        return $"{text} {m}: {s}";
                }
            }
        }
        catch { }

        return text;
    }
    static string? Extract_StartGameBtn(UUserWidget w) => FindAnyText(w);
    /// <summary>存档: 存档 {地点} {日期} {时间} {游戏时长} {时长值} {等级} {等级值}</summary>
    static string? Extract_ArchivesBtn(UUserWidget w)
    {
        var parts = new[] {
            "存档",
            FindTextByName(w, "TxtName"),
            FindTextByName(w, "TxtDate"),
            FindTextByName(w, "TxtTime"),
            FindTextByName(w, "TxtPlayTimeTitle"),
            FindTextByName(w, "TxtPlayTime"),
            FindTextByName(w, "TxtLvTitle"),
            FindTextByName(w, "TxtLv"),
        };
        var joined = string.Join(" ", parts.Where(p => p != null));
        if (joined.Length > 0) return joined;
        return FindAnyText(w);
    }

    /// <summary>确认按钮: {按钮文本} {待确认内容}。确定按钮（Btn_Confirm）读取父级确认内容。</summary>
    static string? Extract_ReconfirmBtn(UUserWidget w)
    {
        var text = FindAnyText(w);
        if (text == null) return null;

        try
        {
            var fname = w.GetFName().ToString();
            if (fname == "Btn_Confirm" || fname.StartsWith("Btn_Confirm"))
            {
                // 向上导航父级：Button → HBoxBtn → BtnCon → BoxCon
                var boxCon = w.GetParent();
                boxCon = boxCon?.GetParent();
                boxCon = boxCon?.GetParent();
                if (boxCon != null && boxCon.GetChildrenCount() > 1)
                {
                    var contentCon = boxCon.GetChildAt(1);
                    if (contentCon is UPanelWidget panel && panel.GetChildrenCount() > 0)
                    {
                        var txt = panel.GetChildAt(0);
                        if (txt != null)
                        {
                            // UGSRichScaleText 等非 UTextBlock 类型：反射调用 GetText()
                            var m = txt.GetType().GetMethod("GetText", System.Type.EmptyTypes);
                            var content = m?.Invoke(txt, null)?.ToString();
                            if (!string.IsNullOrEmpty(content))
                                return $"{text} {content}";
                        }
                    }
                }
            }
        }
        catch { }

        return text;
    }

    static string? Extract_FirstStartBtn(UUserWidget w) => FindAnyText(w);
    static string? Extract_ShrineMenu(UUserWidget w) => FindAnyText(w);
    /// <summary>法术面板标签: {标签名}</summary>
    static string? Extract_SpellPanelTitle(UUserWidget w)
    {
        var name = FindTextByName(w, "TxtName");
        if (name != null) return name;
        return FindAnyText(w);
    }
    /// <summary>根基技能: 根基技能 / 根基技能 Lv.{等级限制}</summary>
    static string? Extract_TalentItem(UUserWidget w)
    {
        var limit = FindTextByName(w, "TxtLevelLimit");
        if (limit != null) return $"根基技能 Lv.{limit}";
        return "根基技能";
    }
    /// <summary>键盘技能图标: 从父级 LeftRoot 读取技能描述</summary>
    /// <remarks>控件树无文本，向父级导航找到 LeftRoot → ContentAbilityRoot，
    /// 读取 TxtAbilityTitle / TxtAbilityTypeTitle。</remarks>
    static string? Extract_AbilityIcon_KB(UUserWidget w)
    {
        try
        {
            UWidget? parent = w.GetParent();
            while (parent != null && parent.IsValidLowLevel())
            {
                if (parent.GetFName().ToString() == "LeftRoot"
                    && parent is UPanelWidget leftRoot
                    && leftRoot.GetChildrenCount() > 0)
                {
                    var contentRoot = leftRoot.GetChildAt(0);
                    if (contentRoot is UPanelWidget crPanel && crPanel.GetChildrenCount() >= 4)
                    {
                        var getText = new Func<UWidget?, string?>(widget =>
                        {
                            if (widget == null || !widget.IsValidLowLevel()) return null;
                            var m = widget.GetType().GetMethod("GetText", Type.EmptyTypes);
                            return m?.Invoke(widget, null)?.ToString();
                        });
                        var title = getText(crPanel.GetChildAt(0));
                        var typeTitle = getText(crPanel.GetChildAt(2));
                        if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(typeTitle))
                            return $"{title}：{typeTitle}";
                        if (!string.IsNullOrEmpty(title))
                            return title;
                    }
                    break;
                }
                parent = parent.GetParent();
            }
        }
        catch { }
        return null;
    }
    /// <summary>手柄技能图标: 根基 / 棍法（控件树无文本，硬编码标签名）</summary>
    static string? Extract_AbilityIcon_GP(UUserWidget w)
    {
        try
        {
            var cn = w.GetClass().GetFName().ToString();
            if (cn.Contains("_Advance")) return "棍法";
        }
        catch { }
        return "根基";
    }
    /// <summary>行囊物品: 用品 [??] x{数量} | 用品 (空)</summary>
    /// <remarks>TODO: 物品名需从 Bag.BagItemList 获取 ItemDesc.Name，待实现</remarks>
    static string? Extract_InventoryItem(UUserWidget w)
    {
        try
        {
            var num = FindTextByName(w, "TxtNum");
            if (string.IsNullOrEmpty(num) || num == "0") return "用品 (空)";
            return $"用品 [??] x{num}";
        }
        catch { }
        return FindAnyText(w) ?? "用品 [??]";
    }

    /// <summary>随身之物: {物品名} x{数量} | 随身之物 (空)</summary>
    static string? Extract_QuickItem(UUserWidget w)
    {
        try
        {
            var num = FindTextByName(w, "TxtNum");

            var pos = GetQuickItemPosition(w);
            A11yLog.Debug($"[QuickItem] pos={pos} num={num}");
            if (pos >= 0)
            {
                var itemName = ResolveQuickItemNameByPos(pos);
                if (itemName != null)
                    return $"{itemName} x{num}";
                return "随身之物 (空)";
            }

            var empty = string.IsNullOrEmpty(num) || num == "0";
            if (empty) return "随身之物 (空)";
            return $"随身之物 [??] x{num}";
        }
        catch { }
        return FindAnyText(w) ?? "随身之物 [??]";
    }

    /// <summary>珍玩槽: {槽位名} - {物品名}</summary>
    static string? Extract_GearItem(UUserWidget w)
    {
        try
        {
            var name = w.GetFName().ToString();
            if (string.IsNullOrEmpty(name) || name.Contains("Default"))
                return "珍玩 [??]";

            string? slotLabel = name switch
            {
                "BI_EquipSlotItem_8" => "老葫芦",
                "BI_EquipSlotItem_9" => "珍玩·一",
                "BI_EquipSlotItem_10" => "珍玩·二",
                _ => null
            };

            var idx = ParseSlotIndex(name);
            if (idx >= 0)
            {
                var itemName = ResolveEquipName(idx);
                if (itemName != null)
                    return slotLabel != null ? $"{slotLabel} - {itemName}" : itemName;
                return slotLabel != null ? $"{slotLabel} (空)" : "珍玩 (空)";
            }

            return slotLabel != null ? $"{slotLabel} [??]" : $"珍玩 [??]";
        }
        catch { }
        return "珍玩 [??]";
    }

    /// <summary>装备槽: {物品名}</summary>
    static string? Extract_EquipItem(UUserWidget w)
    {
        try
        {
            var name = w.GetFName().ToString();
            if (string.IsNullOrEmpty(name) || name.Contains("Default"))
                return "装备 [??]";

            var idx = ParseSlotIndex(name);
            if (idx >= 0)
            {
                var itemName = ResolveEquipName(idx);
                if (itemName != null)
                    return itemName;
                return "装备 (空)";
            }

            return $"装备 {name} [??]";
        }
        catch { }
        return "装备 [??]";
    }
    /// <summary>交互提示: 交互 - {按键} - {提示}</summary>
    static string? Extract_Interact(UUserWidget w)
    {
        var item = FindTextByName(w, "TxtItem");
        var tips = FindTextByName(w, "TxtTips");
        if (item != null && tips != null) return $"交互 - {item} - {tips}";
        if (item != null) return $"交互 - {item}";
        if (tips != null) return $"交互 - {tips}";
        return FindAnyText(w);
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
