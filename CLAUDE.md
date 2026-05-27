# CLAUDE.md

本文档为 Claude Code (claude.ai/code) 在此仓库中工作时提供指导。

## 项目概述

《黑神话：悟空》无障碍 Mod，分为三部分：

- **UE4SS 部分：** `BlackMythA11y/`（Lua 脚本）+ `BlackMythA11yCpp/`（C++ 插件），两者同属一个 UE4SS Mod 项目
- **C# 部分：** `CSharpMods/` — 通过 B1CSharpLoader 框架热加载的 C# Mod
- **项目公共部分：** `docs/`、`release/`、`RE-UE4SS/`（框架）、`tools/`


## 项目结构

```
Root/
│
├── 📦 UE4SS 部分
│   ├── BlackMythA11y/           # Lua 脚本
│   └── BlackMythA11yCpp/        # C++ 插件
│
├── 📦 C# 部分
│   ├── B1CSharpLoader/          # C# 加载器框架（来自上游）
│   │   ├── GameDll/             # 编译好的游戏 DLL（仅供引用）
│   │   ├── GameDll-src/         # 游戏 DLL 源码（参考用）
│   │   ├── CSharpModBase/       # ICSharpMod 接口
│   │   ├── CSharpModExample/    # Mod 示例
│   │   └── CSharpManager/       # Mod 加载器/管理器
│   └── WkAccess/                # ** 当前无障碍 Mod **
│       ├── A11y/
│       │   ├── A11yLog.cs       # 控制台日志
│       │   └── SceneDetector.cs # 场景/UI 状态检测
│       ├── ModMain.cs           # Mod 入口
│       └── WkUtils.cs           # UE 世界/玩家辅助方法
│
├── 📦 项目公共部分
│   ├── RE-UE4SS/                # UE4SS 框架（上游 fork）
│   ├── docs/
│   │   ├── GameDll-doc/         # 游戏 DLL 类导航文档
│   │   └── csharp_technical_validation.md
│   ├── release/                 # 发布包
│   ├── tools/                   # 工具脚本
│   └── CSharpMods/WkAccess-MOD/ # C# Mod 游戏部署文件
```

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

示例：

```
cs: 添加场景检测器，挂钩场景加载/UI 页面切换
docs: 添加 10 个游戏 DLL 的类导航文档
chore: 忽略 mod 文件夹软连接
lua: 更新读屏焦点跟随逻辑
```
