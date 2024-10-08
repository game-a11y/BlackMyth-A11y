-- SPDX-License-Identifier: MIT
-- UI Hook 行囊（道具物品）
-- Author: inkydragon
local SubModeName = "[BlackMythA11y.UIHooks] "
-- [[require Global]]

-- [[require Local]]
local WkGlobals = require("WkGlobals")
-- [[Global Var]]


-- [[ Hook 事件处理函数 ]] ----------------------------------------------------------

-- 游戏中:背包1:行囊（道具物品）
--[[
BI_InventoryItem_C.WidgetTree.RootWidget
Root
[0] ResizeCon
 [0] HoverRoot
  [0] ImgItem
  [1] MarkerCon
   [0] ImgRedPoint
   [1] MarkerBase
   [2] TxtNum
   [3] ImgTimeMarker
   [4] MarkerSlot
  [2] FocusWidget
]]
WkGlobals.GetTextFuncMap["BI_InventoryItem_C"] = function(Button, InFocusEvent)
    local ClassName = Button:GetFName():ToString()

    -- 物品名称
    local hasItem = false
    local ResizeCon = Button.WidgetTree.RootWidget:GetChildAt(0)
    local ImgItem = ResizeCon:GetChildAt(0):GetChildAt(0)
    -- TArray< struct FTextureParameterValue >
    local TextureParameterValues = ImgItem.Brush.ResourceObject.TextureParameterValues
    local TextureName = TArray_GetTextureName1(TextureParameterValues)
    hasItem = (nil ~= TextureName)
    -- TODO: 获取物品名称

    -- 物品数量
    local MarkerCon = ResizeCon:GetChildAt(0):GetChildAt(1)
    local TxtNum = MarkerCon:GetChildAt(2) -- 物品数量。默认 99
    local TxtNum_txt = TxtNum:GetText():ToString()

    -- 物品类型：用品/材料/酒食/药材/要紧物事
    local ItemType = "用品"

    if not hasItem then
        return string.format("%s (空)", ItemType)
    else
        return string.format("%s %s %s", ItemType, TextureName, TxtNum_txt)
    end
end
