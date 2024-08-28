-- SPDX-License-Identifier: MIT
-- BlackMythA11y Mod
local ModName = "[BlackMythA11y] "
-- Author: inkydragon
local UEHelpers = require("UEHelpers")
local WkUtils = require("WkUtils")

local GetGameplayStatics = UEHelpers.GetGameplayStatics


local function DumpInfo()
    print(ModName.."DumpInfo...\n")

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
end

RegisterKeyBind(Key.F11, DumpInfo)
