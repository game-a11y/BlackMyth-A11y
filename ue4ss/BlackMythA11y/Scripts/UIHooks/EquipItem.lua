-- SPDX-License-Identifier: MIT
-- UI Hook 披挂（装备）
-- Author: inkydragon
local SubModeName = "[BlackMythA11y.UIHooks] "
-- [[require Global]]

-- [[require Local]]
local WkGlobals = require("WkGlobals")
-- [[Global Var]]


-- [[ Hook 事件处理函数 ]] ----------------------------------------------------------

-- 获取 TArray< struct FTextureParameterValue > 中的第一个材质名称
-- 用于判断背包物品是否为空
function TArray_GetTextureName1(TextureParameterValues)
    local UObjGood = TextureParameterValues and TextureParameterValues:IsValid()
    local TextureName = nil
    if not UObjGood then
        -- 无效的对象
        return TextureName
    end

    if #TextureParameterValues > 0 then
        -- 有贴图材料
        local Texture1 = TextureParameterValues[1]
        -- print(string.format("  %s\n", Texture1:GetFullName()))
        -- ParameterValue UTexture
        local ParameterValue = Texture1.ParameterValue
        TextureName = ParameterValue:GetFName():ToString()

        -- 默认材质 Texture2D /Game/00MainHZ/UI/Atlas/Icon/Item_Icon_Default_t.Item_Icon_Default_t
        if string.find(TextureName, "Item_Icon_Default_t") then
            -- 默认材质，忽略
            TextureName = nil
        end
    end

    return TextureName
end -- GetTextureName1


-- 游戏中:背包0:披挂（装备）
--[[
BI_EquipItem_Slot_C.WidgetTree.RootWidget
Root
[0] ResizeCon
 [0] HoverRoot
  [0] ImgItem
  [1] MarkerCon
   [0] ImgRedPoint
   [1] MarkerSlot
    [0] MarkerBase
  [2] FocusWidget
]]
WkGlobals.GetTextFuncMap["BI_EquipItem_Slot_C"] = function(Button, InFocusEvent)
    local ResizeCon = Button.WidgetTree.RootWidget:GetChildAt(0)
    local ImgItem = ResizeCon:GetChildAt(0):GetChildAt(0)
    local Brush = ImgItem.Brush
    -- MaterialInstanceDynamic
    local ResourceObject = Brush.ResourceObject
    print(string.format("  %s\n", ResourceObject:GetFullName()))
    -- TArray< struct FTextureParameterValue >
    local TextureParameterValues = ResourceObject.TextureParameterValues

    local TextureName = TArray_GetTextureName1(TextureParameterValues)
    if nil == TextureName then
        TextureName = "(空)"
    end
    -- TODO: 获取物品名称

    return string.format("背包物品 %s", TextureName)
end

-- 游戏中:背包:珍玩（葫芦等）
--[[
GSRichScaleText /.BUI_EquipMain_C_2147461353.WidgetTree.TxtHuluTitleRuby
BI_GearItem_Slot_C.WidgetTree.RootWidget
Root
[0] ResizeCon
 [0] HoverRoot
  [0] ImgItem
  [1] MarkerCon
   [0] ImgRedPoint
   [1] MarkerSlot
    [0] MarkerBase
  [2] FocusWidget
]]
WkGlobals.GetTextFuncMap["BI_GearItem_Slot_C"] = function(Button, InFocusEvent)
    local SuperClassName = Button:GetClass():GetFName():ToString()
    local ClassName = Button:GetFName():ToString()
    
    local ResizeCon = Button.WidgetTree.RootWidget:GetChildAt(0)
    local ImgItem = ResizeCon:GetChildAt(0):GetChildAt(0)
    local Brush = ImgItem.Brush
    -- MaterialInstanceDynamic
    local ResourceObject = Brush.ResourceObject
    print(string.format("  %s\n", ResourceObject:GetFullName()))
    -- TArray< struct FTextureParameterValue >
    local TextureParameterValues = ResourceObject.TextureParameterValues

    local TextureName = TArray_GetTextureName1(TextureParameterValues)
    if nil == TextureName then
        TextureName = "(空)"
    end
    -- TODO: 获取物品名称

    local TxtName_txt = "珍玩物品"
    if "BI_EquipSlotItem_9" == ClassName then
        TxtName_txt = "珍玩·一"
    elseif "BI_EquipSlotItem_10" == ClassName then
        TxtName_txt = "珍玩·二"
    elseif "BI_EquipSlotItem_8" == ClassName then
        TxtName_txt = "老葫芦"
    end

    return string.format("%s %s", TxtName_txt, TextureName)
end

-- 游戏中:背包:随身之物（快速道具）
--[[
GSRichScaleText /.BUI_EquipMain_C_2147461353.WidgetTree.TxtQuickItemTitleRuby
BI_QuickItem_C.WidgetTree.RootWidget
Root
[0] ResizeCon
 [0] HoverRoot
  [0] ImgItem
  [1] UINS_Config
  [2] MarkerCon
   [0] TxtNum
   [1] MarkerSlot
    [0] MarkerBase
  [3] FocusWidget
  [4] ImgHitArea
]]
WkGlobals.GetTextFuncMap["BI_QuickItem_C"] = function(Button, InFocusEvent)
    local ResizeCon = Button.WidgetTree.RootWidget:GetChildAt(0)
    local ImgItem = ResizeCon:GetChildAt(0):GetChildAt(0)
    local Brush = ImgItem.Brush
    -- MaterialInstanceDynamic
    local ResourceObject = Brush.ResourceObject
    -- TArray< struct FTextureParameterValue >
    local TextureParameterValues = ResourceObject.TextureParameterValues
    local TextureName = TArray_GetTextureName1(TextureParameterValues)
    if nil == TextureName then
        return "随身之物 (空)"
    end
    -- TODO: 获取物品名称

    local MarkerCon = ResizeCon:GetChildAt(0):GetChildAt(2)
    local TxtNum = MarkerCon:GetChildAt(0) -- 物品数量。默认 99
    local TxtNum_txt = TxtNum:GetText():ToString()

    return string.format("随身之物 %s %s", TextureName, TxtNum_txt)
end
