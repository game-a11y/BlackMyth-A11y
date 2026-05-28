-- SPDX-License-Identifier: MIT
-- UI Hook
-- Author: inkydragon
local SubModeName = "[BlackMythA11y.UIHooks] "
-- [[require Global]]

-- [[require Local]]
local WkGlobals = require("WkGlobals")
-- [[Global Var]]


-- [[ Hook 事件处理函数 ]] ----------------------------------------------------------

-- 首次加载 主界面
--[[
BI_FirstStartBtn_C.WidgetTree.RootWidget
RootCon
[0] BtnCon
 [0] ImgBgW
 [1] ImgBg
 [2] ImgBgFX
 [3] TxtName
 [4] FocusWidget
]]
WkGlobals.GetTextFuncMap["BI_FirstStartBtn_C"] = function(Button, InFocusEvent)
    local BtnCon = Button.WidgetTree.RootWidget:GetChildAt(0)
    local TxtName = BtnCon:GetChildAt(3)  -- .BtnCon.CanvasPanelSlot_5
    return TxtName:GetText():ToString()
end

-- 主界面/主菜单: 所有按钮
--[[
<BI_StartGame_C>
    BI_StartGameBtn_0   继续游戏
    BI_StartGameBtn_1   新游戏
    BI_StartGameBtn_2   载入游戏
    BI_StartGameBtn_3   小曲
    BI_StartGameBtn_4   设置
    BI_StartGameBtn_5   退出

TODO:  首屏播报版本号
版本号  TextBlock   /.BUI_StartGame_C_2147456059.WidgetTree.TxtVersion
]]
WkGlobals.GetTextFuncMap["BI_StartGame_C"] = function(Button, InFocusEvent)
    local ClassName = Button:GetFName():ToString()

    -- 按钮 [继续游戏] 选中时，输出当前关卡信息: .TxtMainName; .TxtSubName
    local CurLevelName_txt = ""
    if "BI_StartGameBtn_0" == ClassName then
        local MainListCon = Button:GetParent()
        local StartBtnCon = MainListCon:GetParent()
        local Overlay_0 = StartBtnCon:GetParent()
        local MainCon = Overlay_0:GetParent()  -- .WidgetTree.MainCon
        -- 
        local RegionName = MainCon:GetChildAt(0)
        local TxtMainName = RegionName:GetChildAt(3)
        local TxtMainName_txt = TxtMainName.Text:ToString()
        local HorizontalBox_1 = RegionName:GetChildAt(1)
        local TxtSubName = HorizontalBox_1:GetChildAt(1)
        local TxtSubName_txt = TxtSubName.Text:ToString()
        CurLevelName_txt = string.format(" %s: %s", TxtMainName_txt, TxtSubName_txt)
    end

    -- 当前按钮文本
    local ContinueBtnTxt = Button.BI_TextLoop.Content:GetText():ToString()
    return ContinueBtnTxt .. CurLevelName_txt
end -- BI_StartGame_C

-- 主界面/主菜单/继续游戏 （无需特殊处理）
-- 主界面/主菜单/新游戏 （无需特殊处理）

