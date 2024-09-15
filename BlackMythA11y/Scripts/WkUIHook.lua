-- SPDX-License-Identifier: MIT
-- WuKong UI Hooks
-- Author: inkydragon
local SubModeName = "[BlackMythA11y.WkUIHook] "
-- [[require Global]]

-- [[require Local]]
local WkUtils = require("WkUtils")
-- [[Global Var]]
local WkUIHook = {}
-- 父类名 => GetText 函数
local GetTextFuncMap = {}


-- [[ 辅助函数 ]] -----------------------------------------------------------------

-- 打印出所有父级组件
local function PrintAllParents(CurWidget)
    if CurWidget == nil or (not CurWidget:IsValid()) then
        return
    end

    local FullName = CurWidget:GetFullName()
    -- local SuperClassName = CurWidget:GetClass():GetFName():ToString()
    -- local ClassName = CurWidget:GetFName():ToString()
    print(string.format("\t%s\n", FullName))

    PrintAllParents(CurWidget:GetParent())
end


-- [[ Hook 函数 ]] --------------------------------------------------------------

-- 首次加载 主界面
GetTextFuncMap["BI_FirstStartBtn_C"] = function(Button, InFocusEvent)
    -- CanvasPanel
    local WidgetTree_Root = Button.WidgetTree.RootWidget
    -- CanvasPanel
    local BtnCon = WidgetTree_Root:GetChildAt(0)
    -- GSScaleText
    local TxtName = BtnCon:GetChildAt(3)  -- .BtnCon.CanvasPanelSlot_5
    return TxtName:GetContent():ToString()
end

-- 主界面/主菜单: 所有按钮
--[[
<BI_StartGame_C>
    BI_StartGameBtn_0   继续游戏
    BI_StartGameBtn_1   新游戏
    BI_StartGameBtn_2   载入游戏
    BI_StartGameBtn_3   小曲
    BI_StartGameBtn_4   设置
    BI_StartGameBtn_5   退出

TODO:  首屏播报版本号
版本号  TextBlock   /.BUI_StartGame_C_2147456059.WidgetTree.TxtVersion
]]
GetTextFuncMap["BI_StartGame_C"] = function(Button, InFocusEvent)
    local ClassName = Button:GetFName():ToString()

    -- 按钮 [继续游戏] 选中时，输出当前关卡信息: .TxtMainName; .TxtSubName
    local CurLevelName_txt = ""
    if "BI_StartGameBtn_0" == ClassName then
        local MainListCon = Button:GetParent()
        local StartBtnCon = MainListCon:GetParent()
        local Overlay_0 = StartBtnCon:GetParent()
        local MainCon = Overlay_0:GetParent()  -- .WidgetTree.MainCon
        -- 
        local RegionName = MainCon:GetChildAt(0)
        local TxtMainName = RegionName:GetChildAt(3)
        local TxtMainName_txt = TxtMainName.Text:ToString()
        local HorizontalBox_1 = RegionName:GetChildAt(1)
        local TxtSubName = HorizontalBox_1:GetChildAt(1)
        local TxtSubName_txt = TxtSubName.Text:ToString()
        CurLevelName_txt = string.format(" %s: %s", TxtMainName_txt, TxtSubName_txt)
    end

    -- 当前按钮文本
    local ContinueBtnTxt = Button.BI_TextLoop.Content:GetContent():ToString()
    return ContinueBtnTxt .. CurLevelName_txt
end -- BI_StartGame_C

-- 主界面/主菜单/继续游戏 （无需特殊处理）
-- 主界面/主菜单/新游戏 （无需特殊处理）

