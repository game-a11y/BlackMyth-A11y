-- SPDX-License-Identifier: MIT
-- UI Hook 游记
-- Author: inkydragon
local SubModeName = "[BlackMythA11y.UIHooks] "
-- [[require Global]]

-- [[require Local]]
local WkGlobals = require("WkGlobals")
-- [[Global Var]]


-- [[ Hook 事件处理函数 ]] ----------------------------------------------------------

-- 游戏中:背包2:游记
--[[
BI_TravelNotesMain_Tab_1 <: BI_TravelNotesMain_Tab_C
    BI_TravelNotesMain_ListBar_1 <: BI_TravelNotesMain_ListBar_C
    BI_TravelNotesMain_ListBar_0 <: BI_TravelNotesMain_ListBar_C
]]
WkGlobals.GetTextFuncMap["BI_TravelNotesMain_Tab_C"] = function(Button, InFocusEvent)
    local ClassName = Button:GetFName():ToString()

    local TxtName_txt = ""
    if "BI_TravelNotesMain_Tab_0" == ClassName then
        TxtName_txt = "影神图"
    elseif "BI_TravelNotesMain_Tab_1" == ClassName then
        TxtName_txt = "妙诀"
    end

    return TxtName_txt
end

WkGlobals.GetTextFuncMap["BI_TravelNotesMain_ListBar_C"] = function(Button, InFocusEvent)
    local TxtName_txt = "游记 二级选项"
    return DevNote..TxtName_txt
end
