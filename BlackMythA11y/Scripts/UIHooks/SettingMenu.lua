-- SPDX-License-Identifier: MIT
-- UI Hook
-- Author: inkydragon
local SubModeName = "[BlackMythA11y.UIHooks] "
-- [[require Global]]

-- [[require Local]]
local WkGlobals = require("WkGlobals")
-- [[Global Var]]


-- [[ Hook 事件处理函数 ]] ----------------------------------------------------------

-- 主界面/设置: 一级菜单
--[[
BI_SettingTab_0 ~ 9
    0. 控制器
    1. 键盘与鼠标
    2. 游戏
    3. 视角
    4. 语言
    5. 显示
    6. 画质
    7. 声音
    8. 辅助设置
    9. 退出游戏

BI_SettingTab_C.WidgetTree.RootWidget
[0] BtnCon
 [0] ImgBg
 [1] ImgIcon
 [2] TxtName
 [3] FocusWidget
]]
WkGlobals.GetTextFuncMap["BI_SettingTab_C"] = function(Button, InFocusEvent)
    local BtnCon = Button.WidgetTree.RootWidget:GetChildAt(0)
    local TxtName = BtnCon:GetChildAt(2)  -- CPS1
    local TxtName_txt = TxtName:GetText():ToString()  -- GSScaleText
    return TxtName_txt
end -- BI_SettingTab_C

-- 主界面/设置: 左右单项选择
--[[
GSScaleText /.BI_SettingFixedItem_0.WidgetTree.BI_Btn.WidgetTree.TxtName
TextBlock   /.BI_SettingFixedItem_0.WidgetTree.TxtDesc

TODO:
- 补充选项的详细说明
-- 主界面/设置/视角: 文本区分 控制器和键盘

BI_SettingFixedItem_C.WidgetTree.RootWidget
[0] BtnCon
 [0] BI_Btn
   Default__BI_SettingMainBtn_C
    WidgetTree
     [0] BtnCon
      [0] ImgBg
      [1] ImgLock
      [2] HorizontalBox_0
       [0] TxtName
       [1] ModifiedCon
        [0] ImgModified
       [2] RestartCon
        [0] ImgRestart
       [3] WarningCon
        [0] TextWarning
      [3] FocusWidget

    BI_SettingFixedItem_C /.BUI_InitSetting_C_2147482286.WidgetTree.BI_SettingLocalization
]]
WkGlobals.GetTextFuncMap["BI_SettingFixedItem_C"] = function(Button, InFocusEvent)
    local FullName = Button:GetFullName()
    local ClassName = Button:GetFName():ToString()

    -- NOTE: 这里基于 Button.BI_Btn
    local BtnCon = Button.BI_Btn.WidgetTree.RootWidget:GetChildAt(0)
    local HBox0 = BtnCon:GetChildAt(2)
    local TxtName = HBox0:GetChildAt(0)
    local TxtName_txt = TxtName:GetText():ToString()  -- GSScaleText

    --[[
    BI_SettingFixedItem_C.WidgetTree.RootWidget
      [0] BtnCon
       [0] BI_Btn
       [1] FocusWidget
       [2] DescCon
        [0] ImgLeft
        [1] CanvasPanel_165
         [0] ScaleBox_58
          [0] TxtDesc
        [2] ImgRight
    ]]
    BtnCon = Button.WidgetTree.RootWidget:GetChildAt(0)
    local DescCon = BtnCon:GetChildAt(2)
    local CanvasPanel_165 = DescCon:GetChildAt(1)
    local ScaleBox_58 = CanvasPanel_165:GetChildAt(0)
    local TxtDesc = ScaleBox_58:GetChildAt(0)
    local TxtDesc_txt = TxtDesc:GetText():ToString()  -- TextBlock

    local BtnType = "左右单项选择"
    local A11yNote = ""
    if "控制器类型" == TxtName_txt then
        A11yNote = "无障碍提示：类型一是 XBox 手柄；类型二是 PS 手柄"
    elseif "输入类型" == TxtName_txt then
        A11yNote = "无障碍提示：输入类型用于切换键位布局，目前不支持自定义手柄键位，建议使用 Steam 自定义映射"
    end

    -- 首次启动
    if "文本语言" == TxtName_txt and string.find(FullName, "BUI_InitSetting_C") then
        A11yNote = "手柄按 A 确定，键盘按 E 确定"
    end

    return string.format("%s %s %s %s", TxtName_txt, BtnType, TxtDesc_txt, A11yNote)
end -- BI_SettingFixedItem_C

