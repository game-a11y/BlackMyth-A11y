-- SPDX-License-Identifier: MIT
-- Author: inkydragon
-- WuKong KeyBind Hook
local SubModeName = "[BlackMythA11y.WkKeyBind] "
-- [[require Global]]
local UEHelpers = require("UEHelpers")
-- [[require Local]]
local WkUtils = require("WkUtils")
-- [[Global Var]]
local WkKeyBind = {}
local GetGameplayStatics = UEHelpers.GetGameplayStatics
local KeyBindArgMap = {}


local function PrintCanvas(widget)
    if widget and widget:IsValid() then
        local slot= WidgetLib:SlotAsCanvasSlot(widget)
        WkUtils.PrintUObject(widget)
        print(tostring(widget:GetFullName()))
        print(tostring(slot:GetFullName()))
	end
end

local function DumpInfo()
    print(SubModeName.."DumpInfo...\n")
    WidgetLib = StaticFindObject("/Script/UMG.Default__WidgetLayoutLibrary")

    WkUtils.PrintGameVersion()

    local UWorld = UEHelpers.GetWorld()
    -- WkUtils.DumpAllLevels()

    -- 1. 检查当前关卡的名称
    -- if UWorld then
    --     print(string.format("UWorld:GetMapName: %s\n", UWorld:GetMapName()))
    -- else
    --     print("Null UWorld")
    -- end
    
    -- 2. 检查当前的游戏模式或状态
    print(string.format(SubModeName.."GetFullName: %s\n", GetGameplayStatics():GetFullName()))
    print(string.format(SubModeName.."GetPlatformName: %s\n", GetGameplayStatics():GetPlatformName():ToString()))
    local GameMode = GetGameplayStatics():GetGameMode(UWorld)
    print(string.format(SubModeName.."GetLevel: %s\n", GameMode:GetLevel():GetFullName()))

    WkUtils.PrintUIPage()
    -- print(SubModeName.."-----------\n")
    -- WkUtils.PrintGameInfo()
    
    print(SubModeName.."Dump UI...\n")
    BattleMainUI = FindFirstOf("BUI_BattleMain_C")
	if not BattleMainUI:IsValid() then
		print(SubModeName.."Game not ready, Abort\n")
		return false
	end
    prefix = BattleMainUI:GetFullName():gsub("([^ ]*) (.*)","%2")..".WidgetTree." -- BattleMainUI object name
    
    BUI_BattleMain_C_WidgetTree = StaticFindObject(prefix.."PlayerStCon")

    hpbar=StaticFindObject(prefix.."PlayerStCon")
    fpbar=StaticFindObject(prefix.."BI_PlayerStickLevel")
    fpbar2=StaticFindObject(prefix.."BI_DaShengStickLevel")--大圣套棍势
    --StaticFindObject(prefix.."BI_DaShengStickLevel"):SetVisibility(0)
    --StaticFindObject(prefix.."BI_Trans"):SetVisibility(0)
    skillshortcut=StaticFindObject(prefix.."BI_ShortcutSkill")
    itemshortcut=StaticFindObject(prefix.."BI_ShortcutItem")
    treasureshortcut=StaticFindObject(prefix.."BI_Treasure")
    rzdshortcut=StaticFindObject(prefix.."BI_RZDSkill")

    PrintCanvas(hpbar)
    PrintCanvas(fpbar)
end
-- KeyBindArgMap[Key.F11] = DumpInfo

-- 测试调用 Cpp 注入的模块
local function CallCppModTest()
    print(SubModeName.."CallCppModTest...\n")

    print(string.format(SubModeName.."A11yTolk: %s\n", A11yTolk))
    local Major, Minor, Hotfix = A11yTolk:GetVersion()
    print(string.format(SubModeName.."A11yTolk v%d.%d.%d\n", Major, Minor, Hotfix))
    print(string.format(SubModeName.."A11yTolk:Speak(string)\n"))
    A11yTolk:Speak("aaaaa")
    A11yTolk:Speak("你好")
end
KeyBindArgMap[Key.F11] = CallCppModTest


function WkKeyBind.init()
    for key, func in pairs(KeyBindArgMap) do --actualcode
        RegisterKeyBind(key, func)
    end
end

return WkKeyBind
