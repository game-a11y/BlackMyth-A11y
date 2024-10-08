-- SPDX-License-Identifier: MIT
-- UI Hook 交互/弹出提示
-- Author: inkydragon
local SubModeName = "[BlackMythA11y.UIHooks] "
-- [[require Global]]

-- [[require Local]]
local WkGlobals = require("WkGlobals")
-- [[Global Var]]


-- [[ Hook 事件处理函数 ]] ----------------------------------------------------------

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
