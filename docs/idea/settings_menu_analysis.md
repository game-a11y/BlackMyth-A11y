# 设置菜单 UI 结构与数据源分析

## 概述

本文档分析《黑神话：悟空》设置菜单的 UI 控件树结构、说明文字的存储位置、数据的原始来源，以及运行时获取方法。作为无障碍朗读设置菜单的开发计划指引。

---

## 1. 设置菜单主面板

**主面板**：`BUI_Setting`（WidgetBlueprint 类名 `BUI_Setting_C`，继承自 `BUI_DependWidget`）

pak-json 路径：
```
Content/00Main/UI/BluePrintsV3/Setting/BUI_Setting.json
```

Widget 树结构：
```
BUI_Setting
├── BI_InputRoot                    (输入根容器)
├── BI_ScrollBox                    (设置列表滚动区域)
├── BI_Setting_headline_0          (右侧详情标题区 — 对应第0组设置)
├── BI_Setting_headline_1          (右侧详情标题区 — 对应第1组设置)
├── BI_Setting_headline_2
├── BI_Setting_headline_3
├── BI_Setting_headline_4
├── BI_SettingFixedItem_0          (多选按钮项)
├── BI_SettingFixedItem_1
├── BI_SettingFixedItem_10
├── BI_SettingFixedItem_11
├── BI_SettingFixedItem_12
└── ... (更多设置项)
```

**关键属性**：每个 `BI_SettingFixedItem` 的 `ImgRight` 属性引用对应的 `BI_SettingDetail_headline`，构成左侧按钮 ↔ 右侧说明的关联。

---

## 2. 聚焦按钮时，右侧说明信息的存储位置

### 2.1 `BI_SettingFixedItem`（多选按钮，如语言/字幕开关）

```
RootCon
└── BtnCon
    ├── BI_Btn (BI_SettingMainBtn_C)     ← 聚焦的按钮
    │   └── TxtName                       ← **选项名称文本**（左侧主文字）
    ├── FocusWidget (GSFocusWidget)       ← 真正接收焦点的控件
    └── DescCon (HorizontalBox)           ← **右侧说明区域**
        ├── ImgLeft                       (左箭头装饰)
        ├── CanvasPanel_165
        │   └── ScaleBox_58
        │       └── TxtDesc (TextBlock)   ← **当前选中值的说明文本**
        └── ImgRight                      (右箭头装饰)
```

- **左侧按钮名**：`BI_Btn → TxtName`
- **右侧说明/当前值**：`DescCon → TxtDesc`
- **右侧详情标题**：与 `BI_SettingFixedItem` 配对的 `BI_SettingDetail_headline → TxtName`

当前无障碍提取器（`B1WidgetExtractors.cs:52-58`）：
```csharp
// BI_SettingFixedItem 提取
var label = FindTextByName(biBtn, "TxtName");   // 按钮名
var value = FindTextByName(w, "TxtDesc");        // 右侧当前值
```

### 2.2 `BI_SettingDetail_headline`（右侧详情标题栏）

```
RootCon
└── BtnCon
    ├── TxtName (GSScaleText)       ← **右侧详情标题文本**
    ├── FocusWidget (GSFocusWidget)
    └── Image_42                    (分隔线)
```

继承自 `BUI_Button`，可接收焦点。

### 2.3 `BI_SettingMenuItem`（下拉菜单项）

```
CanvasPanel_0
├── BtnCon
│   └── BI_Btn (BI_SettingMenuBtn_C)   ← 聚焦的按钮
│       ├── BI_Btn (BI_SettingMainBtn_C)
│       │   └── TxtName                 ← 选项名称
│       └── MenuCon
│           ├── TxtDesc                 ← **当前选中值**
│           └── ImgArrow                (下拉箭头)
└── ImgMark                            (高亮标记)
```

2层嵌套：`BI_SettingMenuItem → BI_Btn → BI_Btn → TxtName`

### 2.4 其他设置项类型

| Widget 类型 | 用途 | 说明文本位置 |
|---|---|---|
| `BI_SettingKeyItem` | 按键配置 | `BI_Btn → TxtName`（名称）|
| `BI_SettingSliderItem` | 拖动条 | `BI_Btn → TxtName`（名称）+ `BI_Slider → TxtNum`（当前值）|
| `BI_SettingIconItem` | 图标按钮 | `BI_Btn → TxtName`（名称）|
| `BI_ModeBtnItem` | 下拉选项（展开后的子项） | `TxtName` |

---

## 3. 数据的原始来源

