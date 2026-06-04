# HUD 系统源码分析

> 基于 `docs/b1GameDLL-src/` 游戏源码分析，为游戏时信息输出做准备。

## 一、基类层级

```
GSDObject                    # 最底层基类
 └─ GSUIView                 # GSE.GSUI/GSUIView.cs — 所有 View 基类
     └─ GSUIPage             # GSE.GSUI/GSUIPage.cs — 所有 Page 基类  ★ 缓存目标
         ├─ UIBattleMainCon  # 战斗 HUD 容器 (PageID=2)
         ├─ UIBloodBarList   # 敌人血条列表  (PageID=5)
         ├─ UIInteract       # 交互提示      (PageID=6)
         ├─ UIMapTips        # 地点发现提示  (PageID=32)
         ├─ UICommTips       # 通用通知弹窗  (PageID=39/40/98)
         ├─ UIAward          # 获取物品弹窗  (PageID=42)
         ├─ UISimpleTips     # 物品详情浮窗  (PageID=50)
         ├─ UIAchieveTips    # 成就解锁弹窗  (PageID=69)
         └─ UIGuideBase      # 引导/任务追踪基类
             ├─ UIGuideNormal
             ├─ UIGuideQuest
             └─ UIGuideMain
```

**缓存方式：** `GSG.UIMgr.FindUIPage(pageId)` — 内部是 `ArrayPage[pageId]` 直接数组索引，返回 `GSUIPage?`。

BUI_Widget 控件树：
```
BUI_Widget
 ├─ BI_PlayerBarCS      # 玩家状态栏 (HP/MP/Stamina/Gourd)
 │   ├─ BI_HpProgBarCS  #   生命条（含异常状态 VFX）
 │   ├─ BI_ProgBarCS    #   法力/气力条（通用进度条）
 │   └─ BI_GourdCS      #   葫芦槽位
 ├─ BI_StickLevelCS     # 棍势/棍法指示器
 ├─ BI_TreasureCS       # 法宝槽位
 ├─ BI_PlayerStateBoxCS # 玩家状态图标
 ├─ BUI_InputTipsOne    # 单个按键提示
 ├─ BUI_BossBar         # Boss 血条 (extends BUI_EnemyBloodBarPure)
 └─ BUI_MInteractIcon   # 交互图标
```

## 二、UIBattleMainCon — 战斗 HUD 容器 (PageID=2)

**文件：** `B1UI_GSE.Script/B1UI.GSUI/UIBattleMainCon.cs`
**蓝图：** `BUI_BattleMain`

### 内部子面板（按 EnMainAreaType 管理）

| 区域 | 字段 | 类型 | 说明 |
|------|------|------|------|
| PlayerBar | `PlayerStCon` | `VIMPlayerBar`→`BI_PlayerBarCS` | HP/MP/气力/葫芦 |
| Style | `StyleCon` | `BI_StickLevelCS` | 棍法架势 |
| SoulSkill | `SoulSkillCon` | — | 精魂/化身技能 |
| ShortcutItem | `ShortcutItemCon` | `VIMShortcutItem` | 快捷物品栏 |
| ShortcutSpell | `ShortcutSpellCon` | `VIMShortcutSpell` | 快捷法术栏 |
| Treasure | `TreasureCon` | `BI_TreasureCS` | 法宝 |
| Trans | `TransCon` | — | 变身状态 |
| Abnormal | `AbnormalAccCon` | — | 异常累积指示 |
| RideMount | `RideMountCon` | — | 坐骑/云骞环 |

### 关键方法

| 方法 | 用途 |
|------|------|
| `OnUIPageConstructImpl()` | 初始化绑定、子控件、事件监听 |
| `OnUIPageTickImpl(float)` | 每帧：TickIsInBattle, TickUpdateUIShowState, TickStickLevel, TickCheckShow |
| `OnBindFloatAttrsOnSetIdxValue(int AttrId, ...)` | 监听属性变化 (HP/MP/Stamina)，触发 HUD 显示 |

### 显隐状态机

`DSBattleMain.ShowState` (`GSUIBiProp<EnMainShowState>`) — INIT/HIDE/SHOW/SHOWAREA。交互后有 6 秒保护时间，之后自动隐藏。

## 三、属性系统

### EBGUAttrFloat 关键枚举

**文件：** `GSE.ProtobufDB/BtlShare/EBGUAttrFloat.cs`（~100+ 属性值）

| 属性 | ID | 说明 |
|------|-----|------|
| HpMax | 1 | 生命上限（最终值） |
| MpMax | 2 | 法力上限 |
| StaminaMax | 8 | 气力上限 |
| StaminaDepletedLimit | 7 | 气力耗尽阈值 |
| TransEnergyMax | 11 | 神通力上限（变身能量） |
| PevalueMax | 39 | 棍势上限 |
| Hp | 151 | 当前生命 |
| Mp | 152 | 当前法力 |
| Stamina | 158 | 当前气力 |
| StaminaRecover | 159 | 气力回复速度 |
| CurEnergy | 188 | 当前神通力 |
| Shield | 190 | 护盾值 |
| Pevalue | 191 | 当前棍势 |
| FabaoEnergy | 201 | 法宝能量 |
| VigorEnergy | 202 | 精魂能量 |
| Atk | 153 | 攻击力 |
| Def | 154 | 防御力 |
| CritRate | 161 | 暴击率 |
| CritMultiplier | 162 | 暴击伤害 |

### 属性读取

```csharp
// 方式 A — 一次性查询（已有）
var hp = BGUFunctionLibraryCS.GetAttrValue(player, EBGUAttrFloat.Hp);
// 内部：BUC_AttrContainer.GetFloatValue(AttrID) → FloatAttrs[(int)AttrID]

// 方式 B — 通过 ECS 容器直接读
var container = BGU_DataUtil.GetReadOnlyData<BUC_AttrContainer>(pawn);
var hp = container.GetFloatValue(EBGUAttrFloat.Hp);
```