-- 主界面/主菜单/载入游戏
--[[
InfoCon
    .TxtName:   游戏存档点名
    .TimeCon
        .TxtPlayTimeTitle: 游玩时间标题
        .TxtPlayTime: 游玩时间
        .TxtDate:   日期
        .TxtTime:   时间
    .TxtLvTitle: 等级标题
    .TxtLv:     等级

TODO:
- 读出当前存档数："当前存档 2/10"
]]
GetTextFuncMap["BI_ArchivesBtnV2_C"] = function(Button, InFocusEvent)
    local CanvasPanel_0 = Button.WidgetTree.RootWidget
    local CP28 = CanvasPanel_0:GetChildAt(0)  -- CPS0
    local InfoCon = CP28:GetChildAt(4)  -- CPS1
    -- InfoCon[1]
    local HBox0 = InfoCon:GetChildAt(1) -- SPS3
    local TxtName = HBox0:GetChildAt(0) -- HBS0
    -- InfoCon[2]: TimeCon[0-3]
    local TimeCon = InfoCon:GetChildAt(2)
    local TxtPlayTimeTitle = TimeCon:GetChildAt(0)
    local TxtPlayTime = TimeCon:GetChildAt(1)
    local TxtDate = TimeCon:GetChildAt(2)
    local TxtTime = TimeCon:GetChildAt(3)
    -- InfoCon[3]
    local LevelCon = InfoCon:GetChildAt(3)
    local TxtLvTitle = LevelCon:GetChildAt(0)
    local TxtLv = LevelCon:GetChildAt(1)

    -- TODO: 加上存档编号
    local SaveDescId_txt = "存档"
    local TxtName_txt =  TxtName:GetText():ToString()
    local TxtLvTitle_txt =  TxtLvTitle:GetText():ToString()
    local TxtLv_txt = TxtLv:GetText():ToString()
    local TxtDate_txt =  TxtDate:GetText():ToString()
    local TxtTime_txt =  TxtTime:GetText():ToString()
    local TxtPlayTimeTitle_txt =  TxtPlayTimeTitle:GetText():ToString()
    local TxtPlayTime_txt =  TxtPlayTime:GetText():ToString()

    local DescTable = {
        -- 存档地点
        SaveDescId_txt, TxtName_txt,
        -- 存档日期
        TxtDate_txt, TxtTime_txt,
        -- 游戏时长
        TxtPlayTimeTitle_txt, TxtPlayTime_txt,
        -- 等级
        TxtLvTitle_txt, TxtLv_txt
    }

    return table.concat(DescTable, " ")
end -- BI_ArchivesBtnV2_C

-- 主界面/主菜单/小曲 (音乐)
--[[
TODO:
- 读出功能按钮
- 歌曲编号
]]
GetTextFuncMap["BI_AccordionChildBtn_Echo_C"] = GetTextFuncMap["BI_StartGame_C"]

-- 主界面/主菜单/设置: 一级菜单
--[[
BI_SettingTab_0 ~ 9
    0. 控制器
    1. 键盘与鼠标
    2. 游戏
    3. 视角
    4. 语言
    5. 显示
    6. 画质
    7. 声音
    8. 辅助设置
    9. 退出游戏
]]
GetTextFuncMap["BI_SettingTab_C"] = function(Button, InFocusEvent)
    local BtnCon = Button.WidgetTree.RootWidget:GetChildAt(0)
    local TxtName = BtnCon:GetChildAt(2)  -- CPS3
    local TxtName_txt = TxtName:GetContent():ToString()  -- GSScaleText
    return TxtName_txt
end -- BI_SettingTab_C

