using GSE.GSUI;
using HarmonyLib;
using UnrealEngine.UMG;
using UnrealEngine.Runtime;
using b1.UI;
using B1UI;
using B1UI.GSUI;
using CommB1;
using b1.Localization;

namespace WkAccess.A11y.UI.Extraction;

public static partial class UIScreenTextProvider
{

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

            var pos = GetQuickItemPosition(w) - 1; // 父容器索引 0 为非 slot 控件
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
