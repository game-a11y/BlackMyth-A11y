-- SPDX-License-Identifier: MIT
-- UI Hook 修行（技能）
-- Author: inkydragon
local SubModeName = "[BlackMythA11y.UIHooks] "
-- [[require Global]]

-- [[require Local]]
local WkGlobals = require("WkGlobals")
local WkUtils = require("WkUtils")
-- [[Global Var]]


-- [[ Hook 事件处理函数 ]] ----------------------------------------------------------

--[[ 父对象树
BI_AbilityIcon_KB_Basic_C /BUI_TalentMain_C_2147464952.WidgetTree.AbilityIcon1_KB
    Overlay /BUI_TalentMain_C_2147464952.WidgetTree.Overlay_12
    CanvasPanel /BUI_TalentMain_C_2147464952.WidgetTree.AbilityRoot_KB
    CanvasPanel /BUI_TalentMain_C_2147464952.WidgetTree.AbilityMainRoot
    CanvasPanel /BUI_TalentMain_C_2147464952.WidgetTree.LeftRoot    # <-----
    CanvasPanel /BUI_TalentMain_C_2147464952.WidgetTree.MainRoot
    CanvasPanel /BUI_TalentMain_C_2147464952.WidgetTree.DebugSafeArea
    CanvasPanel /BUI_TalentMain_C_2147464952.WidgetTree.RootCon
]]
local function GetLeftRoot(AbilityIcon_KB)
    return WkUtils.FindParentByName(AbilityIcon_KB, "LeftRoot")
end

-- 游戏中:背包-1:修行（技能）
--[[
TODO: 实时获取文本

BI_AbilityIcon_KB_Basic_C
BI_AbilityIcon_KB_Advance_C

BI_AbilityIcon_GP_Basic_C   /.BUI_TalentMain_C_2147461288.WidgetTree.AbilityIcon1_GP
BI_AbilityIcon_GP_Advance_C /.BUI_TalentMain_C_2147461288.WidgetTree.AbilityIcon2_GP
]]

-- 从 BI_AbilityIcon_KB_*_C 开始获取 LeftRoot 然后得到描述
--[[
LeftRoot
   [0] ContentAbilityRoot
    [0] TxtAbilityTitle         本事
    [1] TxtAbilityDesc          求仙问卜，不如自己做主。诵佛念经，不如本事在身。
    [2] TxtAbilityTypeTitle     棍法
    [3] TxtAbilityTypeDesc      学习招式，精进棍术，壮大棍势。
]]
local function GetAbilityIcon_KB_desc(Button, txt_desc)
    -- WkUtils.PrintAllParents(Button)
    local LeftRoot = GetLeftRoot(Button)
    if LeftRoot then
        -- print(string.format("\t%s\n", LeftRoot:GetFullName()))
        local TxtAbilityTitle = LeftRoot:GetChildAt(0):GetChildAt(0):GetText():ToString()
        local TxtAbilityDesc = LeftRoot:GetChildAt(0):GetChildAt(1):GetText():ToString()
        local TxtAbilityTypeTitle = LeftRoot:GetChildAt(0):GetChildAt(2):GetText():ToString()
        local TxtAbilityTypeDesc = LeftRoot:GetChildAt(0):GetChildAt(3):GetText():ToString()
        -- print(string.format("\tTxtAbilityTitle      = %s\n", TxtAbilityTitle))
        -- print(string.format("\tTxtAbilityDesc       = %s\n", TxtAbilityDesc))
        -- print(string.format("\tTxtAbilityTypeTitle  = %s\n", TxtAbilityTypeTitle))
        -- print(string.format("\tTxtAbilityTypeDesc   = %s\n", TxtAbilityTypeDesc))
    
        txt_desc = string.format("%s：%s %s", TxtAbilityTitle, TxtAbilityTypeTitle, TxtAbilityTypeDesc)
    end

    return txt_desc
end

-- 游戏中:背包-1:修行（技能）:本事：根基
WkGlobals.GetTextFuncMap["BI_AbilityIcon_KB_Basic_C"] = function(Button, InFocusEvent)
    return GetAbilityIcon_KB_desc(Button, "本事：根基")
end
-- 游戏中:背包-1:修行（技能）:本事：根基
WkGlobals.GetTextFuncMap["BI_AbilityIcon_KB_Advance_C"] = function(Button, InFocusEvent)
    return GetAbilityIcon_KB_desc(Button, "本事：棍法")
end

WkGlobals.GetTextFuncMap["BI_AbilityIcon_GP_Basic_C"] = function(Button, InFocusEvent)
    local TxtName_txt = "根基"
    return TxtName_txt
end
WkGlobals.GetTextFuncMap["BI_AbilityIcon_GP_Advance_C"] = function(Button, InFocusEvent)
    local TxtName_txt = "棍法"
    return TxtName_txt
end

-- 游戏中:背包-1:修行（技能）:根基:气力
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
    return TxtName_txt
end