-- 主界面/设置: 左右单项选择
--[[
GSScaleText /.BI_SettingFixedItem_0.WidgetTree.BI_Btn.WidgetTree.TxtName
TextBlock   /.BI_SettingFixedItem_0.WidgetTree.TxtDesc

TODO:
- 补充选项的详细说明
-- 主界面/设置/视角: 文本区分 控制器和键盘
]]
GetTextFuncMap["BI_SettingFixedItem_C"] = function(Button, InFocusEvent)
    local FullName = Button:GetFullName()
    local ClassName = Button:GetFName():ToString()

    -- NOTE: 这里基于 Button.BI_Btn
    local BtnCon = Button.BI_Btn.WidgetTree.RootWidget:GetChildAt(0)
    local HBox0 = BtnCon:GetChildAt(2)
    local TxtName = HBox0:GetChildAt(0)
    local TxtName_txt = TxtName:GetContent():ToString()  -- GSScaleText

    BtnCon = Button.WidgetTree.RootWidget:GetChildAt(0)
    local DescCon = BtnCon:GetChildAt(2)
    local CPanel165 = DescCon:GetChildAt(1)
    local SBox58 = CPanel165:GetChildAt(0)
    local TxtDesc = SBox58:GetChildAt(0)
    local TxtDesc_txt = TxtDesc:GetText():ToString()  -- TextBlock

    local BtnType = "左右单项选择"
    local A11yNote = ""
    if "控制器类型" == TxtName_txt then
        A11yNote = "无障碍提示：类型一是 XBox 手柄；类型二是 PS 手柄"
    elseif "输入类型" == TxtName_txt then
        A11yNote = "无障碍提示：输入类型用于切换键位布局，目前不支持自定义手柄键位，建议使用 Steam 自定义映射"
    end

    return string.format("%s %s %s %s", TxtName_txt, BtnType, TxtDesc_txt, A11yNote)
end -- BI_SettingFixedItem_C

-- 主界面/设置: 下拉单项选择
--[[
    GSScaleText /.BI_SettingMenuItem_0.WidgetTree.BI_Btn.WidgetTree.BI_Btn.WidgetTree.TxtName
    TextBlock   /.BI_SettingMenuItem_0.WidgetTree.BI_Btn.WidgetTree.TxtDesc

TODO:   下拉项读取
BI_ModeBtnItem_C /.BUI_Setting_C_2147476617.WidgetTree.BI_SettingMenuPage.WidgetTree.BI_Item.WidgetTree.BI_ModeBtnItem_C_2147475188
TextBlock        /..BUI_Setting_C_2147476617.WidgetTree.BI_SettingMenuPage.WidgetTree.BI_Item.WidgetTree.BI_ModeBtnItem_C_2147475188.WidgetTree.TxtName
]]
GetTextFuncMap["BI_SettingMenuItem_C"] = function(Button, InFocusEvent)
    -- print(string.format("Button=%s\n", Button:GetFullName()))
    -- PrintAllParents(Button)

    -- NOTE: 这里基于 Button.BI_Btn.BI_Btn
    local BtnCon = Button.BI_Btn.BI_Btn.WidgetTree.RootWidget:GetChildAt(0)
    local HBox0 = BtnCon:GetChildAt(2)
    local TxtSettingInfo = HBox0:GetChildAt(0)
    local TxtSettingInfo_txt = TxtSettingInfo:GetContent():ToString()  -- GSScaleText

    BtnCon = Button.BI_Btn.WidgetTree.RootWidget:GetChildAt(0)
    local MenuCon = BtnCon:GetChildAt(2)
    local SBox0 = MenuCon:GetChildAt(0)
    local TxtDesc = SBox0:GetChildAt(0)
    local TxtDesc_txt = TxtDesc:GetText():ToString()  -- TextBlock
    
    local A11yNote = ""
    if "文本语言" == TxtSettingInfo_txt then -- 设置/语言/1
        A11yNote = "无障碍提示：除汉语英语以外的语言，目前下拉选项无障碍输出有误，选择语言后可正确读取已选择的语言"
    end

    return string.format("%s %s %s %s", TxtSettingInfo_txt, "下拉单项选择", TxtDesc_txt, A11yNote)
end -- BI_SettingMenuItem_C

