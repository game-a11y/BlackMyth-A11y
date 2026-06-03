namespace WkAccess.B1;

/// <summary>
/// 游戏枚举 → 中文显示名 集中翻译。
/// 所有面向用户的枚举文本输出统一经此类，便于后期国际化。
/// </summary>
internal static class EnumLocale
{
    /// <summary>GameScene → 中文场景名</summary>
    internal static string Scene(GameState.GameScene scene) => scene switch
    {
        GameState.GameScene.Startup         => "启动画面",
        GameState.GameScene.LogIn           => "登录界面",
        GameState.GameScene.MainMenu        => "主菜单",
        GameState.GameScene.Loading         => "加载中",
        GameState.GameScene.InGame          => "游戏中",
        GameState.GameScene.PauseMenu       => "暂停菜单",
        GameState.GameScene.ShaderCompiling => "着色器编译中",
        _                                   => $"UnknownScene({(int)scene}):{scene}",
    };

    /// <summary>EnPageID → 中文页面名（常用页面，逐步补全）</summary>
    internal static string Page(EnPageID page) => page switch
    {
        // 主菜单/开始
        EnPageID.StartGame       => "开始游戏",
        EnPageID.Archives         => "存档",
        EnPageID.SelectChapter    => "选择章节",
        EnPageID.NewGamePlusGuide => "二周目引导",
        // 设置
        EnPageID.Setting          => "设置",
        EnPageID.InitSetting      => "初始设置",
        // 游戏内 HUD
        EnPageID.BattleMainCon    => "战斗主界面",
        EnPageID.BloodBarList     => "血条列表",
        EnPageID.Interact         => "交互提示",
        EnPageID.TPSReticle       => "瞄准准星",
        // 功能页面
        EnPageID.ShrineMain       => "土地庙",
        EnPageID.RoleMain         => "角色",
        EnPageID.EquipMain        => "装备",
        EnPageID.BagMain          => "背包",
        EnPageID.TalentMain       => "天赋",
        EnPageID.LearnTalent      => "学习天赋",
        EnPageID.TravelNotesMain  => "游记",
        EnPageID.Map              => "地图",
        EnPageID.Shop             => "商店",
        EnPageID.EquipShop        => "装备商店",
        EnPageID.Alchemy          => "炼丹",
        EnPageID.SoakingMain      => "泡酒",
        EnPageID.Farm             => "种植",
        EnPageID.CollectionMain   => "图鉴",
        // 弹窗/提示
        EnPageID.Confirm          => "确认",
        EnPageID.Death            => "死亡",
        EnPageID.FallDying        => "濒死",
        EnPageID.NpcInteract      => "NPC对话",
        EnPageID.Award            => "奖励",
        EnPageID.MapTips          => "地图提示",
        EnPageID.CommTips         => "通用提示",
        EnPageID.SaveTips         => "保存提示",
        EnPageID.AgeTips          => "年龄提示",
        // 暂停/系统
        EnPageID.Login            => "登录",
        EnPageID.LoginNotice      => "登录公告",
        EnPageID.ShaderCompiling  => "着色器编译",
        // 回退
        _                         => $"UnknownPage({(int)page}):{page}",
    };
}
