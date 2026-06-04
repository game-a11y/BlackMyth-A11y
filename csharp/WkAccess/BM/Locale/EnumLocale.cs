namespace WkAccess.BM;

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

    /// <summary>EnPageID → 中文页面名</summary>
    internal static string Page(EnPageID page) => page switch
    {
        // ═══════════ 系统/内部 ═══════════
        EnPageID.MIN                   => page.ToString(),
        EnPageID.MAX                   => page.ToString(),
        EnPageID.Managed               => page.ToString(),
        EnPageID.StartGameManaged      => page.ToString(),

        // ═══════════ 游戏内 HUD ═══════════
        EnPageID.BattleMainCon         => "战斗主界面",
        EnPageID.BloodBarList          => "血条列表",
        EnPageID.Interact              => "交互提示",
        EnPageID.TPSReticle            => "瞄准准星",
        EnPageID.Story                 => "剧情",
        EnPageID.TeamInfo              => "队伍信息",
        EnPageID.Name                  => page.ToString(),
        EnPageID.FallDying             => "濒死",
        EnPageID.Death                 => "死亡",
        EnPageID.BlackOut              => "黑屏过渡",
        EnPageID.MDropMain             => "掉落物",
        EnPageID.DropSpecial           => "特殊掉落",
        EnPageID.AnimShowBlock         => "动画遮挡",

        // ═══════════ 主菜单/开始 ═══════════
        EnPageID.StartGame             => "开始游戏",
        EnPageID.Archives              => "存档",
        EnPageID.SelectChapter         => "选择章节",
        EnPageID.NewGamePlusGuide      => "二周目引导",
        EnPageID.Login                 => "登录",
        EnPageID.LoginNotice           => "登录公告",
        EnPageID.AgreementPolicy       => "用户协议",
        EnPageID.AgeTips               => "年龄提示",
        EnPageID.Background            => page.ToString(),
        EnPageID.SeqLogo               => "序章动画",

        // ═══════════ 设置 ═══════════
        EnPageID.Setting               => "设置",
        EnPageID.InitSetting           => "初始设置",
        EnPageID.SettingInputKeyBoard  => "按键设置",
        EnPageID.SettingScreenDetail   => "画面设置",
        EnPageID.BrightnessSetting     => "亮度设置",
        EnPageID.HDRSetting            => "HDR设置",

        // ═══════════ 功能页面 ═══════════
        EnPageID.ShrineMain            => "土地庙",
        EnPageID.RoleMain              => "角色",
        EnPageID.EquipMain             => "披挂(装备)",
        EnPageID.BagMain               => "行囊(背包)",
        EnPageID.TalentMain            => "天赋",
        EnPageID.LearnTalent           => "学习天赋",
        EnPageID.LearnLegacyTalent     => "根器",
        EnPageID.TravelNotesMain       => "游记",
        EnPageID.Map                   => "地图",
        EnPageID.DebugMap              => "调试地图",
        EnPageID.Shop                  => "商店",
        EnPageID.EquipShop             => "装备商店",
        EnPageID.Alchemy               => "炼丹",
        EnPageID.SoakingMain           => "泡制",
        EnPageID.Farm                  => "种植",
        EnPageID.CollectionMain        => "图鉴",
        EnPageID.WeaponBuild           => "兵器铸造",
        EnPageID.RoleWeaponBuild       => "角色兵器铸造",
        EnPageID.EquipBuild            => "装备铸造",
        EnPageID.HuluStrength          => "葫芦强化",
        EnPageID.WineStrength          => "酒强化",
        EnPageID.SoulSkillCollect      => "精魂收集",
        EnPageID.SoulSkillStrength     => "精魂强化",
        EnPageID.MedicineRecipe        => "炼丹",
        EnPageID.PastMemory            => "过往记忆",
        EnPageID.SoundtrackV2          => "原声音乐",
        EnPageID.MusicExport           => "音乐导出",
        EnPageID.RoleMeditationPointMain => "打坐",

        // ═══════════ 弹窗/提示 ═══════════
        EnPageID.Confirm               => "确认",
        EnPageID.ConfirmThree          => "三选一确认",
        EnPageID.CommTips              => "通用提示",
        EnPageID.CommTipsBlock         => "通用提示(阻塞)",
        EnPageID.SimpleTips            => "简单提示",
        EnPageID.SaveTips              => "保存提示",
        EnPageID.MapTips               => "地图提示",
        EnPageID.Award                 => "奖励",
        EnPageID.ChapterAward          => "章节奖励",
        EnPageID.AchieveTips           => "成就提示",
        EnPageID.EditionAward          => "版本奖励",
        EnPageID.NpcInteract           => "NPC对话",
        EnPageID.Defeated              => "战败",
        EnPageID.CommSkip              => "跳过",

        // ═══════════ 加载/过渡 ═══════════
        EnPageID.ShaderCompiling       => "着色器编译",
        EnPageID.ShaderCompilingConfirm => "着色器编译确认",
        EnPageID.LoadingAdaptor        => "加载",
        EnPageID.PlayGoProgressRaw     => "PlayGo进度",
        EnPageID.PlayGoProgressNormal  => "PlayGo进度",
        EnPageID.TransGuide            => "传送引导",

        // ═══════════ 引导/教程 ═══════════
        EnPageID.GuideNormal           => "普通引导",
        EnPageID.GuideQuest            => "任务引导",
        EnPageID.GuidePopbox           => "弹窗引导",
        EnPageID.GuideMain             => "引导主界面",

        // ═══════════ Boss Rush / 挑战 ═══════════
        EnPageID.BossReChallengeMain   => "Boss复战",
        EnPageID.BossIterationsMain    => "Boss连战",
        EnPageID.BossRushSettlement    => "Boss战结算",
        EnPageID.BossRushTime          => "Boss战计时",
        EnPageID.BossIterationsAward   => "Boss连战奖励",
        EnPageID.BossRushStartFight    => "Boss战开战",

        // ═══════════ 过场/多媒体 ═══════════
        EnPageID.SeqMediaPlayer        => "过场动画",
        EnPageID.ChapterMovie          => "章节影片",
        EnPageID.EndCredits            => "片尾字幕",
        EnPageID.TakePhoto             => "拍照",
        EnPageID.QTEInteract           => "QTE交互",

        // ═══════════ 调试/开发（保留英文或标记为调试） ═══════════
        EnPageID.MiniGM                => "GM菜单",
        EnPageID.GMConfirm             => "GM确认",
        EnPageID.GMCommTips            => "GM提示",
        EnPageID.GMDisplayUIText       => "GM文本显示",
        EnPageID.ReportBugPanel        => "Bug报告",
        EnPageID.UITexConfigCheck      => "UI贴图检查",
        EnPageID.BenchMark             => "性能测试",
        EnPageID.DebugInfo             => "调试信息",
        EnPageID.DebugBlack            => "调试黑屏",
        EnPageID.HexRoomTest           => page.ToString(),
        EnPageID.GuiqiangTest          => page.ToString(),
        EnPageID.HatumTestUI           => page.ToString(),
        EnPageID.EllenTest             => page.ToString(),

        _                              => $"UnknownPage({(int)page}):{page}",
    };
}