-- 下拉项
-- BI_ModeBtnItem_C
GetTextFuncMap["BI_ModeBtnItem_C"] = function(Button, InFocusEvent)
    -- print(string.format("Button=%s", Button:GetFullName()))
    local BtnCon = Button.WidgetTree.RootWidget:GetChildAt(0)
    -- print(string.format("BtnCon=%s", BtnCon:GetFullName()))
    local TxtName = BtnCon:GetChildAt(1) -- cps1
    -- print(string.format("TxtName=%s", TxtName:GetFullName()))
    local TxtName_txt = TxtName:GetText():ToString()  -- TextBlock
    -- TODO
    local A11yNote = "无障碍提示：除汉语英语以外的语言，目前无障碍输出有误。"
    return string.format("%s %s", TxtName_txt, "下拉项")
end -- BI_ModeBtnItem_C

-- 主界面/设置: 水平滑块
GetTextFuncMap["BI_SettingSliderItem_C"] = function(Button, InFocusEvent)
    local ClassName = Button:GetFName():ToString()

    -- BI_Btn.WidgetTree.TxtName
    local BI_Btn = Button.BI_Btn
    local RootCon = BI_Btn.WidgetTree.RootWidget
    local BtnCon = RootCon:GetChildAt(0)
    local HorizontalBox_1 = BtnCon:GetChildAt(2)
    local TxtName = HorizontalBox_1:GetChildAt(0)
    local TxtName_txt = TxtName.Text:ToString()  -- TextBlock

    -- BI_Slider.WidgetTree.TxtNum
    local BI_Slider = Button.BI_Slider
    local SliderBtn = BI_Slider.SliderBtn
    local TxtNum = SliderBtn:GetChildAt(3)
    local Volume = TxtNum:GetContent():ToString()  -- GSScaleText

    -- BI_Slider.WidgetTree.TxtMaxNum
    local RootCon2 = BI_Slider.WidgetTree.RootWidget
    local BtnCon2 = RootCon2:GetChildAt(0)
    local HorizontalBox_0 = BtnCon2:GetChildAt(0)
    local SizeBox_1 = HorizontalBox_0:GetChildAt(2)
    local TxtMaxNum = SizeBox_1:GetChildAt(0)
    local TxtMaxNum_txt = TxtMaxNum:GetText():ToString()  -- TextBlock

    -- BI_Slider.WidgetTree.TxtMinNum = 0
    return string.format("%s %s %s/%s", TxtName_txt, "水平滑块", Volume, TxtMaxNum_txt)
end -- BI_SettingSliderItem_C

-- 主界面/设置: 图标按钮
--[[
BI_SettingIconItem_C /.BUI_Setting_C_2147457164.WidgetTree.BI_SettingIconItem_0
GSScaleText          /   .BI_SettingIconItem_0.WidgetTree.BI_Btn.WidgetTree.TxtName

校准滑块
BI_SettingSliderL_C /.BUI_BrightnessSetting_C_2147454165.WidgetTree.BI_Slider
]]
GetTextFuncMap["BI_SettingIconItem_C"] = function(Button, InFocusEvent)
    local ClassName = Button:GetFName():ToString()

    local BI_Btn = Button.BI_Btn
    local RootCon = BI_Btn.WidgetTree.RootWidget
    local BtnCon = RootCon:GetChildAt(0)
    local HorizontalBox_1 = BtnCon:GetChildAt(2)
    local TxtName = HorizontalBox_1:GetChildAt(0)
    local TxtName_txt = TxtName.Text:ToString()  -- TextBlock

    local desc_txt = ""
    if "BI_KeyboradEnter" == ClassName then
        desc_txt = "调整键盘按键映射"
    elseif "BI_SettingIconItem_0" == ClassName then
        desc_txt = "调整画面亮度"
    end
    return string.format("%s %s %s", TxtName_txt, "图标按钮", desc_txt)
end -- BI_SettingIconItem_C

-- 主界面/设置: 文本按钮
--[[
BI_SettingMainBtn_C /.BUI_Setting_C_2147457164.WidgetTree.BI_SettingBtnItem_0
GSScaleText         /.BUI_Setting_C_2147457164.WidgetTree.BI_SettingBtnItem_0.WidgetTree.TxtName
]]
GetTextFuncMap["BI_SettingMainBtn_C"] = function(Button, InFocusEvent)
    local BtnCon = Button.WidgetTree.RootWidget:GetChildAt(0)
    local HBox0 = BtnCon:GetChildAt(2)
    local TxtName = HBox0:GetChildAt(0)
    local TxtName_txt = TxtName:GetText():ToString()
    local desc_txt = ""
    return string.format("%s %s %s", TxtName_txt, "文本按钮", desc_txt)
