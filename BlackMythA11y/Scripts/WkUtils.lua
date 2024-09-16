-- SPDX-License-Identifier: MIT
-- WuKong Utils
-- Author: inkydragon
local SubModeName = "[BlackMythA11y.WkUtils] "
-- [[require Global]]

-- [[require Local]]

-- [[Global Var]]
local WkUtils = {}


-- 打印当前已加载的所有关卡(Level)
function WkUtils.DumpAllLevels()
    local LevelInstances = FindAllOf("Level")
    if not LevelInstances then
        print(SubModeName.."No instances of 'Level' were found\n")
        return
    end

    print(SubModeName.."Begin to dump all Levels:\n")
    for Index, Level in pairs(LevelInstances) do
        print(string.format(SubModeName.."[%d] %s\n", Index, Level:GetFullName()))
    end
end

-- 打印游戏版本号
function WkUtils.PrintGameVersion()
    local GSVersionSettings = StaticFindObject("/Script/b1.Default__GSVersionSettings")
    if not GSVersionSettings then return end

    local AppVersion = GSVersionSettings.AppVersion  -- :FString
    print(string.format(SubModeName.."Game Version: %s.%s\n",
        AppVersion:ToString(), GSVersionSettings.Revision))
end

function WkUtils.PrintUObject(uobj)
    if not uobj then return end
    print(string.format(SubModeName.."raw:  UObject=%s; FullName=%s\n", uobj, uobj:GetFullName()))
    --- UIPageList TArray<EUIPageID>
end

-- 打印当前 UI 页面名称
function WkUtils.PrintUIPage()
    --- UIPageID EUIPageID
    --- DialogueID int32
    --- NameID int32
    --- ChapterID int32
    --- MediaId int32
    --- BSNS_ShowSpecialUI /Script/b1-Managed.Default__BSNS_ShowSpecialUI
    local ShowSpecialUI = StaticFindObject("/Script/b1-Managed.Default__BSNS_ShowSpecialUI")
    if not ShowSpecialUI then return end
    -- print(string.format(SubModeName.."%s: %s\n", ShowSpecialUI, ShowSpecialUI:GetFullName()))
    WkUtils.PrintUObject(ShowSpecialUI)
    --- UIPageList TArray<EUIPageID>
    -- local ShowUIArray = StaticFindObject("/Script/b1-Managed.UBSN_ShowUI")
    -- if not ShowUIArray then return end

    print(string.format(SubModeName.."Print:  raw=%s\n", ShowSpecialUI:GetDisplayName()))
    print(string.format(SubModeName.."Print:  raw=%s\n", ShowSpecialUI:GetDisplayName():ToString()))
    print(string.format(
        SubModeName.."[DisplayName=%s]"..
        " UIPageID=%d; DialogueID=%d; NameID=%d; ChapterID=%d; MediaId=%d"..
        "\n",
        ShowSpecialUI:GetDisplayName():ToString(),
        ShowSpecialUI.UIPageID, ShowSpecialUI.DialogueID, ShowSpecialUI.NameID,
        ShowSpecialUI.ChapterID, ShowSpecialUI.MediaId))
end

-- 打印出所有父级组件
function WkUtils.PrintAllParents(CurWidget)
    if CurWidget == nil or (not CurWidget:IsValid()) then
        return
    end

    local FullName = CurWidget:GetFullName()
    -- local SuperClassName = CurWidget:GetClass():GetFName():ToString()
    -- local ClassName = CurWidget:GetFName():ToString()
    print(string.format("\t%s\n", FullName))

    WkUtils.PrintAllParents(CurWidget:GetParent())
end

return WkUtils
