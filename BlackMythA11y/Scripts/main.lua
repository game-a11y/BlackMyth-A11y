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


-- [[ 初始化按键绑定 ]] ---------------------------------------------------------
WkKeyBind.init()


-- [[ 初始化 /Script 挂钩 ]] --------------------------------------------------
-- <: UTextLayoutWidget
RegisterHook("/Script/UMG.TextBlock:SetText", function(Context, InText)
    local Button = Context:get()
    local ClassFullName = Button:GetFullName()
    local Text = InText:get():ToString()
    print(string.format("[TextBlock] \"%s\"\n", Text))
    print(string.format("\t%s\n", ClassFullName))
end)
RegisterHook("/Script/UMG.RichTextBlock:SetText", function(Context, InText)
    local Button = Context:get()
    local ClassFullName = Button:GetFullName()
    local Text = InText:get():ToString()
    print(string.format("[RichTextBlock] \"%s\"\n", Text))
    print(string.format("\t%s\n", ClassFullName))
end)
RegisterHook("/Script/UMG.MultiLineEditableText:SetText", function(Context, InText)
    local Button = Context:get()
    local ClassFullName = Button:GetFullName()
    local Text = InText:get():ToString()
    print(string.format("[MultiLineEditableText] \"%s\"\n", Text))
    print(string.format("\t%s\n", ClassFullName))
end)
RegisterHook("/Script/UMG.MultiLineEditableTextBox:SetText", function(Context, InText)
    local Button = Context:get()
    local ClassFullName = Button:GetFullName()
    local Text = InText:get():ToString()
    print(string.format("[MultiLineEditableTextBox] \"%s\"\n", Text))
    print(string.format("\t%s\n", ClassFullName))
end)
-- <: UWidget
RegisterHook("/Script/UMG.EditableText:SetText", function(Context, InText)
    local Button = Context:get()
    local ClassFullName = Button:GetFullName()
    local Text = InText:get():ToString()
    print(string.format("[EditableText] \"%s\"\n", Text))
    print(string.format("\t%s\n", ClassFullName))
end)
RegisterHook("/Script/UMG.EditableTextBox:SetText", function(Context, InText)
    local Button = Context:get()
    local ClassFullName = Button:GetFullName()
    local Text = InText:get():ToString()
    print(string.format("[EditableTextBox] \"%s\"\n", Text))
    print(string.format("\t%s\n", ClassFullName))
end)
-- Function /Script/UnrealExtent.GSBitmapFontBox:SetText
RegisterHook("/Script/UnrealExtent.GSBitmapFontBox:SetText", function(Context, InText)
    local Button = Context:get()
    local ClassFullName = Button:GetFullName()
    local Text = InText:get():ToString()
    print(string.format("[GSBitmapFontBox] \"%s\"\n", Text))
    print(string.format("\t%s\n", ClassFullName))
end)
RegisterHook("/Script/UnrealExtent.GSRichTextBlock:ApplyText", function(Context, InText)
    local Button = Context:get()
    local ClassFullName = Button:GetFullName()
    local Text = InText:get():ToString()
    print(string.format("[GSRichTextBlock] \"%s\"\n", Text))
    print(string.format("\t%s\n", ClassFullName))
end)
RegisterHook("/Script/UnrealExtent.GSTextBlock:ApplyText", function(Context, InText)
    local Button = Context:get()
    local ClassFullName = Button:GetFullName()
    local Text = InText:get():ToString()
    print(string.format("[GSTextBlock] \"%s\"\n", Text))
    print(string.format("\t%s\n", ClassFullName))
end)

-- UMG  UUserWidget:OnFocusReceived(MyGeometry, InFocusEvent)
--      UUserWidget:OnAddedToFocusPath(InFocusEvent)


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
