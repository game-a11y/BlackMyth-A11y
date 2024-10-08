-- SPDX-License-Identifier: MIT
-- UI Hook 土地庙
-- Author: inkydragon
local SubModeName = "[BlackMythA11y.UIHooks] "
-- [[require Global]]

-- [[require Local]]
local WkGlobals = require("WkGlobals")
-- [[Global Var]]


-- [[ Hook 事件处理函数 ]] ----------------------------------------------------------

-- 游戏中:土地庙:菜单 BI_ShrineMenuParent_C
-- BUI_Tudi_Enter_C.WidgetTree.BI_ShrineFirMenu.WidgetTree.BI_Item.WidgetTree.BI_Item_10
--[[
    主地图  TextBlock   /BUI_Tudi_Enter_C.WidgetTree.TxtMainName
    地点    GSScaleText /BUI_Tudi_Enter_C.WidgetTree.TxtSubName

    [Reset]     BI_ShrineMenuParent_C   /BUI_Tudi_Enter_C.WidgetTree.BI_ShrineFirMenu.WidgetTree.BI_Item.WidgetTree.BI_Item_9
    文字描述    TextBlock   /BUI_Tudi_Enter_C.WidgetTree.TxtTips
]]
WkGlobals.GetTextFuncMap["BI_ShrineMenuParent_C"] = WkGlobals.GetTextFuncMap["BI_StartGame_C"]

-- 游戏中:土地庙:菜单:缩地（传送菜单）
--[[
BI_ShrineMenuChild_C.WidgetTree.RootWidget
Root
[0] BtnCon
 [0] ResizeCon
  [0] ImgBar
 [1] ResizeName
  [0] ImgUnusableMarker
  [1] ImgNPCIcon_Ck
  [2] ImgNPCIcon_Df
  [3] HorizontalBox_0
   [0] BI_TextLoop
     Default__BI_TextLoop_C
 [2] FocusWidget
 [3] MarkerCon
  [0] MarkerTeleport
  [1] ImgRedPoint
]]
WkGlobals.GetTextFuncMap["BI_ShrineMenuChild_C"] = WkGlobals.GetTextFuncMap["BI_StartGame_C"]