end -- BI_SettingMainBtn_C

-- 主界面/设置: 图标按钮-键盘键位
--[[
TODO:
]]
GetTextFuncMap["BI_SettingKeyItem_C"] = function(Button, InFocusEvent)
    -- PrintAllParents(Button)
    -- local BtnCon = Button.WidgetTree.RootWidget:GetChildAt(0)
    -- local KeyCon = BtnCon:GetChildAt(1)  -- CPS0
    -- local ReplaceCon = KeyCon:GetChildAt(1)  -- CPS11
    -- local TxtShuru = ReplaceCon:GetChildAt(1)  -- CPS1
    -- local TxtName_txt = TxtShuru:GetText():ToString()

    -- local TxtKeyName = KeyCon:GetChildAt(3)  -- CPS6
    -- local TxtKeyName_txt = TxtKeyName:GetText():ToString()
    -- return string.format("%s %s %s", TxtName_txt, "按键配置按钮", TxtKeyName_txt)
    return "开发中！ 键盘键位配置按钮"
end -- BI_SettingKeyItem_C


-- 二次确定对话框
--[[
BUI_Reconfirm_C_2147454606.WidgetTree.txt
BUI_Reconfirm_C_2147454606.WidgetTree.Btn_Confirm
BUI_Reconfirm_C_2147454606.WidgetTree.Btn_Cancel
]]
-- TODO: 构造时 BI_ReconfirmBtn_C 读出 .txt
-- GSRichScaleText /.BUI_Reconfirm_C_2147454627.WidgetTree.txt
-- /BUI_Reconfirm_C.WidgetTree.Btn_Cancel
GetTextFuncMap["BI_ReconfirmBtn_C"] = function(Button, InFocusEvent)
    local ClassName = Button:GetFName():ToString()

    -- CanvasPanel
    local RootCon = Button.WidgetTree.RootWidget
    -- CanvasPanel
    local HoverRoot = RootCon:GetChildAt(0)
    -- CanvasPanel
    local BtnCon = HoverRoot:GetChildAt(0)
    -- TextBlock
    local TxtName = BtnCon:GetChildAt(1)  -- .PSSlot2
    local TxtName_txt = TxtName:GetText():ToString()

    if "Btn_Confirm" == ClassName then
        local HBoxBtn = Button:GetParent()
        local BtnCon = HBoxBtn:GetParent()
        local BoxCon = BtnCon:GetParent()
        local ContentCon = BoxCon:GetChildAt(1)
        local txt = ContentCon:GetChildAt(0)
        local ContentCon_txt = txt:GetContent():ToString()
        -- print(string.format("txt=%s:  %s\n", txt:GetFullName(), ContentCon_txt))

        return string.format("%s %s", TxtName_txt, ContentCon_txt)
    end

    return TxtName_txt
end

-- TODO: 加载界面

-- 游戏中:土地庙:菜单 BI_ShrineMenuParent_C
-- BUI_Tudi_Enter_C.WidgetTree.BI_ShrineFirMenu.WidgetTree.BI_Item.WidgetTree.BI_Item_10
--[[
    主地图  TextBlock   /BUI_Tudi_Enter_C.WidgetTree.TxtMainName
    地点    GSScaleText /BUI_Tudi_Enter_C.WidgetTree.TxtSubName

    [Reset]     BI_ShrineMenuParent_C   /BUI_Tudi_Enter_C.WidgetTree.BI_ShrineFirMenu.WidgetTree.BI_Item.WidgetTree.BI_Item_9
    文字描述    TextBlock   /BUI_Tudi_Enter_C.WidgetTree.TxtTips
]]
GetTextFuncMap["BI_ShrineMenuParent_C"] = GetTextFuncMap["BI_StartGame_C"]

