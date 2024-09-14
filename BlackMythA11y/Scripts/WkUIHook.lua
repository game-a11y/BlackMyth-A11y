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


-- 首次加载主界面 BI_FirstStartBtn_C
GetTextFuncMap["BI_FirstStartBtn_C"] = function(Button, InFocusEvent)
    -- CanvasPanel
    local WidgetTree_Root = Button.WidgetTree.RootWidget
    -- CanvasPanel
    local BtnCon = WidgetTree_Root:GetChildAt(0)
    -- GSScaleText
    local TxtName = BtnCon:GetChildAt(3)  -- .BtnCon.CanvasPanelSlot_5
    return TxtName:GetContent():ToString()
end

-- 主界面
-- TODO
--[[
    主地图  TextBlock   /BUI_StartGame_C_2147456059.WidgetTree.TxtMainName
    地点    TextBlock   /BUI_StartGame_C_2147456059.WidgetTree.TxtSubName
    版本号  TextBlock   /BUI_StartGame_C_2147456059.WidgetTree.TxtVersion
]]
GetTextFuncMap["BI_StartGame_C"] = function(Button, InFocusEvent)
    local ClassName = Button:GetFName():ToString()
    -- 当前关卡
    local TxtMainName_txt = ""
    local TxtSubName_txt = ""
    if "BI_StartGameBtn_0" == ClassName then
        local VerticalBoxSlot_0 = Button.Slot
        local MainListCon = VerticalBoxSlot_0.Parent
        local VerticalBoxSlot_2 = MainListCon.Slot
        local StartBtnCon = VerticalBoxSlot_2.Parent
        local OverlaySlot_0 = StartBtnCon.Slot
        local Overlay_0 = OverlaySlot_0.Parent
        local CanvasPanelSlot_3 = Overlay_0.Slot
        local MainCon = CanvasPanelSlot_3.Parent
        -- 
        local RegionName = MainCon:GetChildAt(0)
        local TxtMainName = RegionName:GetChildAt(3)
        TxtMainName_txt = TxtMainName.Text:ToString()
        -- print(string.format("MainNameText: %s\n", TxtMainName_txt))
        local HorizontalBox_1 = RegionName:GetChildAt(1)
        local TxtSubName = HorizontalBox_1:GetChildAt(1)
        TxtSubName_txt = TxtSubName.Text:ToString()
        -- print(string.format("TxtSubName_txt: %s\n", TxtSubName_txt))
    end

    local ContinueBtnTxt = Button.BI_TextLoop.Content:GetContent():ToString()
    return string.format("%s %s: %s", ContinueBtnTxt, TxtMainName_txt, TxtSubName_txt)
end

-- 主界面/设置: 一级菜单
-- BI_SettingTab_C /Game/00Main/UI/BluePrintsV3/Setting/BUI_Setting.BUI_Setting_C:WidgetTree.BI_SettingTab_9
GetTextFuncMap["BI_SettingTab_C"] = function(Button, InFocusEvent)
    -- CanvasPanel
    local WidgetTree_Root = Button.WidgetTree.RootWidget
    -- CanvasPanel
    local BtnCon = WidgetTree_Root:GetChildAt(0)
    -- GSScaleText
    local TxtName = BtnCon:GetChildAt(2)
    return TxtName:GetContent():ToString()
end

-- 主界面.加载存档
-- TextBlock /BI_ArchivesBtnV2_C_2147480738.WidgetTree.TxtDate
--[[
    InfoCon
    .TimeCon
        .TxtName:   游戏存档点名
        .TxtLv:     等级
        .TxtDate:   日期
        .TxtTime:   时间
        .TxtPlayTime: 游玩时间
]]
GetTextFuncMap["BI_ArchivesBtnV2_C---"] = function(Button, InFocusEvent)
    -- CanvasPanel
    local WidgetTree_Root = Button.WidgetTree.RootWidget
    -- CanvasPanel
    local BtnCon = WidgetTree_Root:GetChildAt(0)
    -- GSScaleText
    local TxtName = BtnCon:GetChildAt(2)
    return ""
end

-- 主界面/音乐
-- BI_AccordionChildBtn_Echo_C /Engine/Transient.GameEngine_2147482611:BGW_GameInstance_B1_2147482576.BUI_B1_Root_V2_C_2147462481.WidgetTree.BUI_SoundtrackV2_C_2147455841.WidgetTree.BI_ContentBtn.WidgetTree.BI_AccordionChildBtn_Echo_C_2147455810
GetTextFuncMap["BI_AccordionChildBtn_Echo_C"] = GetTextFuncMap["BI_StartGame_C"]

-- 二次确定对话框
-- TODO: 构造时 BI_ReconfirmBtn_C 读出 .txt
-- /BUI_Reconfirm_C.WidgetTree.Btn_Cancel
GetTextFuncMap["BI_ReconfirmBtn_C"] = function(Button, InFocusEvent)
    -- CanvasPanel
    local RootCon = Button.WidgetTree.RootWidget
    -- CanvasPanel
    local HoverRoot = RootCon:GetChildAt(0)
    -- CanvasPanel
    local BtnCon = HoverRoot:GetChildAt(0)
    -- TextBlock
    local TxtName = BtnCon:GetChildAt(1)  -- .PSSlot2
    return TxtName:GetText():ToString()
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

-- TODO: 游戏中:背包

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
        -- -- TxtName = FindFirstOf("GSTextBlock /Engine/Transient.GameEngine_2147482611:BGW_GameInstance_B1_2147482576.BUI_B1_Root_V2_C_2147480953.WidgetTree.BUI_StartGame_C_2147477556.WidgetTree.BI_StartGameBtn_0")
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
