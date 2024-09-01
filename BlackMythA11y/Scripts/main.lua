-- SPDX-License-Identifier: MIT
-- BlackMythA11y Mod
local ModName = "[BlackMythA11y] "
-- Author: inkydragon
local UEHelpers = require("UEHelpers")
local WkUtils = require("WkUtils")

local GetGameplayStatics = UEHelpers.GetGameplayStatics
-- Global Var
local WidgetLib = nil
local bModInit = false


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


local function InitManagedHook()
    RegisterHook("/Script/b1-Managed.BUI_Button:OnMouseButtonDown", function(Context, MyGeometry, MouseEvent)
        Button = Context:get()
        print(string.format("OnMouseButtonDown: %s\n", tostring(Button:GetFullName())))
        WidgetTree = Button.WidgetTree
        -- WkUtils.PrintUObject(WidgetTree)
        -- WkUtils.PrintUObject(WidgetTree.TxtName)
    
        -- -- TxtName_name = tostring(Button:GetFullName()) .. ".WidgetTree.TxtName"
        -- WkUtils.PrintUObject(Button)
        -- -- TxtName = FindFirstOf("GSTextBlock /Engine/Transient.GameEngine_2147482611:BGW_GameInstance_B1_2147482576.BUI_B1_Root_V2_C_2147480953.WidgetTree.BUI_StartGame_C_2147477556.WidgetTree.BI_StartGameBtn_0")
        -- -- WkUtils.PrintUObject(TxtName)
    
        -- TxtName = WidgetTree.TxtName:get()
        -- WkUtils.PrintUObject(WidgetTree)
        -- if TxtName ~= nil then
        --     BtnText = TxtName
        --     print(string.format("\t%s\n", tostring(TxtName:GetFullName())))
        -- end
    end)
    
    local OnAddedToFocusPath_Hook = function(pContext, pInFocusEvent)
        Button = pContext:get()
        -- TODO: 根据原因筛选相应，消除重复的事件
        InFocusEvent = pInFocusEvent:get()
        -- WkUtils.PrintUObject(UGSE_UMGFuncLib)
        -- https://github.com/UE4SS-RE/RE-UE4SS/issues/378
        -- local UGSE_UMGFuncLib = StaticFindObject("/Script/UnrealExtent.Default__GSE_UMGFuncLib")
        -- Cause = UGSE_UMGFuncLib:GetFocusEventCause(InFocusEvent)
        print(string.format("OnAddedToFocusPath(%s): %s\n", tostring(InFocusEvent:GetFullName()), tostring(Button:GetFullName())))
    end
    -- RegisterHook("/Script/b1-Managed.BUI_Button:OnAddedToFocusPath", OnAddedToFocusPath_Hook)
    -- RegisterHook("/Script/b1-Managed.BUI_Widget:OnAddedToFocusPath", OnAddedToFocusPath_Hook)
    -- 返回的是上一步的对象
    -- RegisterHook("/Script/b1-Managed.BUI_Button:OnCustomWidgetNavigation", function(Context, Navigation)
    --     Button = Context:get()
    --     print(string.format("OnCustomWidgetNavigation: %s\n", tostring(Button:GetFullName())))
    -- end)
    
    -- NOTE: 会显示父级的聚焦事件
    -- RegisterHook("/Script/b1-Managed.BUI_Widget:OnFocusChanging", function(self, InFocusEvent)
    --     print(string.format("OnFocusChanging: %s\n", tostring(self:get():GetFullName())))
    -- end)

    -- NOTE: hook "/Script/b1-Managed.BUI_Widget:OnAnimationSequenceEvent" 闪退
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


-- RegisterHook("/Script/UMG.TextBlock:SetText", function(Context, InText)
--     Button = Context:get()
--     print(string.format("SetText: %s\n", tostring(Button:GetFullName())))
--     print(string.format("\t%s\n", tostring(InText:get():ToString())))
-- end)
