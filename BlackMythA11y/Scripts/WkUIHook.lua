-- SPDX-License-Identifier: MIT
-- WuKong UI Hooks
-- Author: inkydragon
local SubModeName = "[BlackMythA11y.WkUIHook] "
-- [[require Global]]

-- [[require Local]]
local WkGlobals = require("WkGlobals")
local WkUtils = require("WkUtils")
local WkConfig = require("WkConfig")
-- [[Global Var]]
local WkUIHook = {}
-- 开发中标记
local DevNote = "开发中! "


-- [[ 辅助函数 ]] -----------------------------------------------------------------
-- WkUtils.PrintAllParents


-- [[ Hook 事件处理函数 ]] ----------------------------------------------------------

-- 首次加载 主界面
-- 主界面/主菜单: 所有按钮
-- 主界面/主菜单/继续游戏 （无需特殊处理）
-- 主界面/主菜单/新游戏 （无需特殊处理）
-- 主界面/主菜单/载入游戏
-- 主界面/主菜单/小曲 (音乐)
require("UIHooks.HomeScreen")

-- 主界面/设置: 一级菜单
-- 主界面/设置: 左右单项选择
-- 主界面/设置: 下拉单项选择
-- 主界面/设置: 下拉项
-- 主界面/设置: 水平滑块
-- 主界面/设置: 图标按钮
-- 主界面/设置: 文本按钮
-- 主界面/设置: 图标按钮-键盘键位
require("UIHooks.SettingMenu")


-- 二次确定对话框
--[[
BUI_Reconfirm_C_2147454606.WidgetTree.txt
BUI_Reconfirm_C_2147454606.WidgetTree.Btn_Confirm
BUI_Reconfirm_C_2147454606.WidgetTree.Btn_Cancel

BI_ReconfirmBtn_C.WidgetTree.RootWidget
[0] HoverRoot
 [0] BtnCon
  [0] ImgBg
  [1] TxtName
  [2] FocusWidget
]]
-- TODO: 构造时 BI_ReconfirmBtn_C 读出 .txt
-- GSRichScaleText /.BUI_Reconfirm_C_2147454627.WidgetTree.txt
-- /BUI_Reconfirm_C.WidgetTree.Btn_Cancel
WkGlobals.GetTextFuncMap["BI_ReconfirmBtn_C"] = function(Button, InFocusEvent)
    local ClassName = Button:GetFName():ToString()

    local HoverRoot = Button.WidgetTree.RootWidget:GetChildAt(0)
    local BtnCon = HoverRoot:GetChildAt(0)
    local TxtName = BtnCon:GetChildAt(1)  -- .PSSlot2
    local TxtName_txt = TxtName:GetText():ToString()  -- TextBlock

    if "Btn_Confirm" == ClassName then
        local HBoxBtn = Button:GetParent()
        local BtnCon = HBoxBtn:GetParent()
        local BoxCon = BtnCon:GetParent()
        --
        local ContentCon = BoxCon:GetChildAt(1)
        local txt = ContentCon:GetChildAt(0)
        local ContentCon_txt = txt:GetText():ToString()
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

