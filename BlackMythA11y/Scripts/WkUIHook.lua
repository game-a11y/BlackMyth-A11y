-- SPDX-License-Identifier: MIT
-- WuKong UI Hooks
-- Author: inkydragon
local SubModeName = "[BlackMythA11y.WkUIHook] "
-- [[require Global]]

-- [[require Local]]
local WkGlobals = require("WkGlobals")
local WkUtils = require("WkUtils")
local WkConfig = require("WkConfig")
-- [[Global Var]]
local WkUIHook = {}
-- 开发中标记
local DevNote = "开发中! "


-- [[ 辅助函数 ]] -----------------------------------------------------------------
-- WkUtils.PrintAllParents


-- [[ Hook 事件处理函数 ]] ----------------------------------------------------------

-- 首次加载 主界面
-- 主界面/主菜单: 所有按钮
-- 主界面/主菜单/继续游戏 （无需特殊处理）
-- 主界面/主菜单/新游戏 （无需特殊处理）
-- 主界面/主菜单/载入游戏
-- 主界面/主菜单/小曲 (音乐)
require("UIHooks.HomeScreen")

-- 主界面/设置: 一级菜单
-- 主界面/设置: 左右单项选择
-- 主界面/设置: 下拉单项选择
-- 主界面/设置: 下拉项
-- 主界面/设置: 水平滑块
-- 主界面/设置: 图标按钮
-- 主界面/设置: 文本按钮
-- 主界面/设置: 图标按钮-键盘键位
require("UIHooks.SettingMenu")

-- 游戏中:土地庙:菜单
-- 游戏中:土地庙:菜单:缩地
require("UIHooks.Tudi")

-- 游戏中:背包-1:修行（技能）
require("UIHooks.Ability")
-- 游戏中:背包0:披挂（装备）
require("UIHooks.EquipItem")
-- 游戏中:背包1:行囊（道具物品）
require("UIHooks.InventoryItem")
-- 游戏中:背包2:游记
require("UIHooks.TravelNotes")
-- 游戏中:背包3:设置    SKIP

-- 游戏中:交互/弹出提示
require("UIHooks.InteractIcon")


-- TODO: 加载界面

-- 二次确定对话框
--[[
BUI_Reconfirm_C_2147454606.WidgetTree.txt
BUI_Reconfirm_C_2147454606.WidgetTree.Btn_Confirm
BUI_Reconfirm_C_2147454606.WidgetTree.Btn_Cancel

BI_ReconfirmBtn_C.WidgetTree.RootWidget
[0] HoverRoot
 [0] BtnCon
  [0] ImgBg
  [1] TxtName
  [2] FocusWidget
]]
-- TODO: 构造时 BI_ReconfirmBtn_C 读出 .txt
-- GSRichScaleText /.BUI_Reconfirm_C_2147454627.WidgetTree.txt
-- /BUI_Reconfirm_C.WidgetTree.Btn_Cancel
WkGlobals.GetTextFuncMap["BI_ReconfirmBtn_C"] = function(Button, InFocusEvent)
    local ClassName = Button:GetFName():ToString()

    local HoverRoot = Button.WidgetTree.RootWidget:GetChildAt(0)
    local BtnCon = HoverRoot:GetChildAt(0)
    local TxtName = BtnCon:GetChildAt(1)  -- .PSSlot2
    local TxtName_txt = TxtName:GetText():ToString()  -- TextBlock

    if "Btn_Confirm" == ClassName then
        local HBoxBtn = Button:GetParent()
        local BtnCon = HBoxBtn:GetParent()
        local BoxCon = BtnCon:GetParent()
        --
        local ContentCon = BoxCon:GetChildAt(1)
        local txt = ContentCon:GetChildAt(0)
        local ContentCon_txt = txt:GetText():ToString()
        -- print(string.format("txt=%s:  %s\n", txt:GetFullName(), ContentCon_txt))

        return string.format("%s %s", TxtName_txt, ContentCon_txt)
    end

    return TxtName_txt
end



-- [[ Hook 函数 ]] --------------------------------------------------------------
local function OnAddedToFocusPath_Hook(pContext, pInFocusEvent)
    local Button = pContext:get()
    local SuperClassName = Button:GetClass():GetFName():ToString()
    local ClassName = Button:GetFName():ToString()

    -- TODO: 根据原因筛选相应，消除重复的事件
    local InFocusEvent = pInFocusEvent:get()
    -- WkUtils.PrintUObject(TxtName_name)
    -- https://github.com/UE4SS-RE/RE-UE4SS/issues/378
    -- local UGSE_UMGFuncLib = StaticFindObject("/Script/UnrealExtent.Default__GSE_UMGFuncLib")
    -- Cause = UGSE_UMGFuncLib:GetFocusEventCause(InFocusEvent)

    print(string.format(
        "OnAddedToFocusPath(%s): %s\n",
        tostring(InFocusEvent:GetFullName()),
        tostring(Button:GetFullName())))

    -- Print text
    if WkGlobals.GetTextFuncMap[SuperClassName] then
        -- 默认打断输出
        InterruptSpeak = true
        local curText = WkGlobals.GetTextFuncMap[SuperClassName](Button, InFocusEvent)
        print(string.format("\t\"%s\"\n", curText))
        A11yTolk:Speak(curText, InterruptSpeak)
    else
        print(string.format("Cannot gettext for:  %s <: %s\n", ClassName, SuperClassName))
    end