-- 主界面/设置: 下拉单项选择
--[[
    GSScaleText /.BI_SettingMenuItem_0.WidgetTree.BI_Btn.WidgetTree.BI_Btn.WidgetTree.TxtName
    TextBlock   /.BI_SettingMenuItem_0.WidgetTree.BI_Btn.WidgetTree.TxtDesc

TODO:   下拉项读取
BI_ModeBtnItem_C /.BUI_Setting_C_2147476617.WidgetTree.BI_SettingMenuPage.WidgetTree.BI_Item.WidgetTree.BI_ModeBtnItem_C_2147475188
TextBlock        /..BUI_Setting_C_2147476617.WidgetTree.BI_SettingMenuPage.WidgetTree.BI_Item.WidgetTree.BI_ModeBtnItem_C_2147475188.WidgetTree.TxtName

BI_SettingMenuItem_C.WidgetTree.RootWidget
CanvasPanel_0
[0] BtnCon
 [0] BI_Btn
   Default__BI_SettingMenuBtn_C
    WidgetTree
     RootCon
     [0] BtnCon
      [0] BI_Btn
        Default__BI_SettingMainBtn_C
         WidgetTree
          RootCon
          [0] BtnCon
           [0] ImgBg
           [1] ImgLock
           [2] HorizontalBox_0
            [0] TxtName
            [1] ModifiedCon
             [0] ImgModified
            [2] RestartCon
             [0] ImgRestart
            [3] WarningCon
             [0] TextWarning
           [3] FocusWidget
]]
WkGlobals.GetTextFuncMap["BI_SettingMenuItem_C"] = function(Button, InFocusEvent)
    -- print(string.format("Button=%s\n", Button:GetFullName()))
    -- PrintAllParents(Button)

    -- NOTE: 这里基于 Button.BI_Btn.BI_Btn
    local BtnCon = Button.BI_Btn.BI_Btn.WidgetTree.RootWidget:GetChildAt(0)
    local HBox0 = BtnCon:GetChildAt(2)
    local TxtSettingInfo = HBox0:GetChildAt(0)
    local TxtSettingInfo_txt = TxtSettingInfo:GetText():ToString()  -- GSScaleText

    --[[
      BI_SettingMenuItem_C.WidgetTree.RootWidget
      CanvasPanel_0
      [0] BtnCon
       [0] BI_Btn
         Default__BI_SettingMenuBtn_C
          WidgetTree
           RootCon
           [0] BtnCon
            [0] BI_Btn
            [1] TxtName
            [2] MenuCon
             [0] ScaleBox_0
              [0] TxtDesc
             [1] ImgArrow
    ]]
    BtnCon = Button.BI_Btn.WidgetTree.RootWidget:GetChildAt(0)
    local MenuCon = BtnCon:GetChildAt(2)
    local SBox0 = MenuCon:GetChildAt(0)
    local TxtDesc = SBox0:GetChildAt(0)
    local TxtDesc_txt = TxtDesc:GetText():ToString()  -- TextBlock
    
    local A11yNote = ""
    if "文本语言" == TxtSettingInfo_txt then -- 设置/语言/1
        A11yNote = "无障碍提示：除汉语英语以外的语言，目前下拉选项无障碍输出有误，选择语言后可正确读取已选择的语言"
    end

    return string.format("%s %s %s %s", TxtSettingInfo_txt, "下拉单项选择", TxtDesc_txt, A11yNote)
end -- BI_SettingMenuItem_C

-- 主界面/设置: 下拉项
--[[
BI_ModeBtnItem_C.WidgetTree.RootWidget
[0] BtnCon
 [0] ImgBg
 [1] TxtName
 [2] ImgIcon
 [3] FocusWidget
]]
WkGlobals.GetTextFuncMap["BI_ModeBtnItem_C"] = function(Button, InFocusEvent)
    local BtnCon = Button.WidgetTree.RootWidget:GetChildAt(0)
    local TxtName = BtnCon:GetChildAt(1) -- cps1
    local TxtName_txt = TxtName:GetText():ToString()  -- TextBlock
    -- TODO "无障碍提示：除汉语英语以外的语言，目前无障碍输出有误。"
    return string.format("%s %s", TxtName_txt, "下拉项")
end -- BI_ModeBtnItem_C

