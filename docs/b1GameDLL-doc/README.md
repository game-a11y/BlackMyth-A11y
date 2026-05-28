# b1GameDLL-doc — C# 游戏 DLL 类导航文档

本文档目录包含《黑神话：悟空》C# Mod 中 10 个核心 DLL 的类导航文档，方便快速查找类型定义。

> **基于版本**: v1.0.21.23831（源码位于 `docs/b1GameDLL-src/`）
> **编译产物**: `csharp/B1CSharpLoader/GameDll/`

## 路径说明

所有路径均相对于项目根目录（`/mnt/a/Game-A11y/BlackMyth-A11y/`）。

| # | DLL 程序集 | 源码路径 (GameDll-src) / 本文档参考 | 编译产物 (GameDll) | 文档 |
|---|-----------|--------------------------------------|-------------------|------|
| 1 | **b1.Managed** | `docs/b1GameDLL-src/b1.Managed/` | `csharp/B1CSharpLoader/GameDll/b1.Managed.dll` | [b1.Managed.md](b1.Managed.md) |
| 2 | **b1.Native** | `docs/b1GameDLL-src/b1.Native/` | `csharp/B1CSharpLoader/GameDll/b1.Native.dll` | [b1.Native.md](b1.Native.md) |
| 3 | **b1.NativePlugins** | `docs/b1GameDLL-src/b1.NativePlugins/` | `csharp/B1CSharpLoader/GameDll/b1.NativePlugins.dll` | [b1.NativePlugins.md](b1.NativePlugins.md) |
| 4 | **B1UI_GSE.Script** | `docs/b1GameDLL-src/B1UI_GSE.Script/` | `csharp/B1CSharpLoader/GameDll/B1UI_GSE.Script.dll` | [B1UI_GSE.Script.md](B1UI_GSE.Script.md) |
| 5 | **BtlSvr.Main** | `docs/b1GameDLL-src/BtlSvr.Main/` | `csharp/B1CSharpLoader/GameDll/BtlSvr.Main.dll` | [BtlSvr.Main.md](BtlSvr.Main.md) |
| 6 | **GSE.Core** | `docs/b1GameDLL-src/GSE.Core/` | `csharp/B1CSharpLoader/GameDll/GSE.Core.dll` | [GSE.Core.md](GSE.Core.md) |
| 7 | **GSE.GSNet** | `docs/b1GameDLL-src/GSE.GSNet/` | `csharp/B1CSharpLoader/GameDll/GSE.GSNet.dll` | [GSE.GSNet.md](GSE.GSNet.md) |
| 8 | **GSE.GSSdk** | `docs/b1GameDLL-src/GSE.GSSdk/` | `csharp/B1CSharpLoader/GameDll/GSE.GSSdk.dll` | [GSE.GSSdk.md](GSE.GSSdk.md) |
| 9 | **GSE.OnlineBase** | `docs/b1GameDLL-src/GSE.OnlineBase/` | `csharp/B1CSharpLoader/GameDll/GSE.OnlineBase.dll` | [GSE.OnlineBase.md](GSE.OnlineBase.md) |
| 10 | **GSE.ProtobufDB** | `docs/b1GameDLL-src/GSE.ProtobufDB/` | `csharp/B1CSharpLoader/GameDll/GSE.ProtobufDB.dll` | [GSE.ProtobufDB.md](GSE.ProtobufDB.md) |

## DLL 概述

