# Black Myth: WuKong - A11y Mod

> An accessibility mod for "Black Myth: Wukong"
>
> 适用于游戏 <黑神话：悟空> 的无障碍补丁


## Mod 组成

本 Mod 由三部分组成，分别通过不同机制挂载到游戏中：

| 部分 | 目录 | 机制 | 文档 |
|------|------|------|------|
| **UE4SS Mod** | `ue4ss/` | Lua 脚本 + C++ 插件，基于 [UE4SS](https://docs.ue4ss.com/) 框架注入 | [说明](ue4ss/README.md) |
| **C# Mod** | `csharp/` | C# 程序集，基于 [B1CSharpLoader](https://github.com/czastack/B1CSharpLoader) 热加载 | [说明](csharp/README.md) |
| **Pak Mod** | `pak/` | UE `.pak` 打包格式，直接替换游戏资产/配置 | [说明](pak/README.md) |

三者可独立或组合使用，互不冲突。


## Mod 安装

> 注意：MOD 开发中！使用时需要注意保存当前的存档。

安装参考:

- B1CSharpLoader: [**00WkAccess安装说明.txt**](csharp/WkAccess-MOD/00WkAccess安装说明.txt)
- UE4SS: [**00安装说明.txt**](ue4ss/release/00安装说明.txt)


## Mod 开发

See: [CLAUDE.md](CLAUDE.md)


## LICENSE

本项目基于 [MIT License](LICENSE)
