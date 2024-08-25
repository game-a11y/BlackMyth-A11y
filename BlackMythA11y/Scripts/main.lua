-- BlackMythA11y Mod
local UEHelpers = require("UEHelpers")

-- Importing functions to the global namespace of this mod just so that we don't have to retype 'UEHelpers.' over and over again.
local GetGameplayStatics = UEHelpers.GetGameplayStatics
local GetPlayerController = UEHelpers.GetPlayerController


RegisterHook("/Script/Engine.PlayerController:ClientRestart", function (self, NewPawn) 
    local PlayerController = self:get()

    print("[BlackMythA11y] Mod Start...")
end)

local function DumpAllLevels()
    local ActorInstances = FindAllOf("Level")
    if not ActorInstances then
        print("No instances of 'Level' were found\n")
    else
        for Index, ActorInstance in pairs(ActorInstances) do
            print(string.format("[%d] %s\n", Index, ActorInstance:GetFullName()))
        end
    end
end


local function DumpAllWidgets()
    -- 获取当前世界的玩家控制器
    local PlayerController = UEHelpers:GetWorldContextObject()

    if PlayerController ~= nil then
        -- 获取玩家控制器中的所有 Widgets
        local widgets = PlayerController.PlayerState.PlayerUI:GetAllWidgets()

        -- 遍历所有 Widgets
        for i = 1, widgets:Num() do
            local widget = widgets:Get(i)
            if widget ~= nil then
                -- 获取当前 Widget 的类名称
                local widgetClass = widget:GetClass()
                local widgetClassName = widgetClass:GetName()

                -- 打印当前 Widget 的类名称
                print("Current Widget Class: " .. widgetClassName)
            end
        end
    else
        print("PlayerController is nil")
    end
end


local function DumpInfo()
    print("[BlackMythA11y] DumpInfo...")
    local UWorld = UEHelpers.GetWorld()
    DumpAllWidgets()

    -- 1. 检查当前关卡的名称
    -- if UWorld then
    --     print(string.format("UWorld:GetMapName: %s\n", UWorld:GetMapName()))
    -- else
    --     print("Null UWorld")
    -- end
    
    -- 2. 检查当前的游戏模式或状态
    print(string.format("[BlackMythA11y] GetFullName: %s\n", GetGameplayStatics():GetFullName()))
    print(string.format("[BlackMythA11y] GetPlatformName: %s\n", GetGameplayStatics():GetPlatformName():ToString()))
    local GameMode = GetGameplayStatics():GetGameMode(UWorld)
    print(string.format("[BlackMythA11y] GetLevel: %s\n", GameMode:GetLevel():GetFullName()))

    -- local PlayerController = GetPlayerController()
    -- local PlayerPawn = PlayerController.Pawn
    -- local CameraManager = PlayerController.PlayerCameraManager

    -- print(string.format("PlayerPawn: %s\n", PlayerPawn:GetName()))
end

RegisterKeyBind(Key.F11, DumpInfo)
