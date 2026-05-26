# GameDll-doc — C# 游戏 DLL 类导航文档

本文档目录包含《黑神话：悟空》C# Mod 中 10 个核心 DLL 的类导航文档，方便快速查找类型定义。

## 路径说明

所有路径均相对于项目根目录（`/mnt/a/Game-A11y/BlackMyth-A11y/`）。

| # | DLL 程序集 | 源码路径 (GameDll-src) | 编译产物 (GameDll) | 文档 |
|---|-----------|----------------------|-------------------|------|
| 1 | **b1.Managed** | `CSharpMods/B1CSharpLoader/GameDll-src/b1.Managed/` | `CSharpMods/B1CSharpLoader/GameDll/b1.Managed.dll` | [b1.Managed.md](b1.Managed.md) |
| 2 | **b1.Native** | `CSharpMods/B1CSharpLoader/GameDll-src/b1.Native/` | `CSharpMods/B1CSharpLoader/GameDll/b1.Native.dll` | [b1.Native.md](b1.Native.md) |
| 3 | **b1.NativePlugins** | `CSharpMods/B1CSharpLoader/GameDll-src/b1.NativePlugins/` | `CSharpMods/B1CSharpLoader/GameDll/b1.NativePlugins.dll` | [b1.NativePlugins.md](b1.NativePlugins.md) |
| 4 | **B1UI_GSE.Script** | `CSharpMods/B1CSharpLoader/GameDll-src/B1UI_GSE.Script/` | `CSharpMods/B1CSharpLoader/GameDll/B1UI_GSE.Script.dll` | [B1UI_GSE.Script.md](B1UI_GSE.Script.md) |
| 5 | **BtlSvr.Main** | `CSharpMods/B1CSharpLoader/GameDll-src/BtlSvr.Main/` | `CSharpMods/B1CSharpLoader/GameDll/BtlSvr.Main.dll` | [BtlSvr.Main.md](BtlSvr.Main.md) |
| 6 | **GSE.Core** | `CSharpMods/B1CSharpLoader/GameDll-src/GSE.Core/` | `CSharpMods/B1CSharpLoader/GameDll/GSE.Core.dll` | [GSE.Core.md](GSE.Core.md) |
| 7 | **GSE.GSNet** | `CSharpMods/B1CSharpLoader/GameDll-src/GSE.GSNet/` | `CSharpMods/B1CSharpLoader/GameDll/GSE.GSNet.dll` | [GSE.GSNet.md](GSE.GSNet.md) |
| 8 | **GSE.GSSdk** | `CSharpMods/B1CSharpLoader/GameDll-src/GSE.GSSdk/` | `CSharpMods/B1CSharpLoader/GameDll/GSE.GSSdk.dll` | [GSE.GSSdk.md](GSE.GSSdk.md) |
| 9 | **GSE.OnlineBase** | `CSharpMods/B1CSharpLoader/GameDll-src/GSE.OnlineBase/` | `CSharpMods/B1CSharpLoader/GameDll/GSE.OnlineBase.dll` | [GSE.OnlineBase.md](GSE.OnlineBase.md) |
| 10 | **GSE.ProtobufDB** | `CSharpMods/B1CSharpLoader/GameDll-src/GSE.ProtobufDB/` | `CSharpMods/B1CSharpLoader/GameDll/GSE.ProtobufDB.dll` | [GSE.ProtobufDB.md](GSE.ProtobufDB.md) |

## DLL 概述

| DLL | 源码规模 | 核心用途 |
|-----|---------|---------|
| b1.Managed | 2 文件 | IL2CPP 链接引用保留，防止类型裁剪 |
| b1.Native | 558 文件 | UE C++ 自动绑定层（Actor/Component/Struct/Enum），USharp 生成 |
| b1.NativePlugins | 741 文件 | UE 插件绑定（Wwise 音频、Calliope 序列、Houdini、V8、GSInput 等） |
| B1UI_GSE.Script | 2134 文件 | UI 脚本层（视图/数据存储/关卡过渡/网络/RPC） |
| BtlSvr.Main | 10834 文件 | **核心游戏逻辑**（战斗/AI/FSM/UI/ECS/网络/动画/编辑器），项目主体 |
| GSE.Core | 111 文件 | 核心运行时（数学库/原生容器/线程池/日志/调试/Protobuf 辅助） |
| GSE.GSNet | 42 文件 | 网络通信层（TCP/UDP 连接/通道/反向代理/消息帧） |
| GSE.GSSdk | 198 文件 | GSE SDK（HTTP 客户端/文件管理/RPC/性能采样/账户/数据上报） |
| GSE.OnlineBase | 14 文件 | 在线基础库（日志/指标/Protobuf 编解码/URL 解析/Zlib 压缩） |
| GSE.ProtobufDB | 1651 文件 | Protobuf 数据序列化层（持久化/复制/增量同步/Calliope FSM 定义） |

## 构建

项目使用 Visual Studio 方案文件进行构建：

- 方案文件: `CSharpMods/B1CSharpLoader/CSharpLoader.sln`
- 每个 .csproj 配置了 Debug/Release 两个配置，输出到各项目的 `bin/Debug/` 或 `bin/Release/`
- 最终 DLL 产物被复制到 `CSharpMods/B1CSharpLoader/GameDll/` 目录，由 B1CSharpLoader 加载

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