-- 主界面/主菜单/载入游戏
--[[
InfoCon
    .TxtName:   游戏存档点名
    .TimeCon
        .TxtPlayTimeTitle: 游玩时间标题
        .TxtPlayTime: 游玩时间
        .TxtDate:   日期
        .TxtTime:   时间
    .TxtLvTitle: 等级标题
    .TxtLv:     等级

TODO:
- 读出当前存档数："当前存档 2/10"

BI_ArchivesBtnV2_C.WidgetTree.RootWidget
CanvasPanel_0
[0] CanvasPanel_28
 [0] ImgBgShadow
 [1] ImgBgBloom
 [2] ImgArchivesBg
 [3] ImgSelected
 [4] InfoCon
  [0] ImgLine
  [1] HorizontalBox_0
   [0] TxtName
   [1] TxtRestartGame
  [2] TimeCon
   [0] TxtPlayTimeTitle
   [1] TxtPlayTime
   [2] TxtDate
   [3] TxtTime
  [3] LevelCon
   [0] TxtLvTitle
   [1] TxtLv
  [4] NGPCon
   [0] HorizontalBox_58
    [0] MultiCon
     [0] ScaleBox_0
      [0] TxtNewGameMulti
    [1] TxtNewGame
    [2] TxtNewGameNum
    [3] TxtNewGame2
 [5] NGMarkerCon
  [0] ImgNGMarker
 [6] FocusWidget
]]
WkGlobals.GetTextFuncMap["BI_ArchivesBtnV2_C"] = function(Button, InFocusEvent)
    local CanvasPanel_0 = Button.WidgetTree.RootWidget
    local CanvasPanel_28 = CanvasPanel_0:GetChildAt(0)  -- CPS0
    local InfoCon = CanvasPanel_28:GetChildAt(4)  -- CPS1
    -- InfoCon[1]
    local HorizontalBox_0 = InfoCon:GetChildAt(1) -- SPS3
    local TxtName = HorizontalBox_0:GetChildAt(0) -- HBS0
    -- InfoCon[2]: TimeCon[0-3]
    local TimeCon = InfoCon:GetChildAt(2)
    local TxtPlayTimeTitle = TimeCon:GetChildAt(0)
    local TxtPlayTime = TimeCon:GetChildAt(1)
    local TxtDate = TimeCon:GetChildAt(2)
    local TxtTime = TimeCon:GetChildAt(3)
    -- InfoCon[3]
    local LevelCon = InfoCon:GetChildAt(3)
    local TxtLvTitle = LevelCon:GetChildAt(0)
    local TxtLv = LevelCon:GetChildAt(1)

    -- TODO: 加上存档编号
    local SaveDescId_txt = "存档"
    local TxtName_txt =  TxtName:GetText():ToString()
    local TxtLvTitle_txt =  TxtLvTitle:GetText():ToString()
    local TxtLv_txt = TxtLv:GetText():ToString()
    local TxtDate_txt =  TxtDate:GetText():ToString()
    local TxtTime_txt =  TxtTime:GetText():ToString()
    local TxtPlayTimeTitle_txt =  TxtPlayTimeTitle:GetText():ToString()
    local TxtPlayTime_txt =  TxtPlayTime:GetText():ToString()

    local DescTable = {
        -- 存档地点
        SaveDescId_txt, TxtName_txt,
        -- 存档日期
        TxtDate_txt, TxtTime_txt,
        -- 游戏时长
        TxtPlayTimeTitle_txt, TxtPlayTime_txt,
        -- 等级
        TxtLvTitle_txt, TxtLv_txt
    }

    return table.concat(DescTable, " ")
end -- BI_ArchivesBtnV2_C

-- 主界面/主菜单/小曲 (音乐)
--[[
TODO:
- 读出功能按钮
- 歌曲编号

BI_ArchivesBtnV2_C.WidgetTree.RootWidget
CanvasPanel_0
[0] CanvasPanel_28
 [0] ImgBgShadow
 [1] ImgBgBloom
 [2] ImgArchivesBg
 [3] ImgSelected
 [4] InfoCon
  [0] ImgLine
  [1] HorizontalBox_0
   [0] TxtName
   [1] TxtRestartGame
  [2] TimeCon
   [0] TxtPlayTimeTitle
   [1] TxtPlayTime
   [2] TxtDate
   [3] TxtTime
  [3] LevelCon
   [0] TxtLvTitle
   [1] TxtLv
  [4] NGPCon
   [0] HorizontalBox_58
    [0] MultiCon
     [0] ScaleBox_0
      [0] TxtNewGameMulti
    [1] TxtNewGame
    [2] TxtNewGameNum
    [3] TxtNewGame2
 [5] NGMarkerCon
  [0] ImgNGMarker
 [6] FocusWidget
]]
WkGlobals.GetTextFuncMap["BI_AccordionChildBtn_Echo_C"] = WkGlobals.GetTextFuncMap["BI_StartGame_C"]