### 属性变更监听

```csharp
// 订阅 FloatAttrs.OnSetIdxValue（GSBindList<float>，长度 255，按 AttrID 索引）
var container = BGU_DataUtil.GetReadOnlyData<BUC_AttrContainer>(pawn);
container.BindOneValueChanged((int attrId, float oldValue, float newValue) => {
    // attrId == 151 → HP 变化
    // attrId == 152 → MP 变化
    // attrId == 158 → Stamina 变化
});
```

### 获取玩家 Pawn

```csharp
// 已有封装：WkUtils.GetControlledPawn()
var world = GCHelper.FindRef(FGlobals.GWorld);
var pawn = UGSE_EngineFuncLib.GetFirstLocalPlayerController(world).GetControlledPawn();
```

## 四、弹窗 Tip 体系

| PageID | 类 | 用途 | 数据源 |
|--------|-----|------|--------|
| 6 | `UIInteract` | 交互按键提示 | `DSInteract`（管理 `BUI_MInteractIcon` 字典，按 AActor 索引） |
| 32 | `UIMapTips` | 地点发现/土地庙名称 | `DSMapTips.RebirthPointId` / `UIWordId`；`TxtMainName`=区域名，`TxtSubName`=土地庙名 |
| 39 | `UICommTips` | 通用通知/警告/重置 | `DSCommTips.TipsData`（`DSTipsData`） |
| 42 | `UIAward` | 获得物品弹出 | `DSAward.AwardList`（`ItemOne` 序列） |
| 50 | `UISimpleTips` | 物品/装备详情浮窗 | `DSTips.FocusWidget`（跟随聚焦控件定位） |
| 69 | `UIAchieveTips` | 成就解锁 | `DSAchieveTips.AchievementID` |
| 65 | `UIGuideNormal` | 普通引导项 | `DSGuide.AUpdateNodeGuideState` |
| 66 | `UIGuideQuest` | 任务追踪 | `DSGuide.AUpdateNodeGuideState` |
| 77 | `UIGuideMain` | 引导主面板 | `DSGuide` |

## 五、进度条控件 (BI_ProgBarCS)

**文件：** `BtlSvr.Main/b1.UI/`

```
BI_ProgBarCS (extends BUI_Widget)   # 通用进度条
 ├─ BI_HpProgBarCS                  # HP 条（+异常状态 VFX、护盾条）
 ├─ BI_ProgBarCS                    # MP/Stamina 条
```

### 数据存储链

```
DSProgBarInfo        → IsShow, CanShow, MaxValue, Value, Percent, BarData
 └─ DSAttrProgBar    → +AttrContainer 绑定
     └─ DSHpProgBar  → +ShieldData, AbnormalStateData, UnitBarInfoData
```

### 更新模式

1. **数据驱动：** `DSProgBarInfo.Value` / `Percent` 变更 → `OnValueChanged` → `SetProgress()`
2. **属性绑定：** `DSAttrProgBar` 自动绑定 `BUC_AttrContainer.FloatAttrs.OnSetIdxValue`
3. **Tick 动画：** `OnUIGSInnerTickImpl()` → 缩放动画计时

## 六、Boss 血条 (BUI_BossBar)

**文件：** `BtlSvr.Main/b1.UI.Comm/BUI_BossBar.cs`

- 继承 `BUI_EnemyBloodBarPure`（抽象类，extends `BUI_Widget`）
- 初始化读取 HpMax/Hp 并 `HPBar.InitSetCurAndMaxValue()`
- 监听 `BUC_AttrContainer.BindOneValueChanged` → AttrId==151 → `OnHPChanged()` → `HPBar.ValueDecrease()`
- `DoShowIn()`/`DoShowOut()` 播放显隐动画

## 七、缓存的实现方式参考

**已有实现：** `B1PageCache.cs`

通过 Harmony Patch `BUI_Widget.Construct_Implementation`(Postfix) 缓存 6 个页面实例。

```csharp
// 扩展到 HUD 页面
[HarmonyPatch(typeof(BUI_Widget), "Construct_Implementation")]
static void Postfix(BUI_Widget __instance) {
    var className = __instance.GetClass().GetFName().ToString();
    // 匹配 BI_PlayerBarCS, BI_HpProgBarCS, BUI_BossBar ...
}
```

**备选方式：** 直接通过 Page 系统获取（无需 Harmony）：
```csharp
var hudPage = GSG.UIMgr.FindUIPage((int)EnPageID.BattleMainCon) as UIBattleMainCon;
```

## 八、输出策略总结

| 目标 | 推荐方式 |
|------|----------|
| HP/MP/气力 数值 | 轮询 `BGUFunctionLibraryCS.GetAttrValue(pawn, attr)` — 简单可靠 |
| HP/MP/气力 变化通知 | 订阅 `BUC_AttrContainer.BindOneValueChanged` — 实时零开销 |
| 交互提示文本 | 缓存 `UIInteract` 页面，读取 `BUI_MInteractIcon` 的文本控件 |
| 按键提示 | 读取 `GSUIPage.InputTipsPool` 中的 `BUI_InputTipsOne` |
| Boss 名称/HP | 缓存 `BUI_BossBar`，读取其关联的 `AActor` 名称 |
| 弹窗 Tip 文本 | 监听 `UICommTips`/`UIAward`/`UIMapTips` 页面的 `OnShow`/数据变更 |
| 棍势/棍法 | 读取 `BI_StickLevelCS`，判断当前架势 |
| 快捷物品/法术 | 遍历 `ShortcutItemCon`/`ShortcutSpellCon` 的子控件 |