-- 主界面/设置: 水平滑块
--[[
BI_SettingSliderItem_C.WidgetTree.RootWidget
[0] CanvasPanel_71
 [0] BI_Btn
   Default__BI_SettingMainBtn_C
    WidgetTree
     RootCon
     [0] BtnCon
      [0] ImgBg
      [1] ImgLock
      [2] HorizontalBox_0
       [0] TxtName
       [1] ModifiedCon
        [0] ImgModified
       [2] RestartCon
        [0] ImgRestart
       [3] WarningCon
        [0] TextWarning
      [3] FocusWidget
 [1] BI_Slider
   Default__BI_SettingSlider_C
]]
WkGlobals.GetTextFuncMap["BI_SettingSliderItem_C"] = function(Button, InFocusEvent)
    local ClassName = Button:GetFName():ToString()

    -- BI_Btn.WidgetTree.TxtName
    local BI_Btn = Button.BI_Btn
    local RootCon = BI_Btn.WidgetTree.RootWidget
    local BtnCon = RootCon:GetChildAt(0)
    local HorizontalBox_1 = BtnCon:GetChildAt(2)
    local TxtName = HorizontalBox_1:GetChildAt(0)
    local TxtName_txt = TxtName.Text:ToString()  -- TextBlock

    -- BI_Slider.WidgetTree.TxtNum
    local BI_Slider = Button.BI_Slider
    local SliderBtn = BI_Slider.SliderBtn
    local TxtNum = SliderBtn:GetChildAt(3)
    local Volume = TxtNum:GetText():ToString()  -- GSScaleText

    -- BI_Slider.WidgetTree.TxtMaxNum
    local RootCon2 = BI_Slider.WidgetTree.RootWidget
    local BtnCon2 = RootCon2:GetChildAt(0)
    local HorizontalBox_0 = BtnCon2:GetChildAt(0)
    local SizeBox_1 = HorizontalBox_0:GetChildAt(2)
    local TxtMaxNum = SizeBox_1:GetChildAt(0)
    local TxtMaxNum_txt = TxtMaxNum:GetText():ToString()  -- TextBlock

    -- BI_Slider.WidgetTree.TxtMinNum = 0
    return string.format("%s %s %s/%s", TxtName_txt, "水平滑块", Volume, TxtMaxNum_txt)
end -- BI_SettingSliderItem_C

-- 主界面/设置: 图标按钮
--[[
BI_SettingIconItem_C /.BUI_Setting_C_2147457164.WidgetTree.BI_SettingIconItem_0
GSScaleText          /   .BI_SettingIconItem_0.WidgetTree.BI_Btn.WidgetTree.TxtName

校准滑块
BI_SettingSliderL_C /.BUI_BrightnessSetting_C_2147454165.WidgetTree.BI_Slider

BI_SettingIconItem_C.WidgetTree.RootWidget
[0] BtnCon
 [0] BI_Btn
   Default__BI_SettingMainBtn_C
    WidgetTree
     RootCon
     [0] BtnCon
      [0] ImgBg
      [1] ImgLock
      [2] HorizontalBox_0
       [0] TxtName
       [1] ModifiedCon
        [0] ImgModified
       [2] RestartCon
        [0] ImgRestart
       [3] WarningCon
        [0] TextWarning
      [3] FocusWidget
]]
WkGlobals.GetTextFuncMap["BI_SettingIconItem_C"] = function(Button, InFocusEvent)
    local ClassName = Button:GetFName():ToString()

    local BtnCon = Button.BI_Btn.WidgetTree.RootWidget:GetChildAt(0)
    local HorizontalBox_1 = BtnCon:GetChildAt(2)
    local TxtName = HorizontalBox_1:GetChildAt(0)
    local TxtName_txt = TxtName.Text:ToString()  -- TextBlock

    local desc_txt = ""
    if "BI_KeyboradEnter" == ClassName then
        desc_txt = "调整键盘按键映射"
    elseif "BI_SettingIconItem_0" == ClassName then
        desc_txt = "调整画面亮度"
    end
    return string.format("%s %s %s", TxtName_txt, "图标按钮", desc_txt)
end -- BI_SettingIconItem_C

