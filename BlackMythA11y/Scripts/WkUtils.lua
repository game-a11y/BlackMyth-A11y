-- SPDX-License-Identifier: MIT
-- WuKong Utils
-- Author: inkydragon
local WkUtils = {}

-- 打印当前已加载的所有关卡(Level)
function WkUtils.DumpAllLevels()
    local LevelInstances = FindAllOf("Level")
    if not LevelInstances then
        print("[WkUtils] No instances of 'Level' were found\n")
        return
    end

    print("[WkUtils] Begin to dump all Levels:\n")
    for Index, Level in pairs(LevelInstances) do
        print(string.format("[%d] %s\n", Index, Level:GetFullName()))
    end
end

-- 打印游戏版本号
function WkUtils.PrintGameVersion()
    local GSVersionSettings = StaticFindObject("/Script/b1.Default__GSVersionSettings")
    if not GSVersionSettings then return end

    local AppVersion = GSVersionSettings.AppVersion  -- :FString
    print(string.format("[WkUtils] Game Version: %s.%s\n",
        AppVersion:ToString(), GSVersionSettings.Revision))
end

return WkUtils