-- TODO: 游戏中:背包栏
--[[
背包顶层选项卡
BI_CommTxtTab_C     /.BUI_RoleMain_C_2147463565.WidgetTree.BI_RoleTab.WidgetTree.BI_SecTab.WidgetTree.BI_CommTxtTab_C_2147463554
    GSScaleText /.BI_RoleTab.WidgetTree.BI_SecTab.WidgetTree.BI_CommTxtTab_C_2147471225.WidgetTree.TxtName
BI_EquipItem_Slot_C /.BUI_EquipMain_C_2147465651.WidgetTree.BI_EquipSlotItem_1
    GSScaleText /.BI_RoleTab.WidgetTree.BI_SecTab.WidgetTree.BI_CommTxtTab_C_2147471217.WidgetTree.TxtName
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
WkGlobals.GetTextFuncMap["BI_AbilityIcon_KB_Basic_C"] = function(Button, InFocusEvent)
    local TxtName_txt = "根基"
    return TxtName_txt
end
WkGlobals.GetTextFuncMap["BI_AbilityIcon_KB_Advance_C"] = function(Button, InFocusEvent)
    local TxtName_txt = "棍法"
    return TxtName_txt
end
WkGlobals.GetTextFuncMap["BI_AbilityIcon_GP_Basic_C"] = function(Button, InFocusEvent)
    local TxtName_txt = "根基"
    return TxtName_txt
end
WkGlobals.GetTextFuncMap["BI_AbilityIcon_GP_Advance_C"] = function(Button, InFocusEvent)
    local TxtName_txt = "棍法"
    return TxtName_txt
end

-- 游戏中:背包:技能:根基
--[[
Tab 按钮
BI_SpellPanelTitle_Btn_C.WidgetTree.RootWidget
[0] BgRoot
 [0] ImgBg
[1] BtnCon
 [0] ResizeSelectRipple
  [0] UIFX_SelectRipple
 [1] ResizeBtn
  [0] ImgBtn
 [2] SizeBoxName
  [0] TxtName
 [3] ImgBar
 [4] ImgDot
 [5] FocusWidget

右侧侧边栏
"气力" 标题 GSScaleText /.BUI_LearnTalent_C_2147454110.WidgetTree.BI_SpellDetail.WidgetTree.TxtName

"主动" 消耗1 TextBlock /.BUI_LearnTalent_C_2147454110.WidgetTree.BI_SpellDetail.WidgetTree.TxtSpellType
TextBlock /.BUI_LearnTalent_C_2147454110.WidgetTree.BI_SpellDetail.WidgetTree.TxtEnergy
TextBlock /.BUI_LearnTalent_C_2147454110.WidgetTree.BI_SpellDetail.WidgetTree.TxtCost
TextBlock /.BUI_LearnTalent_C_2147454110.WidgetTree.BI_SpellDetail.WidgetTree.TxtCd
技能描述 GSInputRichTextBlock /.BUI_LearnTalent_C_2147454110.WidgetTree.BI_SpellDetail.WidgetTree.TxtSpellDesc

"已开悟" 学习技能按钮 GSScaleText /.BUI_LearnTalent_C_2147454110.WidgetTree.BI_SpellDetail.WidgetTree.BI_LongPress.WidgetTree.TxtName
]]
WkGlobals.GetTextFuncMap["BI_SpellPanelTitle_Btn_C"] = function(Button, InFocusEvent)
    local BtnCon = Button.WidgetTree.RootWidget:GetChildAt(1)
    local SizeBoxName = BtnCon:GetChildAt(2)
    local TxtName = SizeBoxName:GetChildAt(0)
    local TxtName_txt = TxtName:GetText():ToString()

    -- BUI_LearnTalent_C
    local SpellDetail_txt = ""
    local BUI_LearnTalent_C = WkGlobals.UIGlobals["BUI_LearnTalent_C"]
    if BUI_LearnTalent_C then
        -- local ResizeBg = BUI_LearnTalent_C.WidgetTree.RootWidget:GetChildAt(0)
        print(string.format("%s\n", BUI_LearnTalent_C:GetFullName()))
        -- [TitleConTxtName]
        local ResizeBg = BUI_LearnTalent_C.BI_SpellDetail.WidgetTree.RootWidget:GetChildAt(0)
        local TitleConTxtName = ResizeBg:GetChildAt(1):GetChildAt(1)
        -- TODO: 输出不稳定???
        print(string.format("%s %s\n", TitleConTxtName:GetText():ToString(), TitleConTxtName:GetFullName()))
        -- [TxtSpellType]
        -- ResizeBg                  .SizeMain.VerticalBox_195.SizeBase
        local SizeBase = ResizeBg:GetChildAt(2):GetChildAt(0):GetChildAt(1)
        -- SpellInfoCon.SizeSpellType.ResizeSpellType.TxtSpellType
        local TxtSpellType = SizeBase:GetChildAt(0):GetChildAt(0):GetChildAt(0):GetChildAt(0)
        print(string.format("%s %s\n", TxtSpellType:GetText():ToString(), TxtSpellType:GetFullName()))
        
        -- [TxtSpellType]
        -- -- ResizeBg                  .SizeMain.VerticalBox_195.SizeBase
        -- local SizeBase = ResizeBg:GetChildAt(2):GetChildAt(0):GetChildAt(1)
        -- -- SpellInfoCon.SizeSpellType.ResizeSpellType.TxtSpellType
        -- local TxtSpellType = SizeBase:GetChildAt(0):GetChildAt(0):GetChildAt(0):GetChildAt(0)
        -- print(string.format("%s %s\n", TxtSpellType:GetText():ToString(), TxtSpellType:GetFullName()))
    end -- BUI_LearnTalent_C

    local TabName = ""
    return TabName..TxtName_txt..SpellDetail_txt
end

-- BI_TalentItem_1_1_C
--[[
BI_TalentItem_1_1_C.WidgetTree.RootWidget
[0] RootCon
 [0] HoverRoot
  [0] UIFX_SelectRipple
  [1] ImgItem
  [2] TxtLevelLimit
  [3] UIFX_CanLearn
  [4] UIFX_CanRipple
  [5] UINS_CanLearn
  [6] LevelRoot
   [0] BI_TalentLevelsNew
     Default__BI_TalentLevelsNew_C
  [7] UIFXLearnedSlot
  [8] ImgHitArea
  [9] FocusWidget
]]
WkGlobals.GetTextFuncMap["BI_TalentItem_1_1_C"] = function(Button, InFocusEvent)
    TxtName_txt = "根基技能"
    return DevNote..TxtName_txt
end

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

-- 游戏中:背包:物品
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

-- 游戏中:背包1:行囊(物品)
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


-- 游戏中:背包:游记
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


-- [[ Hook 函数 ]] --------------------------------------------------------------
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
    if WkGlobals.GetTextFuncMap[SuperClassName] then
        -- 默认打断输出
        InterruptSpeak = true
        local curText = WkGlobals.GetTextFuncMap[SuperClassName](Button, InFocusEvent)
        print(string.format("\t\"%s\"\n", curText))
        A11yTolk:Speak(curText, InterruptSpeak)
    else
        print(string.format("Cannot gettext for:  %s <: %s\n", ClassName, SuperClassName))
    end
end

-- 初始化所有 UI 钩子
function WkUIHook.InitUiHooks()
    local Hook_BUI_Widget_Class = {}
    -- 性能测试结果
    Hook_BUI_Widget_Class["BUI_BenchMark_V2_C"] = 1
    Hook_BUI_Widget_Class["BUI_Setting_C"] = 1
    -- -- Hook_BUI_Widget_Class["BUI_LoadingV2_C"] = 1  -- 没有挂钩到加载页面
    -- Hook_BUI_Widget_Class["BUI_LearnTalent_C"] = 1

    NotifyOnNewObject("/Script/b1-Managed.BUI_Widget", function(ConstructedObject)
        local FullName = ConstructedObject:GetFullName()
        local SuperClassName = ConstructedObject:GetClass():GetFName():ToString()
        local ClassName = ConstructedObject:GetFName():ToString()

        if not Hook_BUI_Widget_Class[SuperClassName] then
            return
        end
        
        -- 保存引用
        WkGlobals.UIGlobals[SuperClassName] = ConstructedObject
        print(string.format("BUI_Widget Constructed: %s\n", FullName))
    end)
    RegisterHook("/Script/b1-Managed.BUI_Widget:Destruct", function(Context)
        local Button = Context:get()
        local FullName = Button:GetFullName()
        local SuperClassName = Button:GetClass():GetFName():ToString()
        local ClassName = Button:GetFName():ToString()

        if not Hook_BUI_Widget_Class[SuperClassName] then
            return
        end

        -- 清除引用
        WkGlobals.UIGlobals[SuperClassName] = nil
        print(string.format("BUI_Widget Destruct: %s\n", FullName))
    end)

    RegisterHook("/Script/b1-Managed.BUI_Button:OnAddedToFocusPath", OnAddedToFocusPath_Hook)
    -- RegisterHook("/Script/b1-Managed.BUI_Widget:OnAddedToFocusPath", OnAddedToFocusPath_Hook)

    RegisterHook("/Script/b1-Managed.BUI_Button:OnMouseButtonDown", function(Context, MyGeometry, MouseEvent)
        local Button = Context:get()
        local FullName = Button:GetFullName()
        local SuperClassName = Button:GetClass():GetFName():ToString()
        local ClassName = Button:GetFName():ToString()
        print(string.format("OnMouseButtonDown: %s <: %s\n\t%s\n",
            ClassName, SuperClassName, FullName))
    end)

    -- hook 模拟输入
    ---@param Context UBUI_Button
    ---@param MyGeometry FGeometry
    ---@param InAnalogInputEvent FAnalogInputEvent
    -- RegisterHook("/Script/b1-Managed.BUI_Button:OnAnalogValueChanged", function(Context, MyGeometry, InAnalogInputEvent)
    --     local Button = Context:get()
    --     local AnalogInputEvent = InAnalogInputEvent:get()
    --     local FullName = Button:GetFullName()
    --     local SuperClassName = Button:GetClass():GetFName():ToString()
    --     local ClassName = Button:GetFName():ToString()
        
    --     if not string.find(ClassName, "Slider") then
    --         return
    --     end

    --     print(string.format("OnAnalogValueChanged(%s): %s <: %s\n\t%s\n",
    --         AnalogInputEvent:GetFName():ToString(),
    --         ClassName, SuperClassName, FullName))
    -- end)

    -- hook 按键抬起输入
    -- TODO: 目前不能按 InKeyEvent 筛选事件
    -- RegisterHook("/Script/b1-Managed.BUI_Button:OnKeyUp", function(Context, MyGeometry, InKeyEvent)
    --     local Button = Context:get()
    --     local AnalogInputEvent = InKeyEvent:get()
    --     local FullName = Button:GetFullName()
    --     local SuperClassName = Button:GetClass():GetFName():ToString()
    --     local ClassName = Button:GetFName():ToString()
        
    --     local log_class = {}
    --     log_class["BI_SettingFixedItem_C"] = 1
    --     log_class["BI_SettingSliderItem_C"] = 1
    --     if not log_class[SuperClassName] then
    --         return
    --     end

    --     print(string.format("OnKeyUp(%s): %s <: %s\n\t%s\n",
    --         AnalogInputEvent:GetFName():ToString(),
    --         ClassName, SuperClassName, FullName))
    --     print(string.format("InKeyEvent(%s): %s\n",
    --     InKeyEvent, AnalogInputEvent:GetFullName()))
    -- end)
    
    -- 返回的是上一步的对象
    -- RegisterHook("/Script/b1-Managed.BUI_Button:OnCustomWidgetNavigation", function(Context, Navigation)

    -- 会显示父级的聚焦事件
    -- RegisterHook("/Script/b1-Managed.BUI_Widget:OnFocusChanging", function(self, InFocusEvent)

    -- 闪退 hook "/Script/b1-Managed.BUI_Widget:OnAnimationSequenceEvent"
end

-- bmb 性能测试工具
if WkConfig.IsBencmarkTools then
    local WkbHook = require("WkbHook")
    RegisterKeyBind(Key.F12, function()
        WkbHook.BenchMarkReportBind(WkGlobals.UIGlobals)
    end)
end -- WkConfig.IsBencmarkTools

return WkUIHook