数据链路：**Protobuf 表 → 托管运行时类型 → C# 数据绑定层 → UI 控件**

### 3.1 Protobuf 数据表（最底层）

**`FUStSettingDetailDesc`**（`GSE.ProtobufDB/BtlB1/FUStSettingDetailDesc.cs`）：

| 字段 | 类型 | 含义 |
|------|------|------|
| `ID` | int | 设置项 ID |
| `SortOrder` | int | 排序 |
| `ClassID` | int | 所属分类 ID |
| `OPType` | ESettingOPType | 操作类型 |
| `GName` | string | **本地化 Key 字符串** |
| `ValueDesc` | RepeatedField\<string\> | 可选值描述列表 |

**`FUStSettingClassNameDesc`**（`GSE.ProtobufDB/BtlB1/FUStSettingClassNameDesc.cs`）：

| 字段 | 类型 | 含义 |
|------|------|------|
| `ID` | int | 分类 ID |
| `GName` | string | **分类名本地化 Key** |

### 3.2 运行时托管数据（中间层）

**`UISettingConfigDesc`**（`ResB1` 命名空间，通过 `GameDBRuntime` 访问）：

| 字段 | 类型 | 含义 |
|------|------|------|
| `Id` | int | 配置 ID |
| `ConfigName` | FText | **配置名称（已本地化）** |
| `ConfigDesc` | FText | **配置详细描述（已本地化）** |
| `ConfigTab` | EUIConfigTab | 所属标签页 |
| `ConfigType` | UIConfigType | UI 控件类型 |
| `InnerTabId` | int | 内页标签 ID |
| `DefaultValue` | int | 默认值 |
| `Params` | RepeatedField\<int\> | 模板参数 |
| `TempelteParam` | string | 模板参数字符串 |
| `ShowType` | int | 显示条件标志 |
| `DetailDisplayType` | UISettingDetailDisplayType | 详情显示类型 |
| `SettingLockInfoEx` | RepeatedField | 锁定条件信息 |
| `LocalizationTag` | FText | 本地化标签 |

### 3.3 C# 数据绑定层（上层）

- **`DSConfigData`** — 抽象基类，封装设置项通用状态
  - `ConfigName`（GSUIBiProp\<string\>）：配置名称，UI 通过双向绑定读取
  - `ConfigDisableState`：禁用状态
  - `ConfigIsModified`：是否已修改
  - `ConfigNoRecommend`：是否不推荐
  - `ConfigReStart`：是否需要重启
  - `DetailDisplayType`：详情展示方式

- **`DSConfigFixedItemData`** — 多选按钮
  - `FixedItemList`（GSUIBiList\<ConfigFixedItem\>）：可选值列表
  - `CurSelectedItem`（GSUIBiProp\<ConfigFixedItem\>）：当前选中项

- **`DSConfigMenuItemData`** — 下拉菜单
  - 同上结构 + `MenuItemType`

- **`DSConfigHeadlineData`** — 纯标题，无数据绑定

- **`ConfigFixedItem`** — 单个选项：
  ```csharp
  public class ConfigFixedItem {
      public int ConfigId;        // 选项 ID
      public string ConfigName;   // 选项名称（已本地化）
  }
  ```

### 3.4 数据填充流程

```
Protobuf 表加载
  → GameDBRuntime.GetTBUISettingConfigDesc().List
    → 遍历生成 DSConfigData 子类实例
      → GSUIBiProp/GSUIBiList 双向绑定
        → UI Widget (BI_SettingFixedItem / BI_SettingMenuItem 等)
          → TxtName 显示 ConfigName
          → TxtDesc 显示 CurSelectedItem.ConfigName
```

参考 `DSInitSetting.cs:52-76`：
```csharp
UISettingConfigDesc uISettingConfigDesc = GameDBRuntime.GetUISettingConfigDesc(ConfigType);
int id = uISettingConfigDesc.Id;
string configName = uISettingConfigDesc.ConfigName;
int defaultValue = GetDefaultValue(ConfigType, uISettingConfigDesc.DefaultValue);
DSConfigFixedItemData result = new DSConfigFixedItemData(
    ID: id, InConfigName: configName, InnerTabID: innerTabId,
    InConfigItemList: value, InDefaultConfigId: defaultValue,
    InSettingType: ConfigType, DelFunc: this.FixedDelFunc);
```

### 3.5 本地化文本存储

`Content/Localization/Game/zh-Hans-CN/Game.json` 中 `StringKVMapDesc` 包含实际翻译：

