-- SPDX-License-Identifier: MIT
-- Author: inkydragon
-- BlackMythA11y Mod
local ModName = "[BlackMythA11y] "
-- [[require Global]]

-- [[require Local]]
local WkGlobals = require("WkGlobals")
local WkUtils = require("WkUtils")
local WkUIHook = require("WkUIHook")
local WkKeyBind = require("WkKeyBind")
-- [[Global Var]]


-- [[ 初始化按键绑定 ]] ---------------------------------------------------------
WkKeyBind.init()


-- [[ 初始化 /Script 挂钩 ]] --------------------------------------------------
require("WkScriptHooks")

-- [[ 初始化 Managed 挂钩 ]] --------------------------------------------------
-- NOTE: 需要等 UE 加载完后才能挂钩

-- 仅在游戏初始化后执行挂钩
local function AfterInitGameStateHook()
    WkUIHook.InitUiHooks()
end

-- InitGameState 游戏已经初始化了
local WkGameStart = ModRef:GetSharedVariable("WkGameStart")
if WkGameStart and type(WkGameStart) == "boolean" then
    print(ModName.."Mod hot-reloading.\n")
    AfterInitGameStateHook()
else
    print(ModName.."First Start Game.\n")
    -- Called after AGameModeBase::InitGameState
    -- 第一次启动时，为延迟构造的函数挂钩
    RegisterInitGameStatePostHook(function(GameState)
        if ModRef:GetSharedVariable("WkGameStart") then
            return -- 避免多次重复挂钩
        end

        -- 持久标记游戏初始化状态
        ModRef:SetSharedVariable("WkGameStart", true)
        print(ModName.."AfterInitGameStateHook\n")
        AfterInitGameStateHook()
    end)
end