-- TODO: 游戏中:背包栏
--[[
背包顶层选项卡
BI_CommTxtTab_C     /.BUI_RoleMain_C_2147463565.WidgetTree.BI_RoleTab.WidgetTree.BI_SecTab.WidgetTree.BI_CommTxtTab_C_2147463554
    GSScaleText /.BI_RoleTab.WidgetTree.BI_SecTab.WidgetTree.BI_CommTxtTab_C_2147471225.WidgetTree.TxtName
BI_EquipItem_Slot_C /.BUI_EquipMain_C_2147465651.WidgetTree.BI_EquipSlotItem_1
    GSScaleText /.BI_RoleTab.WidgetTree.BI_SecTab.WidgetTree.BI_CommTxtTab_C_2147471217.WidgetTree.TxtName
BI_InventoryItem_C  /.BUI_BagMain_C_2147464172.WidgetTree.BI_TileView.WidgetTree.BI_InventoryItem_C_2147463997
    GSScaleText /.BI_RoleTab.WidgetTree.BI_SecTab.WidgetTree.BI_CommTxtTab_C_2147471209.WidgetTree.TxtName
BI_TravelNotesMain_ListBar_C /.BUI_TravelNotesMain_C_2147463697.WidgetTree.BI_TravelNotesMain_ListBar_0
    GSScaleText /.BI_RoleTab.WidgetTree.BI_SecTab.WidgetTree.BI_CommTxtTab_C_2147471201.WidgetTree.TxtName
BI_SettingTab_C     /.BUI_Setting_C_2147463587.WidgetTree.BI_SettingTab_0
    GSScaleText /.BI_RoleTab.WidgetTree.BI_SecTab.WidgetTree.BI_CommTxtTab_C_2147471193.WidgetTree.TxtName
]]

-- 游戏中:背包:技能
--[[
TODO: 实时获取文本

BI_AbilityIcon_KB_Basic_C
BI_AbilityIcon_KB_Advance_C

BI_AbilityIcon_GP_Basic_C   /.BUI_TalentMain_C_2147461288.WidgetTree.AbilityIcon1_GP
BI_AbilityIcon_GP_Advance_C /.BUI_TalentMain_C_2147461288.WidgetTree.AbilityIcon2_GP
]]
GetTextFuncMap["BI_AbilityIcon_KB_Basic_C"] = function(Button, InFocusEvent)
    local TxtName_txt = "根基"
    return TxtName_txt
end
GetTextFuncMap["BI_AbilityIcon_KB_Advance_C"] = function(Button, InFocusEvent)
    local TxtName_txt = "棍法"
    return TxtName_txt
end
GetTextFuncMap["BI_AbilityIcon_GP_Basic_C"] = function(Button, InFocusEvent)
    local TxtName_txt = "根基"
    return TxtName_txt
end
GetTextFuncMap["BI_AbilityIcon_GP_Advance_C"] = function(Button, InFocusEvent)
    local TxtName_txt = "棍法"
    return TxtName_txt
end

-- 游戏中:背包
--
GetTextFuncMap["BI_EquipItem_Slot_C"] = function(Button, InFocusEvent)
    local TxtName_txt = "背包物品"
    return TxtName_txt
end
--[[
BI_GearItem_Slot_C
GSRichScaleText /.BUI_EquipMain_C_2147461353.WidgetTree.TxtHuluTitleRuby
]]
GetTextFuncMap["BI_GearItem_Slot_C"] = function(Button, InFocusEvent)
    local SuperClassName = Button:GetClass():GetFName():ToString()
    local ClassName = Button:GetFName():ToString()

    local TxtName_txt = "珍玩物品"
    if "BI_EquipSlotItem_9" == ClassName then
        TxtName_txt = "珍玩·一"
    elseif "BI_EquipSlotItem_10" == ClassName then
        TxtName_txt = "珍玩·二"
    elseif "BI_EquipSlotItem_8" == ClassName then
        TxtName_txt = "老葫芦"
    end

    return TxtName_txt
