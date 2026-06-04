using WkAccess.A11y;
using WkAccess.A11y.UE;
using CommB1;

namespace WkAccess.BM.UI;

/// <summary>
/// 游戏特有控件文本提取器。通过 RegisterAll 注册到 UIScreenTextProvider。
/// </summary>
public static class B1WidgetExtractors
{
    public static void RegisterAll(Action<string, Func<UUserWidget, string?>> register)
    {
        register("BI_StartGame_C",            Extract_StartGame);
        register("BI_StartGameBtn_",          Extract_StartGameBtn);
        register("BI_ArchivesBtnV2_C",        Extract_ArchivesBtn);
        register("BI_FirstStartBtn_C",        Extract_FirstStartBtn);
        register("BI_SettingTab_C",           Extract_SettingTab);
        register("BI_SettingFixedItem_C",     Extract_SettingFixedItem);
        register("BI_SettingMenuItem_C",      Extract_SettingMenuItem);
        register("BI_ModeBtnItem_C",          Extract_ModeBtnItem);
        register("BI_SettingSliderItem_C",    Extract_SettingSliderItem);
        register("BI_SettingIconItem_C",      Extract_SettingIconItem);
        register("BI_SettingMainBtn_C",       Extract_SettingMainBtn);
        register("BI_SettingMenuBtn_C",       Extract_SettingMenuBtn);
        register("BI_SettingKeyItem_C",       Extract_SettingKeyItem);
        register("BI_ShrineMenuParent_C",     Extract_ShrineMenu);
        register("BI_ShrineMenuChild_C",      Extract_ShrineMenu);
        register("BI_SpellPanelTitle_Btn_C",  Extract_SpellPanelTitle);
        register("BI_TalentItem_",           Extract_TalentItem);
        register("BI_AbilityIcon_KB_Basic_C", Extract_AbilityIcon_KB);
        register("BI_AbilityIcon_KB_Advance_C",Extract_AbilityIcon_KB);
        register("BI_AbilityIcon_GP_Basic_C", Extract_AbilityIcon_GP);
        register("BI_AbilityIcon_GP_Advance_C",Extract_AbilityIcon_GP);
        register("BI_InventoryItem_C",        Extract_InventoryItem);
        register("BI_EquipItem_C",            Extract_EquipItem);
        register("BI_EquipItem_Slot_C",       Extract_EquipItem);
        register("BI_GearItem_Slot_C",        Extract_GearItem);
        register("BI_QuickItem_C",            Extract_QuickItem);
        register("BI_InteractIcon",           Extract_Interact);
        register("BI_ReconfirmBtn_C",          Extract_ReconfirmBtn);
        register("BUI_InputTipsOne",           Extract_InputTipsOne);
        register("BUI_InputActionIcon",        Extract_InputActionIcon);
    }

    #region 设置菜单提取器

    static string? Extract_SettingTab(UUserWidget w)
    {
        var label = B1WidgetResolvers.FindTextByName(w, "TxtName");
        if (label != null) return $"标签页 - {label}";
        return UIScreenTextProvider.FindAnyText(w);
    }

    static string? Extract_SettingFixedItem(UUserWidget w)
    {
        var biBtn = GSUIUtil.FindChildWidget(w, "BI_Btn") as UUserWidget;
        if (biBtn == null) return null;

        var label = B1WidgetResolvers.FindTextByName(biBtn, "TxtName");
        var value = B1WidgetResolvers.FindTextByName(w, "TxtDesc");
        if (label != null && value != null) return $"多选按钮 - {label} - {value}";
        if (label != null) return $"多选按钮 - {label}";
        return UIScreenTextProvider.FindAnyText(w);
    }

    static string? Extract_SettingMenuItem(UUserWidget w)
    {
        var biBtn1 = GSUIUtil.FindChildWidget(w, "BI_Btn") as UUserWidget;
        if (biBtn1 == null) return null;

        var biBtn2 = GSUIUtil.FindChildWidget(biBtn1, "BI_Btn") as UUserWidget;
        var label = B1WidgetResolvers.FindTextByName(biBtn2, "TxtName");
        var value = B1WidgetResolvers.FindTextByName(biBtn1, "TxtDesc");
        if (label != null && value != null) return $"下拉框 - {label} - {value}";
        if (label != null) return $"下拉框 - {label}";
        return UIScreenTextProvider.FindAnyText(w);
    }

    static string? Extract_ModeBtnItem(UUserWidget w)
    {
        var label = UIScreenTextProvider.FindAnyText(w);
        if (label != null) return $"下拉项 - {label}";
        return null;
    }