```json
"UISettingConfigDesc.{id}.ConfigName": "语言",
"UISettingConfigDesc.{id}.ConfigDesc": "选择游戏显示语言"
```

FText 解析链路：`ConfigName (FText key)` → `ToFText().ToString()` → 当前语言文本

---

## 4. 运行时获取方法

### 4.1 从 UI 控件树获取（当前无障碍 Mod 方式）

```csharp
// 已知当前聚焦的 widget（如 BI_SettingFixedItem_C 实例）
var widget = ...;

// 获取按钮名
var biBtn = GSUIUtil.FindChildWidget(widget, "BI_Btn") as UUserWidget;
var label = B1WidgetResolvers.FindTextByName(biBtn, "TxtName");

// 获取右侧当前值说明
var value = B1WidgetResolvers.FindTextByName(widget, "TxtDesc");

// 组合朗读
// e.g. "多选按钮 - 语言 - 简体中文"
```

### 4.2 从游戏数据库获取（推荐用于结构化查询）

```csharp
using b1;
using b1.Localization;

// 获取全部设置配置
var allConfigs = GameDBRuntime.GetTBUISettingConfigDesc().List;

// 按类型精确查找
var specificDesc = GameDBRuntime.GetUISettingConfigDesc(
    UISettingConfigType.Localization);
```

### 4.3 获取完整的纯文本显示内容

参考 `DisplayUITextContentHelper_Setting.cs:9-17`：
```csharp
foreach (UISettingConfigDesc item in GameDBRuntime.GetTBUISettingConfigDesc().List)
{
    string displayContent = item.ConfigName.ToFTextRemoveRichDynamic().ToString()
        + item.ConfigDesc.ToFTextRemoveRichDynamic().ToString();
    // ConfigName: 如"语言"
    // ConfigDesc: 如"选择游戏显示语言"
}
```

---

## 5. 已知按钮名获取对应说明信息

### 核心方法

按钮上的 `TxtName` 文本来源于 `UISettingConfigDesc.ConfigName`（已通过 UE4 FText 系统本地化）。可以反向匹配：

```csharp
using b1;
using b1.Localization;

/// <summary>
/// 通过当前语言的按钮名查找对应的设置描述
/// </summary>
public static string? GetSettingDescByButtonName(string buttonNameText)
{
    foreach (var desc in GameDBRuntime.GetTBUISettingConfigDesc().List)
    {
        string configName = desc.ConfigName
            .ToFTextRemoveRichDynamic()
            .ToString();

        if (configName == buttonNameText)
        {
            return desc.ConfigDesc
                .ToFTextRemoveRichDynamic()
                .ToString();
        }
    }
    return null;
}

/// <summary>
/// 通过设置类型获取当前语言的名称和描述
/// </summary>
public static (string Name, string Desc) GetSettingInfo(
    UISettingConfigType configType)
{
    var desc = GameDBRuntime.GetUISettingConfigDesc(configType);
    return (
        desc.ConfigName.ToFText().ToString(),
        desc.ConfigDesc.ToFText().ToString()
    );
}
```

### 关键 API 速查

| API | 用途 |
|-----|------|
| `GameDBRuntime.GetTBUISettingConfigDesc().List` | 获取全部 `UISettingConfigDesc` |
| `GameDBRuntime.GetUISettingConfigDesc(type)` | 按类型获取单条 |
| `.ConfigName.ToFText().ToString()` | 解析为当前语言文本 |
| `.ConfigName.ToFTextRemoveRichDynamic().ToString()` | 去除富文本标签后获取纯文本 |
| `GSUIUtil.FindChildWidget(root, name)` | 在控件树中按名称查找子控件 |
| `B1WidgetResolvers.FindTextByName(root, name)` | 按名称查找文本控件的文本内容 |
| `GSLocalization.GetCurrentCulture()` | 获取当前语言文化设置 |

### 从聚焦控件出发的完整路径

```csharp
// 已知当前聚焦的 BI_SettingFixedItem widget
UUserWidget widget = ...;

// 第1步：读取按钮名（当前已本地化）
var biBtn = GSUIUtil.FindChildWidget(widget, "BI_Btn") as UUserWidget;
string? buttonName = B1WidgetResolvers.FindTextByName(biBtn, "TxtName");

// 第2步：读取右侧当前选中值的说明
string? currentValue = B1WidgetResolvers.FindTextByName(widget, "TxtDesc");

// 第3步（可选）：通过数据库获取更详细的 ConfigDesc
string? detailDesc = GetSettingDescByButtonName(buttonName);

// 合成朗读文本
// e.g. "语言 - 简体中文 - 选择游戏显示语言"
string speakText = $"{buttonName} - {currentValue}";
if (detailDesc != null && detailDesc != currentValue)
    speakText += $" - {detailDesc}";
```