| DLL | 源码规模 | 核心用途 |
|-----|---------|---------|
| b1.Managed | 3 文件 | IL2CPP 链接引用保留，防止类型裁剪 |
| b1.Native | 572 文件 | UE C++ 自动绑定层（Actor/Component/Struct/Enum），USharp 生成 |
| b1.NativePlugins | 760 文件 | UE 插件绑定（Wwise 音频、Calliope 序列、Houdini、V8、GSInput 等 43 插件） |
| B1UI_GSE.Script | 2282 文件 | UI 脚本层（视图/数据存储/关卡过渡/网络/RPC/在线好友） |
| BtlSvr.Main | 10138 文件 | **核心游戏逻辑**（战斗/AI/FSM/UI/ECS/网络/动画/渲染/编辑器），项目主体 |
| GSE.Core | 112 文件 | 核心运行时（数学库/原生容器/线程池/日志/调试/Protobuf 辅助/ECS/预测/性能分析） |
| GSE.GSNet | 42 文件 | 网络通信层（TCP/UDP/KCP 连接/通道/反向代理/消息帧） |
| GSE.GSSdk | 199 文件 | GSE SDK（HTTP 客户端/文件管理/RPC/性能采样/账户/数据上报） |
| GSE.OnlineBase | 13 文件 | 在线基础库（日志/指标/Protobuf 编解码/URL 解析/Zlib 压缩） |
| GSE.ProtobufDB | 1709 文件 | Protobuf 数据序列化层（持久化/复制/增量同步/Calliope FSM 定义/B2D 战斗） |

## DLL 依赖关系

```
B1UI_GSE.Script (UI 页面层)
        |
        v
  BtlSvr.Main (核心游戏逻辑)
        |
   /----|----\
   |    |    |
GSE.Core  GSE.GSSdk  GSE.GSNet
(ECS/运行)  (SDK)    (网络)
   |              |
GSE.ProtobufDB  GSE.OnlineBase
(数据定义)      (在线基础)
        |
  b1.Native + b1.NativePlugins
  (UE 绑定层)
        |
  b1.Managed (IL2CPP 引用保留)
```

## 架构模式总结

| 模式 | 实现位置 |
|------|---------|
| **事件驱动 (Pub-Sub)** | `BGW_EventCollection` + `BGW_UIEventCollection`，300+ 委托驱动模块间通信 |
| **Singleton 管理器** | `BGWGameInstanceCS.GetObject<T>()` 获取所有 `BGW_*` 管理器 |
| **ECS 数据模型** | GSE.Core 的 `EntityManager` + `Chunk` + `NativeHashMap/List`；游戏状态使用 `BGC/BPC/BUC` 持久容器 |
| **FSM 状态机** | Calliope FSM 系统 + GI_Global/GI_Loading 加载流程状态机 |
| **Protobuf 数据驱动** | `GSE.ProtobufDB` 的 `FUSt*Desc` / `TBFUSt*Desc` 配置表模式 |
| **Delta 同步** | 存档和网络复制使用 `DictDeltaMsg` / `ListDeltaMsg` 增量消息 |
| **UI 框架** | `GSBObject` → `GSDStore`(数据) + `GSUIView`(视图) + `GSAction`(Action) 模式 |
| **C++ 互操作** | `[UClass]` / `[UProperty]` / `[UFunction]` 属性标记 + `CppExport` 委托绑定 |
| **预测/回滚** | `GSPredictionKey` + `IPredictableObject` 客户端预测系统 |
| **ILRuntime 热更新** | `CrossBindingAdaptor` + `ILRuntimeBinding` 注册基类适配器 |

## 构建

项目使用 Visual Studio 方案文件进行构建：

- 方案文件: `csharp/B1CSharpLoader/CSharpLoader.sln`
- 每个 .csproj 配置了 Debug/Release 两个配置，输出到各项目的 `bin/Debug/` 或 `bin/Release/`
- 最终 DLL 产物被复制到 `csharp/B1CSharpLoader/GameDll/` 目录，由 B1CSharpLoader 加载

## 文档格式

每份文档包含：

- **模块结构** — 目录层级及子模块用途说明
- **命名空间清单** — 所有 namespace 及对应说明
- **类型清单** — 按命名空间或目录分组的完整类型列表，每项含：
  - 完整类型名称（含 namespace）
  - 类型（class/interface/struct/enum/delegate）
  - 继承关系（基类、实现接口）
  - 简短中文说明
  - 源文件路径