    static string? Extract_SettingSliderItem(UUserWidget w)
    {
        var biBtn = GSUIUtil.FindChildWidget(w, "BI_Btn") as UUserWidget;
        if (biBtn == null) return null;

        var label = B1WidgetResolvers.FindTextByName(biBtn, "TxtName");
        if (label != null)
        {
            var biSlider = GSUIUtil.FindChildWidget(w, "BI_Slider") as UUserWidget;
            if (biSlider != null)
            {
                var value = B1WidgetResolvers.FindTextByName(biSlider, "TxtNum");
                if (value != null) return $"拖动条 - {label} - {value}";
                value = UIScreenTextProvider.FindAnyText(biSlider);
                if (value != null) return $"拖动条 - {label} - {value}";
            }
            return $"拖动条 - {label}";
        }
        return UIScreenTextProvider.FindAnyText(w);
    }

    static string? Extract_SettingIconItem(UUserWidget w)
    {
        var biBtn = GSUIUtil.FindChildWidget(w, "BI_Btn") as UUserWidget;
        if (biBtn == null) return null;

        var label = B1WidgetResolvers.FindTextByName(biBtn, "TxtName");
        if (label != null) return $"图标按钮 - {label}";
        return UIScreenTextProvider.FindAnyText(w);
    }

    static string? Extract_SettingMainBtn(UUserWidget w)
    {
        var label = B1WidgetResolvers.FindTextByName(w, "TxtName");
        if (label != null) return $"文本按钮 - {label}";
        return UIScreenTextProvider.FindAnyText(w);
    }

    static string? Extract_SettingMenuBtn(UUserWidget w)
    {
        var biBtn = GSUIUtil.FindChildWidget(w, "BI_Btn") as UUserWidget;
        if (biBtn != null)
            return Extract_SettingMainBtn(biBtn);
        return UIScreenTextProvider.FindAnyText(w);
    }

    static string? Extract_SettingKeyItem(UUserWidget w)
    {
        var biBtn = GSUIUtil.FindChildWidget(w, "BI_Btn") as UUserWidget;
        if (biBtn == null) return null;

        var label = B1WidgetResolvers.FindTextByName(biBtn, "TxtName");
        if (label == null) return UIScreenTextProvider.FindAnyText(w);

        var keyName = ReadKeyFromIconWidget(w);
        return $"按键配置 - {label} - {keyName}";
    }

    /// <summary>从控件树中查找 UGSInputActionIcon 并读取按键名</summary>
    static string ReadKeyFromIconWidget(UUserWidget w, string iconWidgetName = "ImgKeyIcon")
    {
        try
        {
            var iconWidget = GSUIUtil.FindChildWidget(w, iconWidgetName);
            var keyName = Input.GSInputKeyReader.ReadKeyNameFromIcon(iconWidget);
            if (!string.IsNullOrEmpty(keyName))
                return keyName!;
        }
        catch { }

        // Fallback: 尝试读 TxtKeyName 文本
        var txtKey = B1WidgetResolvers.FindTextByName(w, "TxtKeyName");
        if (!string.IsNullOrEmpty(txtKey) && txtKey != "W")
            return txtKey!;

        return "[??]";
    }

    #endregion

    #region 其他 UI 提取器

    static string? Extract_StartGame(UUserWidget w)
    {
        var text = UIScreenTextProvider.FindAnyText(w);

        try
        {
            var name = w.GetFName().ToString();
            if (name == "BI_StartGameBtn_0")
            {
                var root = w.GetOuter()?.GetOuter() as UUserWidget;
                if (root != null)
                {
                    var m = B1WidgetResolvers.FindTextByName(root, "TxtMainName");
                    var s = B1WidgetResolvers.FindTextByName(root, "TxtSubName");
                    if (m != null && s != null)
                        return $"{text} {m}: {s}";
                }
            }
        }
        catch { }

        return text;
    }
    static string? Extract_StartGameBtn(UUserWidget w) => UIScreenTextProvider.FindAnyText(w);
    static string? Extract_ArchivesBtn(UUserWidget w)
    {
        var parts = new[] {
            "存档",
            B1WidgetResolvers.FindTextByName(w, "TxtName"),
            B1WidgetResolvers.FindTextByName(w, "TxtDate"),
            B1WidgetResolvers.FindTextByName(w, "TxtTime"),
            B1WidgetResolvers.FindTextByName(w, "TxtPlayTimeTitle"),
            B1WidgetResolvers.FindTextByName(w, "TxtPlayTime"),
            B1WidgetResolvers.FindTextByName(w, "TxtLvTitle"),
            B1WidgetResolvers.FindTextByName(w, "TxtLv"),
        };
        var joined = string.Join(" ", parts.Where(p => p != null));
        if (joined.Length > 0) return joined;
        return UIScreenTextProvider.FindAnyText(w);
    }