---

## 6. Widget 类型与提取器对应总表

| Widget 类名 | 控件类型 | 注册的提取器 | 朗读模板 |
|---|---|---|---|
| `BI_SettingTab_C` | 标签页 | `Extract_SettingTab` | `标签页 - {TxtName}` |
| `BI_SettingFixedItem_C` | 多选按钮 | `Extract_SettingFixedItem` | `多选按钮 - {TxtName} - {TxtDesc}` |
| `BI_SettingMenuItem_C` | 下拉框 | `Extract_SettingMenuItem` | `下拉框 - {TxtName} - {TxtDesc}` |
| `BI_SettingSliderItem_C` | 拖动条 | `Extract_SettingSliderItem` | `拖动条 - {TxtName} - {TxtNum}` |
| `BI_SettingIconItem_C` | 图标按钮 | `Extract_SettingIconItem` | `图标按钮 - {TxtName}` |
| `BI_SettingMainBtn_C` | 文本按钮 | `Extract_SettingMainBtn` | `文本按钮 - {TxtName}` |
| `BI_SettingMenuBtn_C` | 菜单按钮 | `Extract_SettingMenuBtn` | 嵌套 `BI_SettingMainBtn` |
| `BI_SettingKeyItem_C` | 按键配置 | `Extract_SettingKeyItem` | `按键配置 - {TxtName} [??]` |
| `BI_ModeBtnItem_C` | 下拉子项 | `Extract_ModeBtnItem` | `下拉项 - {Text}` |

---

## 7. 关键文件索引

| 类别 | 路径 |
|------|------|
| 设置主面板 Widget JSON | `docs/pak-json/Content/00Main/UI/BluePrintsV3/Setting/BUI_Setting.json` |
| 多选按钮 JSON | `docs/pak-json/Content/00Main/UI/BluePrintsV3/Setting/Item/BI_SettingFixedItem.json` |
| 详情标题 JSON | `docs/pak-json/Content/00Main/UI/BluePrintsV3/Setting/Item/BI_SettingDetail_headline.json` |
| 下拉菜单项 JSON | `docs/pak-json/Content/00Main/UI/BluePrintsV3/Setting/BI_SettingMenuItem.json` |
| 设置页面模板 JSON | `docs/pak-json/Content/00Main/UI/BluePrintsV3/Setting/BI_SettingMenuPage.json` |
| Protobuf: 设置详情 | `docs/b1GameDLL-src/GSE.ProtobufDB/BtlB1/FUStSettingDetailDesc.cs` |
| Protobuf: 类名描述 | `docs/b1GameDLL-src/GSE.ProtobufDB/BtlB1/FUStSettingClassNameDesc.cs` |
| C#: 设置数据基类 | `docs/b1GameDLL-src/B1UI_GSE.Script/B1UI.GSUI/DSConfigData.cs` |
| C#: 多选按钮数据 | `docs/b1GameDLL-src/B1UI_GSE.Script/B1UI.GSUI/DSConfigFixedItemData.cs` |
| C#: 选项条目 | `docs/b1GameDLL-src/B1UI_GSE.Script/B1UI.GSUI/ConfigFixedItem.cs` |
| C#: 初始设置生成 | `docs/b1GameDLL-src/B1UI_GSE.Script/B1UI.GSUI/DSInitSetting.cs` |
| C#: 显示文本助手 | `docs/b1GameDLL-src/B1UI_GSE.Script/B1UI.GSUI/DisplayUITextContentHelper_Setting.cs` |
| C#: 游戏数据库访问 | `docs/b1GameDLL-src/BtlSvr.Main/b1/GameDBResB1.cs` |
| 本地化文本 | `docs/pak-json/Content/Localization/Game/zh-Hans-CN/Game.json` |
| 无障碍: Widget 提取器 | `csharp/WkAccess/B1/UI/B1WidgetExtractors.cs` |
| 无障碍: Widget 解析器 | `csharp/WkAccess/B1/UI/B1WidgetResolvers.cs` |
| 无障碍: 焦点追踪 | `csharp/WkAccess/A11yMod/UIFocusTracker.cs` |
| 无障碍: 文本提供器 | `csharp/WkAccess/A11y/UI/UIScreenTextProvider.cs` |