end
-- GSRichScaleText /.BUI_EquipMain_C_2147461353.WidgetTree.TxtQuickItemTitleRuby
GetTextFuncMap["BI_QuickItem_C"] = function(Button, InFocusEvent)
    local TxtName_txt = "随身之物"
    return TxtName_txt
end

-- 游戏中:背包:游记
--[[
BI_TravelNotesMain_Tab_1 <: BI_TravelNotesMain_Tab_C
    BI_TravelNotesMain_ListBar_1 <: BI_TravelNotesMain_ListBar_C
    BI_TravelNotesMain_ListBar_0 <: BI_TravelNotesMain_ListBar_C
]]
GetTextFuncMap["BI_TravelNotesMain_Tab_C"] = function(Button, InFocusEvent)
    local ClassName = Button:GetFName():ToString()

    local TxtName_txt = ""
    if "BI_TravelNotesMain_Tab_0" == ClassName then
        TxtName_txt = "影神图"
    elseif "BI_TravelNotesMain_Tab_1" == ClassName then
        TxtName_txt = "妙诀"
    end

    return TxtName_txt
end
GetTextFuncMap["BI_TravelNotesMain_ListBar_C"] = function(Button, InFocusEvent)
    local TxtName_txt = "游记 二级选项"
    return TxtName_txt
end


-- TODO: 游戏中:(土地庙)提示
--[[
    E Offer Incense:    TextBlock /BUI_BattleInfo_C_2147446787.WidgetTree.BI_InteractIcon.WidgetTree.InteractIcon_98_0.WidgetTree.TxtTips

    CanvasPanel /.BUI_BattleInfo_C_2147446787.WidgetTree.BI_InteractIcon.WidgetTree.InteractIcon_98_0.WidgetTree.TipsCon
        可以用 TipsCon 的 StructProperty /Script/UMG.Widget:RenderTransform
            StructProperty /Script/UMG.WidgetTransform:Translation - X
            < 0 == -50 隐藏
            ==0     显示
    或 CanvasPanel /Engine/Transient.GameEngine_2147482611:BGW_GameInstance_B1_2147482576.BUI_BattleInfo_C_2147446787.WidgetTree.BI_InteractIcon.WidgetTree.InteractIcon_98_0.WidgetTree.TipsRoot
        RenderOpacity 
]]
-- TODO: 游戏中:可拾取物品 提示
--[[
    Gather:    TextBlock /BUI_BattleInfo_C_2147446787.WidgetTree.BI_InteractIcon.WidgetTree.InteractIcon_98_1.WidgetTree.TxtTips
]]

-- TODO: 游戏中:物品掉落\经验
--[[
物品掉落
    BUI_DropMain_C_2147444306.WidgetTree.BI_DropManual.WidgetTree.TxtName
    BUI_DropMain_C_2147444306.WidgetTree.BI_DropManual.WidgetTree.TxtSubDecs
    
经验
    BUI_DropMain_C_2147444306.WidgetTree.BI_DropExpProg_V2.WidgetTree.TxtLevel
    BUI_DropMain_C_2147444306.WidgetTree.BI_DropExpProg_V2.WidgetTree.TxtNumExp
]]


