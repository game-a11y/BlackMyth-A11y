-- SPDX-License-Identifier: MIT
-- WuKong Benchmark Tools Mod
-- Author: inkydragon
local SubModeName = "[BlackMythA11y.WkbHook] "
-- [[require Global]]

-- [[require Local]]

-- [[Global Var]]
local WkbHook = {}

-- 【性能测试工具】 测试报告输出
-- @param WkUIGlobals 全局变量捕获
function WkbHook.BenchMarkReportBind(WkUIGlobals)
    local msg = "性能报告"
    print(msg.."\n")
    A11yTolk:Speak(msg)

    -- BUI_LearnTalent_C
    local A11yReport_txt = ""
    local BUI_BenchMark_V2_C = WkUIGlobals["BUI_BenchMark_V2_C"]
    -- BUI_BenchMark_V2_C = FindFirstOf("BUI_BenchMark_V2_C")
    local isClassGood = BUI_BenchMark_V2_C and BUI_BenchMark_V2_C:IsValid()
    if not isClassGood then
        A11yReport_txt = "无障碍 MOD 未找到性能报告，请截图 OCR 以确认测试是否结束"
        print(string.format("%s\n", A11yReport_txt))
        A11yTolk:Speak(A11yReport_txt, true)
        return
    end

--[[BI_BenchMarkHistoryBtn_C.WidgetTree.RootWidget
[0] BtnCon
 [0] ResizeCon
  [0] ImgBar
  [1] ImgBarCk
 [1] ContentCon
  [0] TxtName
  [1] TxtTime
 [2] ArrowCon
 [3] FocusWidget
]]
    -- BI_HistoryBtn
    local BI_HistoryBtn = BUI_BenchMark_V2_C.BI_HistoryBtn
    print(string.format("%s\n", BI_HistoryBtn:GetFullName()))

    local Canvas_full = BUI_BenchMark_V2_C.WidgetTree.RootWidget:GetChildAt(0)
    local ResultsCon = Canvas_full:GetChildAt(5)
    local TitleResults = ResultsCon:GetChildAt(0)
    local Txt_timeStamp = TitleResults:GetChildAt(1):GetChildAt(1)
    -- TODO: 标题、单位
    local ListResults = ResultsCon:GetChildAt(1)
    --                            .ListResults.VerticalBox.HorizontalBox_11.Txt_FPSAbs
    local Txt_FPSAbs = ListResults:GetChildAt(0):GetChildAt(1):GetChildAt(1):GetChildAt(0)
    local Txt_FPSMax = ListResults:GetChildAt(1):GetChildAt(1):GetChildAt(0):GetChildAt(1):GetChildAt(0)
    local Txt_FPSMin = ListResults:GetChildAt(1):GetChildAt(1):GetChildAt(1):GetChildAt(1):GetChildAt(0)
    local TxtTitle_95 = ListResults:GetChildAt(2):GetChildAt(1):GetChildAt(1):GetChildAt(0)
    local Txt_VideoMem = ListResults:GetChildAt(4):GetChildAt(1):GetChildAt(1):GetChildAt(0)
    -- print(string.format("ResultsCon=%s\n", ResultsCon:GetFullName()))

    local Txt_timeStamp_str = Txt_timeStamp:GetText():ToString()
    if "2024/08/20 08:20" == Txt_timeStamp_str then
        -- 性能测试中, 报告日期为特定时间
        local A11yReport_txt = "性能测试中..."
        print(A11yReport_txt.."\n")
        A11yTolk:Speak(A11yReport_txt, true)
        return
    end

    local DescTable = {
        -- 1111
        "性能总结",
        Txt_timeStamp_str,
        "平均帧率", Txt_FPSAbs:GetText():ToString(),
        "最高帧率", Txt_FPSMax:GetText():ToString(),
        "最低帧率", Txt_FPSMin:GetText():ToString(),
        "百分之95帧率", TxtTitle_95:GetText():ToString(),
        "显存占用", Txt_VideoMem:GetText():ToString(), "GB",
        
        -- "系统信息", "(跳过)",
        
        -- "画面设置", 
    }

    -- join string
    local A11yReport_txt = table.concat(DescTable, " ")
    print(A11yReport_txt.."\n")
    A11yTolk:Speak(A11yReport_txt, true)
end

return WkbHook
