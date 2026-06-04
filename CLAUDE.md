# CLAUDE.md

本文档为 Claude Code (claude.ai/code) 在此仓库中工作时提供指导。

## 项目概述

《黑神话：悟空》无障碍 Mod，分为四部分：

- **UE4SS 部分：** `ue4ss/BlackMythA11y/`（Lua 脚本）+ `ue4ss/BlackMythA11yCpp/`（C++ 插件）+ `ue4ss/RE-UE4SS/`（框架），同属一个 UE4SS Mod 项目
- **C# 部分：** `csharp/` — 通过 B1CSharpLoader 框架热加载的 C# Mod
- **Pak 部分：** `pak/` — UE4 pak 打包形式的 Mod（直接替换游戏资产/配置）
- **项目公共部分：** `docs/`（公共文档 + 设计思路）

各模块 README：[UE4SS](ue4ss/README.md) | [C#](csharp/README.md) | [Pak](pak/README.md)

> **注意：** 仓库中的 `GameDir` 软链接指向本地游戏安装目录，用于 Mod 部署和调试。  
> 具体 Mod 部署通过各部分的 `*Mods-*` / `*.lnk` 软链接指向游戏目录下的对应位置。

C# Mod 开发指引见 [csharp/docs/csharp_dev.md](csharp/docs/csharp_dev.md)。架构规范见 [csharp/docs/architecture.md](csharp/docs/architecture.md)。

## 项目结构

```
Root/
│
├── 📦 UE4SS 部分 (ue4ss/)
│   ├── BlackMythA11y/           # Lua 脚本（入口 main.lua + UIHooks/）
│   │   ├── Scripts/             # 源码 + UIHooks/ 子模块
│   │   └── dlls/                # C++ 插件 DLL
│   ├── BlackMythA11yCpp/        # C++ 插件源码（xmake 构建）
│   │   ├── mod.cpp / dllmain.cpp
│   │   └── include/             # mod.hpp, Tolk.h
│   ├── RE-UE4SS/                # UE4SS 框架（上游 submodule）
│   ├── release/                 # 发布包
│   ├── tools/                   # 构建/打包脚本
│   ├── docs/                    # 开发文档
│   ├── Binaries/ + Intermediates/
│
├── 📦 C# 部分 (csharp/)
│   ├── B1CSharpLoader/          # C# 加载器框架（来自上游）
│   ├── docs/                    # C# 开发文档 + 架构规范
│   ├── WkAccess/                # 无障碍 Mod 源码（详见架构规范）
│   │   ├── A11y/                # 跨游戏基础（日志、TTS、文本提取）
│   │   ├── A11yMod/             # 无障碍策略 + Harmony Patch
│   │   ├── BM/                  # 游戏数据层（场景/交互/UI 解析）
│   │   ├── ModMain.cs           # Mod 入口 + 生命周期
│   │   └── BuildInfo.cs / GlobalUsings.cs
│   └── WkAccess-MOD/            # 游戏部署文件
│
├── 📦 Pak 部分 (pak/)
│   ├── b1/                      # UE 测试工程
│   └── README.md
│
├── 📦 公共部分
│   └── docs/                    # 功能清单、设计思路、DLL 参考
│
└── CLAUDE.md                    # 本文件
```

## 软链接索引

仓库中使用软链接连接本地游戏环境和部署目录：

| 软链接 | 指向 | 用途 |
|---|---|---|
| `GameDir` | 游戏安装根目录 | 根入口 |
| `ue4ss/Mods-lua` | 游戏 UE4SS Mods 目录 | Lua Mod 热部署 |
| `ue4ss/ue4ss.lnk` | 游戏 ue4ss 目录 | UE4SS 文件引用 |
| `csharp/Mods-cs` | 游戏 C# Mods 目录 | C# Mod 热部署 |
| `csharp/CSLoader.lnk` | 游戏 CSharpLoader 目录 | 加载器引用 |

## 提交规范

使用 Conventional Commits，格式：`<type>(<scope>): <简短中文描述>`

允许的 type：

| type | 使用场景 |
|---|---|
| `cs` | C# 代码变更 |
| `lua` | Lua 脚本变更 |
| `cpp` | C++ 代码变更 |
| `docs` | 文档变更 |
| `chore` | 构建、配置、杂项 |
| `ref` | 参考资料变更 |
| `TODO` | TODO 计划项 |

scope 为模块名，按功能域划分，不具体到类/文件，例如：

| type | scope 示例 |
|---|---|
| `cs` | `A11y`, `BM`, `A11yMod`, `BM.UI` |
| `lua` | `WkUIHook`, `WkConfig` |
| `docs` | `DLL`, `CLAUDE`, `csharp_dev` |
| `ref` | `GameDll` |

原则：**原子提交** — 每个提交只解决一个问题，不混入不相关的修改。

示例：

```
cs(A11y): 设置菜单完整朗读（类型 - 名称 - 值）
cs(A11yMod): 日志格式统一为 {cn}#{gsid}
docs(DLL): 添加 GameDLL 类导航文档
lua(WkUIHook): 更新读屏焦点跟随逻辑
chore: 忽略 mod 文件夹软连接
```
