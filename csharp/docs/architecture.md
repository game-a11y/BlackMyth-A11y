# WkAccess 架构规范

## 命名空间

- `WkAccess` — 组合根，仅包含 `ModMain`、`BuildInfo`、`GlobalUsings`
- `WkAccess.A11y` — 跨游戏复用基础设施（日志、TTS、文本提取），不依赖 BM 或 A11yMod
- `WkAccess.A11y.UE` — UE 平台绑定（`WkUtils` 等），即使单文件也保留子命名空间作为平台占位
- `WkAccess.BM` — 黑神话系列数据层，仅提供数据查询，不调用 `A11yTolk.Speak()`
- `WkAccess.BM.UI` — 控件树相关（WidgetTreeDumper、Extractors、Resolvers 等），5+ 文件保留子命名空间
- `WkAccess.A11yMod` — 无障碍行为策略层，编排"何时朗读什么"
- `WkAccess.A11yMod.Patches` — 所有 Harmony Patch，按被 Hook 的游戏类文件组织
- 偏好扁平化：仅在文件数 ≥5 或语义边界明确时使用子命名空间

## 依赖方向

```
A11yMod ──→ BM ──→ A11y
  │                   │
  └───────────────────┘
(A11yMod 也直接依赖 A11y)
```

- `A11y` 不依赖 `BM` 或 `A11yMod`
- `BM` 依赖 `A11y`（仅日志 + 工具），不依赖 `A11yMod`
- `A11yMod` 依赖 `BM`（读数据）、`A11y`（输出）
- 禁止循环依赖

## BM 层规则

- BM 仅提供数据，不直接输出无障碍内容
- BM 内**允许**调用 `A11yLog`（日志记录）
- BM 内**禁止**调用 `A11yTolk.Speak()`（朗读输出属于 A11yMod）
- 需要对外通知时发布事件（如 `InteractMonitor.OnInteractTextChanged`），由 A11yMod 订阅

## BM 类命名前缀

- 通用类不加前缀（如 `WidgetTreeDumper`、`SceneMonitor`、`GameState`）
- 悟空特有类加 `B1` 前缀（如 `B1WidgetExtractors`、`B1InputTipsScanner`）
- B2 出现后提取通用部分到 BM 根，`B1*` → `BM.UI` 同理

## Harmony Patch 归属

- 所有 `[HarmonyPatch]` 类统一放在 `A11yMod/Patches/`
- 按被 Hook 的游戏类型分文件（如 `BUI_Button.cs`、`BUI_Widget.cs`）
- Patch 内的业务逻辑委托给 A11yMod 或 BM 的业务类
- 不再将 Patch 与业务逻辑类同文件混放，已在同文件的需搬家

## GlobalUsings

- 保留：BCL（`System*`）、框架（`CSharpModBase`、`HarmonyLib`）、游戏引擎（`b1*`、`UnrealEngine*`、`B1UI*`、`GSE*`等）
- **移除**所有内部项目引用（`WkAccess.A11y`、`WkAccess.BM` 等）
- 每个文件头部显式声明内部依赖

## 翻译数据

- 枚举 → 中文翻译统一收编到 `EnumLocale`
- 按键映射统一收编到 `KeyNameLocale`
- 不在业务代码中内联翻译字典