end

-- 初始化所有 UI 钩子
function WkUIHook.InitUiHooks()
    local Hook_BUI_Widget_Class = {}
    -- 性能测试结果
    Hook_BUI_Widget_Class["BUI_BenchMark_V2_C"] = 1
    Hook_BUI_Widget_Class["BUI_Setting_C"] = 1
    -- -- Hook_BUI_Widget_Class["BUI_LoadingV2_C"] = 1  -- 没有挂钩到加载页面
    -- Hook_BUI_Widget_Class["BUI_LearnTalent_C"] = 1

    NotifyOnNewObject("/Script/b1-Managed.BUI_Widget", function(ConstructedObject)
        local FullName = ConstructedObject:GetFullName()
        local SuperClassName = ConstructedObject:GetClass():GetFName():ToString()
        local ClassName = ConstructedObject:GetFName():ToString()

        if not Hook_BUI_Widget_Class[SuperClassName] then
            return
        end
        
        -- 保存引用
        WkGlobals.UIGlobals[SuperClassName] = ConstructedObject
        print(string.format("BUI_Widget Constructed: %s\n", FullName))
    end)
    RegisterHook("/Script/b1-Managed.BUI_Widget:Destruct", function(Context)
        local Button = Context:get()
        local FullName = Button:GetFullName()
        local SuperClassName = Button:GetClass():GetFName():ToString()
        local ClassName = Button:GetFName():ToString()

        if not Hook_BUI_Widget_Class[SuperClassName] then
            return
        end

        -- 清除引用
        WkGlobals.UIGlobals[SuperClassName] = nil
        print(string.format("BUI_Widget Destruct: %s\n", FullName))
    end)

    RegisterHook("/Script/b1-Managed.BUI_Button:OnAddedToFocusPath", OnAddedToFocusPath_Hook)
    -- RegisterHook("/Script/b1-Managed.BUI_Widget:OnAddedToFocusPath", OnAddedToFocusPath_Hook)

    RegisterHook("/Script/b1-Managed.BUI_Button:OnMouseButtonDown", function(Context, MyGeometry, MouseEvent)
        local Button = Context:get()
        local FullName = Button:GetFullName()
        local SuperClassName = Button:GetClass():GetFName():ToString()
        local ClassName = Button:GetFName():ToString()
        print(string.format("OnMouseButtonDown: %s <: %s\n\t%s\n",
            ClassName, SuperClassName, FullName))
    end)

    -- hook 模拟输入
    ---@param Context UBUI_Button
    ---@param MyGeometry FGeometry
    ---@param InAnalogInputEvent FAnalogInputEvent
    -- RegisterHook("/Script/b1-Managed.BUI_Button:OnAnalogValueChanged", function(Context, MyGeometry, InAnalogInputEvent)
    --     local Button = Context:get()
    --     local AnalogInputEvent = InAnalogInputEvent:get()
    --     local FullName = Button:GetFullName()
    --     local SuperClassName = Button:GetClass():GetFName():ToString()
    --     local ClassName = Button:GetFName():ToString()
        
    --     if not string.find(ClassName, "Slider") then
    --         return
    --     end

    --     print(string.format("OnAnalogValueChanged(%s): %s <: %s\n\t%s\n",
    --         AnalogInputEvent:GetFName():ToString(),
    --         ClassName, SuperClassName, FullName))
    -- end)

    -- hook 按键抬起输入
    -- TODO: 目前不能按 InKeyEvent 筛选事件
    -- RegisterHook("/Script/b1-Managed.BUI_Button:OnKeyUp", function(Context, MyGeometry, InKeyEvent)
    --     local Button = Context:get()
    --     local AnalogInputEvent = InKeyEvent:get()
    --     local FullName = Button:GetFullName()
    --     local SuperClassName = Button:GetClass():GetFName():ToString()
    --     local ClassName = Button:GetFName():ToString()
        
    --     local log_class = {}
    --     log_class["BI_SettingFixedItem_C"] = 1
    --     log_class["BI_SettingSliderItem_C"] = 1
    --     if not log_class[SuperClassName] then
    --         return
    --     end

    --     print(string.format("OnKeyUp(%s): %s <: %s\n\t%s\n",
    --         AnalogInputEvent:GetFName():ToString(),
    --         ClassName, SuperClassName, FullName))
    --     print(string.format("InKeyEvent(%s): %s\n",
    --     InKeyEvent, AnalogInputEvent:GetFullName()))
    -- end)
    
    -- 返回的是上一步的对象
    -- RegisterHook("/Script/b1-Managed.BUI_Button:OnCustomWidgetNavigation", function(Context, Navigation)

    -- 会显示父级的聚焦事件
    -- RegisterHook("/Script/b1-Managed.BUI_Widget:OnFocusChanging", function(self, InFocusEvent)

    -- 闪退 hook "/Script/b1-Managed.BUI_Widget:OnAnimationSequenceEvent"
end

-- bmb 性能测试工具
if WkConfig.IsBencmarkTools then
    local WkbHook = require("WkbHook")
    RegisterKeyBind(Key.F12, function()
        WkbHook.BenchMarkReportBind(WkGlobals.UIGlobals)
    end)
end -- WkConfig.IsBencmarkTools

return WkUIHook
