# Mod 开发

## 项目结构

- `b1/`:  UE 5.0.3 版本的项目文件。用于尝试打包 `.pak` 类型的 Mod
- `BlackMythA11y/`:     Mod Lua 脚本代码。
- `BlackMythA11yCpp/`:  Mod C++ 代码。
- `docs/`:      文档。
- `RE-UE4SS/`:  MOD 框架。
- `release/`:   发布版本的 MOD 二进制包。
- `tools/`:     工具脚本。


## Mod 开发环境

依赖:
- UE4SS https://www.nexusmods.com/blackmythwukong/mods/19
    - 可以用 nexusmods 的专用版本；或者按照子模块 commit 自行编译
- https://github.com/sig-a11y/tolk

### 构建失败

特别是更新了 RE-UE4SS 版本后。

- 检查构建工具版本
- 清理构建文件夹

```bash
rm -rf .xmake
rm -rf Binaries
rm -rf Intermediates
xmake config --mode="Game__Shipping__Win64" --yes

# if get error:  dllmain.cpp: No such file or directory #927
xmake build proxy_generator
```


## Mod 成品

- [Mods at Black Myth: Wukong Nexus - Mods and community](https://www.nexusmods.com/blackmythwukong/mods/)
  - [Steam 社区 :: 指南 :: 必装MOD推荐 | 保姆级前置安装教程 | 8.24 更新至优化 MOD](https://steamcommunity.com/sharedfiles/filedetails/?id=3315419800)


## Mod 开发资料

- [Home - UE4SS Documentation](https://docs.ue4ss.com/index.html)
- [Lua Modding > Introduction | Palworld Modding Docs](https://pwmodding.wiki/docs/lua-modding/lua-intro)
- [kboykboy2/UnrealEngine: Unreal Engine source code](https://github.com/kboykboy2/UnrealEngine)
    这是黑神话使用的修改版本
- 蓝图MOd框架: https://github.com/narknon/WukongB1
- https://modding.wiki/en/hogwartslegacy/developers
