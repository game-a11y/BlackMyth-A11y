-- SPDX-License-Identifier: MIT
-- BlackMythA11y Mod
local ModName = "[BlackMythA11y] "
-- Author: inkydragon
local UEHelpers = require("UEHelpers")
local WkUtils = require("WkUtils")

local GetGameplayStatics = UEHelpers.GetGameplayStatics
-- Global Var
local WidgetLib = nil


local function PrintCanvas(widget)
    if widget and widget:IsValid() then
        local slot= WidgetLib:SlotAsCanvasSlot(widget)
        WkUtils.PrintUObject(widget)
        print(tostring(widget:GetFullName()))
        print(tostring(slot:GetFullName()))
	end
end

local function DumpInfo()
    print(ModName.."DumpInfo...\n")
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
    print(string.format(ModName.."GetFullName: %s\n", GetGameplayStatics():GetFullName()))
    print(string.format(ModName.."GetPlatformName: %s\n", GetGameplayStatics():GetPlatformName():ToString()))
    local GameMode = GetGameplayStatics():GetGameMode(UWorld)
    print(string.format(ModName.."GetLevel: %s\n", GameMode:GetLevel():GetFullName()))

    WkUtils.PrintUIPage()
    -- print(ModName.."-----------\n")
    -- WkUtils.PrintGameInfo()
    
    print(ModName.."Dump UI...\n")
    BattleMainUI = FindFirstOf("BUI_BattleMain_C")
	if not BattleMainUI:IsValid() then
		print(ModName.."Game not ready, Abort\n")
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

RegisterKeyBind(Key.F11, DumpInfo)
