# CLAUDE.md

本文档为 Claude Code (claude.ai/code) 在此仓库中工作时提供指导。

## 项目概述

《黑神话：悟空》无障碍 Mod，分为四部分：

- **UE4SS 部分：** `ue4ss/BlackMythA11y/`（Lua 脚本）+ `ue4ss/BlackMythA11yCpp/`（C++ 插件）+ `ue4ss/RE-UE4SS/`（框架），同属一个 UE4SS Mod 项目
- **C# 部分：** `csharp/` — 通过 B1CSharpLoader 框架热加载的 C# Mod
- **Pak 部分：** `pak/` — UE4 pak 打包形式的 Mod（直接替换游戏资产/配置）
- **项目公共部分：** `docs/`（公共文档）

> **注意：** 仓库中的 `GameDir` 软链接指向本地游戏安装目录，用于 Mod 部署和调试。  
> 具体 Mod 部署通过各部分的 `*Mods-*` / `*.lnk` 软链接指向游戏目录下的对应位置。

C# Mod 开发指引见 [docs/csharp_dev.md](docs/csharp_dev.md)。

## 项目结构

```
Root/
│
├── 📦 UE4SS 部分 (ue4ss/)
│   ├── BlackMythA11y/           # Lua 脚本 Mod
│   │   ├── Scripts/             # Lua 源码
│   │   │   ├── main.lua
│   │   │   ├── WkUIHook.lua
│   │   │   ├── WkScriptHooks.lua
│   │   │   ├── WkKeyBind.lua
│   │   │   ├── WkUtils.lua
│   │   │   ├── WkConfig.lua
│   │   │   ├── WkGlobals.lua
│   │   │   ├── WkzTemplateMod.lua
│   │   │   └── UIHooks/        # UI 挂钩子模块
│   │   ├── dlls/                # 编译好的 C++ 插件 DLL
│   │   └── enabled.txt
│   ├── BlackMythA11yCpp/        # C++ 插件源码（xmake 构建）
│   │   ├── mod.cpp / sr.cpp / dllmain.cpp
│   │   ├── include/
│   │   │   ├── mod.hpp / sr.hpp / WkCommon.hpp
│   │   │   └── Tolk.h           # 读屏（Tolk）封装
│   │   └── xmake.lua
│   ├── RE-UE4SS/                # UE4SS 框架（上游 fork，submodule）
│   ├── release/                 # 发布包（含安装说明）
│   ├── tools/                   # 构建/打包工具
│   │   ├── package.ps1 / build_and_install.lua / install_mod.lua
│   │   └── bp_visual.py         # 蓝图可视化工具
│   ├── docs/                    # UE4SS 相关文档
│   │   ├── BUI.WidgetTree/      # BUI 控件树文档
│   │   └── dev.md / mods.md / pack.md
│   ├── Binaries/                # UE4SS 构建输出
│   └── Intermediates/           # UE4SS 中间文件
│
├── 📦 C# 部分 (csharp/)
│   ├── B1CSharpLoader/          # C# 加载器框架（来自上游）
│   │   ├── CSharpLoaderDll/     # C++ 加载器 DLL（注入游戏）
│   │   ├── CSharpManager/       # Mod 加载器/管理器
│   │   ├── CSharpModBase/       # ICSharpMod 接口 + 日志/工具
│   │   ├── CSharpModExample/    # Mod 示例
│   │   ├── GameDll/             # 编译好的游戏 DLL（仅供引用）
│   │   └── extract_dlls.py      # DLL 提取脚本
│   ├── WkAccess/                # ** 当前无障碍 Mod 源码 **
│   │   ├── A11y/
│   │   │   ├── A11yLog.cs       # 控制台日志
│   │   │   ├── A11yTolk.cs      # 读屏（Tolk）C# 封装
│   │   │   ├── DebugCommands.cs # 调试命令
│   │   │   ├── KeyBindings.cs   # 按键绑定
│   │   │   ├── SceneDetector.cs # 场景/UI 状态检测
│   │   │   └── UI/              # UI 辅助类
│   │   │       ├── ButtonTextProvider.cs
│   │   │       ├── UIFocusTracker.cs
│   │   │       ├── UIMouseTracker.cs
│   │   │       └── UIScreenTextProvider.cs
│   │   ├── ModMain.cs           # Mod 入口
│   │   ├── WkUtils.cs           # UE 世界/玩家辅助方法
│   │   ├── BuildInfo.cs / GlobalUsings.cs
│   │   └── Properties/
│   └── WkAccess-MOD/            # C# Mod 游戏部署文件
│       ├── b1/                  # 部署目录结构
│       └── WkAccess-MOD说明文档/
│
├── 📦 Pak 部分 (pak/)
│   ├── b1/                      # UE4 测试工程（供打包参考）
│   └── README.md
│
├── 📦 项目公共部分
│   ├── docs/
│   │   ├── b1GameDLL-doc/       # 游戏 DLL 类导航文档（含 _index.yaml）
│   │   ├── b1GameDLL-src/       # 游戏 DLL 源码（参考用，含 .sln）
│   │   ├── csharp_dev.md        # C# Mod 开发指引
│   │   ├── csharp_hook_boundary_test.md
│   │   ├── csharp_technical_validation.md
│   │   ├── feature_list.md      # 项目功能清单
│   │   └── idea.md              # 设计思路/想法记录
│   └── .cyhan/                  # 本地开发备份/发布目录
│       ├── B1CSharpLoader/
│       ├── 【CSLoader】/ 【Mods】/ 【paks】/ 【UE4SS】/ 【存档】/ 【已发布】
│       └── ...
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

使用 Conventional Commits，格式：`<type>: <简短中文描述>`

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

原则：**原子提交** — 每个提交只解决一个问题，不混入不相关的修改。

示例：

```
cs: 添加场景检测器，挂钩场景加载/UI 页面切换
docs: 添加 10 个游戏 DLL 的类导航文档
chore: 忽略 mod 文件夹软连接
lua: 更新读屏焦点跟随逻辑
```
