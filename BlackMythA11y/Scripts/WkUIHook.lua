-- SPDX-License-Identifier: MIT
-- WuKong UI Hooks
local SubModeName = "[BlackMythA11y.WkUIHook] "
-- Author: inkydragon
local WkUIHook = {}


local function OnAddedToFocusPath_Hook(pContext, pInFocusEvent)
    Button = pContext:get()
    -- TODO: 根据原因筛选相应，消除重复的事件
    InFocusEvent = pInFocusEvent:get()
    -- WkUtils.PrintUObject(TxtName_name)
    -- https://github.com/UE4SS-RE/RE-UE4SS/issues/378
    -- local UGSE_UMGFuncLib = StaticFindObject("/Script/UnrealExtent.Default__GSE_UMGFuncLib")
    -- Cause = UGSE_UMGFuncLib:GetFocusEventCause(InFocusEvent)

    print(string.format("OnAddedToFocusPath(%s): %s\n", tostring(InFocusEvent:GetFullName()), tostring(Button:GetFullName())))
    TxtName = Button.BI_TextLoop.Content
    print(string.format("\t%s\n", tostring(TxtName:GetContent():ToString())))
end

-- 初始化所有 UI 钩子
function WkUIHook.InitUiHooks()
    RegisterHook("/Script/b1-Managed.BUI_Button:OnAddedToFocusPath", OnAddedToFocusPath_Hook)
    -- RegisterHook("/Script/b1-Managed.BUI_Widget:OnAddedToFocusPath", OnAddedToFocusPath_Hook)
end


return WkUIHook
