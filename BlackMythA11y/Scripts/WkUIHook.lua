-- SPDX-License-Identifier: MIT
-- WuKong UI Hooks
local SubModeName = "[BlackMythA11y.WkUIHook] "
-- Author: inkydragon
local WkUtils = require("WkUtils")
local WkUIHook = {}


-- 父类名 => GetText 函数
local GetTextFuncMap = {}

-- 主界面
-- BI_StartGame_C /Game/00Main/UI/BluePrintsV3/Btn/BI_StartGame.Default__BI_StartGame_C
-- WidgetBlueprintGeneratedClass /Game/00Main/UI/BluePrintsV3/Btn/BI_StartGame.BI_StartGame_C
GetTextFuncMap["BI_StartGame_C"] = function(uObject, InFocusEvent)
    return Button.BI_TextLoop.Content:GetContent():ToString()
end

-- 主界面/设置: 一级菜单
-- BI_SettingTab_C /Game/00Main/UI/BluePrintsV3/Setting/BUI_Setting.BUI_Setting_C:WidgetTree.BI_SettingTab_9
GetTextFuncMap["BI_SettingTab_C"] = function(uObject, InFocusEvent)
    -- CanvasPanel
    WidgetTree_Root = Button.WidgetTree.RootWidget
    -- CanvasPanel
    BtnCon = WidgetTree_Root:GetChildAt(0)
    -- GSScaleText
    TxtName = BtnCon:GetChildAt(2)
    return TxtName:GetContent():ToString()
end


local function OnAddedToFocusPath_Hook(pContext, pInFocusEvent)
    Button = pContext:get()
    SuperClassName = Button:GetClass():GetFName():ToString()
    ClassName = Button:GetFName():ToString()

    -- TODO: 根据原因筛选相应，消除重复的事件
    InFocusEvent = pInFocusEvent:get()
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
        local curText = GetTextFuncMap[SuperClassName](Button, InFocusEvent)
        print(string.format("\t%s\n", curText))
    else
        print(string.format("Cannot gettext for:  %s <: %s\n", ClassName, SuperClassName))
    end
end

-- 初始化所有 UI 钩子
function WkUIHook.InitUiHooks()
    RegisterHook("/Script/b1-Managed.BUI_Button:OnAddedToFocusPath", OnAddedToFocusPath_Hook)
    -- RegisterHook("/Script/b1-Managed.BUI_Widget:OnAddedToFocusPath", OnAddedToFocusPath_Hook)
end


return WkUIHook
