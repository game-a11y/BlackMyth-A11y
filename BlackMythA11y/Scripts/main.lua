-- SPDX-License-Identifier: MIT
-- Author: inkydragon
-- BlackMythA11y Mod
local ModName = "[BlackMythA11y] "
-- [[require Global]]

-- [[require Local]]
local WkUtils = require("WkUtils")
local WkUIHook = require("WkUIHook")
local WkKeyBind = require("WkKeyBind")
-- [[Global Var]]
local bModInit = false


-- [[ 初始化按键绑定 ]] ---------------------------------------------------------
WkKeyBind.init()


-- [[ 初始化 /Script 挂钩 ]] --------------------------------------------------
-- RegisterHook("/Script/UMG.TextBlock:SetText", function(Context, InText)
--     Button = Context:get()
--     print(string.format("SetText: %s\n", tostring(Button:GetFullName())))
--     print(string.format("\t%s\n", tostring(InText:get():ToString())))
-- end)


-- [[ 初始化 Managed 挂钩 ]] --------------------------------------------------
-- NOTE: 需要等 UE 加载完后才能挂钩

local function InitManagedHook()
    WkUIHook.InitUiHooks()
end

-- Called after AGameModeBase::InitGameState
-- 第一次启动时，为延迟构造的函数挂钩
RegisterInitGameStatePostHook(function(GameState)
    if bModInit then
        return
    end

    bModInit = true
    -- These variables do not get reset when hot-reloading.
    ModRef:SetSharedVariable("WkGameStart", true)
    print(ModName.."InitGameStatePostHook\n")
    InitManagedHook()
end)

local WkGameStart = ModRef:GetSharedVariable("WkGameStart")
if WkGameStart and type(WkGameStart) == "boolean" then
    print("Mod hot-reloading.\n")
    InitManagedHook()
else
    print("First Start Game.\n")
end