    static string? Extract_ReconfirmBtn(UUserWidget w)
    {
        var text = UIScreenTextProvider.FindAnyText(w);
        if (text == null) return null;

        try
        {
            var fname = w.GetFName().ToString();
            if (fname == "Btn_Confirm" || fname.StartsWith("Btn_Confirm"))
            {
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

    static string? Extract_FirstStartBtn(UUserWidget w) => UIScreenTextProvider.FindAnyText(w);
    static string? Extract_ShrineMenu(UUserWidget w) => UIScreenTextProvider.FindAnyText(w);
    static string? Extract_SpellPanelTitle(UUserWidget w)
    {
        var name = B1WidgetResolvers.FindTextByName(w, "TxtName");
        if (name != null) return name;
        return UIScreenTextProvider.FindAnyText(w);
    }
    static string? Extract_TalentItem(UUserWidget w)
    {
        var limit = B1WidgetResolvers.FindTextByName(w, "TxtLevelLimit");
        if (limit != null) return $"根基技能 Lv.{limit}";
        return "根基技能";
    }
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
    static string? Extract_InventoryItem(UUserWidget w)
    {
        try
        {
            var num = B1WidgetResolvers.FindTextByName(w, "TxtNum");
            if (string.IsNullOrEmpty(num) || num == "0") return "用品 (空)";
            return $"用品 [??] x{num}";
        }
        catch { }
        return UIScreenTextProvider.FindAnyText(w) ?? "用品 [??]";
    }

    static string? Extract_QuickItem(UUserWidget w)
    {
        try
        {
            var num = B1WidgetResolvers.FindTextByName(w, "TxtNum");

            var pos = B1WidgetResolvers.GetQuickItemPosition(w) - 1;
            A11yLog.Debug($"[QuickItem] pos={pos} num={num}");
            if (pos >= 0)
            {
                var itemName = B1WidgetResolvers.ResolveQuickItemNameByPos(pos);
                if (itemName != null)
                    return $"{itemName} x{num}";
                return "随身之物 (空)";
            }

            var empty = string.IsNullOrEmpty(num) || num == "0";
            if (empty) return "随身之物 (空)";
            return $"随身之物 [??] x{num}";
        }
        catch { }
        return UIScreenTextProvider.FindAnyText(w) ?? "随身之物 [??]";
    }

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

            var idx = B1WidgetResolvers.ParseSlotIndex(name);
            if (idx >= 0)
            {
                var itemName = B1WidgetResolvers.ResolveEquipName(idx);
                if (itemName != null)
                    return slotLabel != null ? $"{slotLabel} - {itemName}" : itemName;
                return slotLabel != null ? $"{slotLabel} (空)" : "珍玩 (空)";
            }

            return slotLabel != null ? $"{slotLabel} [??]" : $"珍玩 [??]";
        }
        catch { }
        return "珍玩 [??]";
    }

    static string? Extract_EquipItem(UUserWidget w)
    {
        try
        {
            var name = w.GetFName().ToString();
            if (string.IsNullOrEmpty(name) || name.Contains("Default"))
                return "装备 [??]";

            var idx = B1WidgetResolvers.ParseSlotIndex(name);
            if (idx >= 0)
            {
                var itemName = B1WidgetResolvers.ResolveEquipName(idx);
                if (itemName != null)
                    return itemName;
                return "装备 (空)";
            }

            return $"装备 {name} [??]";
        }
        catch { }
        return "装备 [??]";
    }
    static string? Extract_Interact(UUserWidget w)
    {
        var item = B1WidgetResolvers.FindTextByName(w, "TxtItem");
        var tips = B1WidgetResolvers.FindTextByName(w, "TxtTips");
        if (item != null && tips != null) return $"交互 - {item} - {tips}";
        if (item != null) return $"交互 - {item}";
        if (tips != null) return $"交互 - {tips}";
        return UIScreenTextProvider.FindAnyText(w);
    }

    static string? Extract_InputTipsOne(UUserWidget w)
    {
        var desc = B1WidgetResolvers.FindTextByName(w, "TxtDesc");
        var keyName = ReadKeyFromIconWidget(w, "InputIcon");

        if (!string.IsNullOrEmpty(keyName) && !string.IsNullOrEmpty(desc))
            return $"按键 {keyName}: {desc}";
        if (!string.IsNullOrEmpty(desc))
            return desc;
        if (!string.IsNullOrEmpty(keyName))
            return $"按键 {keyName}";
        return null;
    }

    static string? Extract_InputActionIcon(UUserWidget w)
    {
        var name = B1WidgetResolvers.FindTextByName(w, "TxtName");
        var iconWidget = GSUIUtil.FindChildWidget(w, "InputIcon");
        var keyName = Input.GSInputKeyReader.ReadKeyNameFromIcon(iconWidget);

        if (!string.IsNullOrEmpty(keyName) && !string.IsNullOrEmpty(name))
            return $"按键 {keyName}: {name}";
        if (!string.IsNullOrEmpty(name))
            return name;
        if (!string.IsNullOrEmpty(keyName))
            return $"按键 {keyName}";
        return UIScreenTextProvider.FindAnyText(w);
    }

    #endregion
}