-- 主界面/设置: 文本按钮
--[[
BI_SettingMainBtn_C /.BUI_Setting_C_2147457164.WidgetTree.BI_SettingBtnItem_0
GSScaleText         /.BUI_Setting_C_2147457164.WidgetTree.BI_SettingBtnItem_0.WidgetTree.TxtName

BI_SettingMainBtn_C.WidgetTree.RootWidget
[0] BtnCon
 [0] ImgBg
 [1] ImgLock
 [2] HorizontalBox_0
  [0] TxtName
  [1] ModifiedCon
   [0] ImgModified
  [2] RestartCon
   [0] ImgRestart
  [3] WarningCon
   [0] TextWarning
 [3] FocusWidget
]]
WkGlobals.GetTextFuncMap["BI_SettingMainBtn_C"] = function(Button, InFocusEvent)
    local BtnCon = Button.WidgetTree.RootWidget:GetChildAt(0)
    local HBox0 = BtnCon:GetChildAt(2)
    local TxtName = HBox0:GetChildAt(0)
    local TxtName_txt = TxtName:GetText():ToString()
    local desc_txt = ""
    return string.format("%s %s %s", TxtName_txt, "文本按钮", desc_txt)
end -- BI_SettingMainBtn_C

-- 主界面/设置: 图标按钮-键盘键位
--[[
BI_SettingKeyItem_C.WidgetTree.RootWidget
[0] BtnCon
 [0] BI_Btn
   Default__BI_SettingMainBtn_C
    WidgetTree
     RootCon
     [0] BtnCon
      [0] ImgBg
      [1] ImgLock
      [2] HorizontalBox_0
       [0] TxtName:     "向前移动"  按键功能名称
       [1] ModifiedCon
        [0] ImgModified
       [2] RestartCon
        [0] ImgRestart
       [3] WarningCon
        [0] TextWarning
      [3] FocusWidget
 [1] KeyCon
  [0] ImgKeyConflictBg
  [1] ReplaceCon
   [0] ImgInputBg
   [1] TxtShuru:        "输入按键"  自定义按键时，占位提示文本
  [2] ImgKeyIcon
  [3] TxtKeyName:       "W" 按键名称，但目前固定为 W
]]
WkGlobals.GetTextFuncMap["BI_SettingKeyItem_C"] = function(Button, InFocusEvent)
    local BI_Btn_BtnCon = Button.BI_Btn.WidgetTree.RootWidget:GetChildAt(0)
    local HorizontalBox_0 = BI_Btn_BtnCon:GetChildAt(2)
    local TxtName = HorizontalBox_0:GetChildAt(0)
    local TxtName_txt = TxtName:GetText():ToString()  -- GSScaleText

    local BtnCon = Button.WidgetTree.RootWidget:GetChildAt(0)
    local KeyCon = BtnCon:GetChildAt(1)
    local ReplaceCon = KeyCon:GetChildAt(1)
    -- local TxtShuru = ReplaceCon:GetChildAt(1)
    -- local TxtShuru_txt = TxtShuru:GetText():ToString()
    -- local TxtKeyName = KeyCon:GetChildAt(3)
    -- local TxtKeyName_txt = TxtKeyName:GetText():ToString()
    -- local ImgKeyIcon = KeyCon:GetChildAt(2)
    -- local ImgKeyIcon_txt = ImgKeyIcon:GetFullName()

    -- TODO: 读取按键名称
    local TxtKeyName_txt = "无障碍说明：目前无法获取当前按键名称，读取的值固定为 W"
    return string.format("%s %s %s", TxtName_txt, "按键配置按钮", TxtKeyName_txt)
end -- BI_SettingKeyItem_C

-- 主界面/设置: 亮度调整
--[[
BUI_BrightnessSetting_C.WidgetTree.RootWidget
RootCon
[0] BI_BlackBG
  Default__BI_BlackBG_C
   WidgetTree
    Root
    [0] BlurBg
    [1] BlackBg
[1] Image_87
[2] CanvasPanel_96
 [0] ImgPreview
[3] CanvasPanel_0
 [0] BI_Slider
   Default__BI_SettingSliderL_C
 [1] TxtBriInc
 [2] TxtBriDec
 [3] TextBlock_168
[4] CanvasPanel
 [0] BI_InputRoot
   Default__BI_InputRoot_C

BI_SettingSliderL_C.WidgetTree.RootWidget
RootCon
[0] BtnCon
 [0] HorizontalBox_0
  [0] SizeBox_0
   [0] TxtMinNum
  [1] SliderBar
   [0] ImgSliderBar
   [1] ImgSliderProg
   [2] SliderBarClickArea
   [3] SliderBtn
    [0] SliderImg
    [1] ImgArrowLeft
    [2] ImgArrowRight
    [3] TxtNum
    [4] SliderBtnClickArea
  [2] SizeBox_1
   [0] TxtMaxNum
]]
-- TODO: 此处没有控制器焦点控件
-- 首次启动 BI_SettingSliderL_C /.BUI_InitSetting_C_2147482286.WidgetTree.BI_Slider