local function OnAddedToFocusPath_Hook(pContext, pInFocusEvent)
    local Button = pContext:get()
    local SuperClassName = Button:GetClass():GetFName():ToString()
    local ClassName = Button:GetFName():ToString()

    -- TODO: 根据原因筛选相应，消除重复的事件
    local InFocusEvent = pInFocusEvent:get()
    -- WkUtils.PrintUObject(TxtName_name)
    -- https://github.com/UE4SS-RE/RE-UE4SS/issues/378
    -- local UGSE_UMGFuncLib = StaticFindObject("/Script/UnrealExtent.Default__GSE_UMGFuncLib")
    -- Cause = UGSE_UMGFuncLib:GetFocusEventCause(InFocusEvent)

    print(string.format(
        "OnAddedToFocusPath(%s): %s\n",
        tostring(InFocusEvent:GetFullName()),
        tostring(Button:GetFullName())))

    -- Print text
    if GetTextFuncMap[SuperClassName] then
        -- 默认打断输出
        InterruptSpeak = true
        local curText = GetTextFuncMap[SuperClassName](Button, InFocusEvent)
        print(string.format("\t\"%s\"\n", curText))
        A11yTolk:Speak(curText, InterruptSpeak)
    else
        print(string.format("Cannot gettext for:  %s <: %s\n", ClassName, SuperClassName))
    
        -- -- CanvasPanel
        -- RootCon = Button.WidgetTree.RootWidget
        -- WkUtils.PrintUObject(RootCon)
        -- -- CanvasPanel
        -- HoverRoot = RootCon:GetChildAt(0)
        -- WkUtils.PrintUObject(HoverRoot)
        -- -- CanvasPanel
        -- BtnCon = HoverRoot:GetChildAt(0)
        -- WkUtils.PrintUObject(BtnCon)
        -- -- TextBlock
        -- TxtName = BtnCon:GetChildAt(1)  -- .PSSlot2
        -- WkUtils.PrintUObject(TxtName)
        -- print(string.format("\t%s\n", TxtName:GetText():ToString()))
        
        -- TxtName = ps:GetContent()
        -- WkUtils.PrintUObject(TxtName)
        -- TxtName:GetContent():ToString()
    end
end

-- 初始化所有 UI 钩子
function WkUIHook.InitUiHooks()
    RegisterHook("/Script/b1-Managed.BUI_Button:OnAddedToFocusPath", OnAddedToFocusPath_Hook)
    -- RegisterHook("/Script/b1-Managed.BUI_Widget:OnAddedToFocusPath", OnAddedToFocusPath_Hook)

    -- 开发中...
    RegisterHook("/Script/b1-Managed.BUI_Button:OnMouseButtonDown", function(Context, MyGeometry, MouseEvent)
        local Button = Context:get()
        print(string.format("OnMouseButtonDown: %s\n", tostring(Button:GetFullName())))
        -- WidgetTree = Button.WidgetTree
        -- WkUtils.PrintUObject(WidgetTree)
        -- WkUtils.PrintUObject(WidgetTree.TxtName)
    
        -- -- TxtName_name = tostring(Button:GetFullName()) .. ".WidgetTree.TxtName"
        -- WkUtils.PrintUObject(Button)
        -- -- TxtName = FindFirstOf("GSTextBlock /.BUI_StartGame_C_2147477556.WidgetTree.BI_StartGameBtn_0")
        -- -- WkUtils.PrintUObject(TxtName)
    
        -- TxtName = WidgetTree.TxtName:get()
        -- WkUtils.PrintUObject(WidgetTree)
        -- if TxtName ~= nil then
        --     BtnText = TxtName
        --     print(string.format("\t%s\n", tostring(TxtName:GetFullName())))
        -- end
    end)

    -- 返回的是上一步的对象
    -- RegisterHook("/Script/b1-Managed.BUI_Button:OnCustomWidgetNavigation", function(Context, Navigation)
    --     Button = Context:get()
    --     print(string.format("OnCustomWidgetNavigation: %s\n", tostring(Button:GetFullName())))
    -- end)
    
    -- NOTE: 会显示父级的聚焦事件
    -- RegisterHook("/Script/b1-Managed.BUI_Widget:OnFocusChanging", function(self, InFocusEvent)
    --     print(string.format("OnFocusChanging: %s\n", tostring(self:get():GetFullName())))
    -- end)

    -- NOTE: hook "/Script/b1-Managed.BUI_Widget:OnAnimationSequenceEvent" 闪退
end


return WkUIHook
