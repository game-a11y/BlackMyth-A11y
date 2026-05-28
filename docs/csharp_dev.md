# C# Mod 开发

## 编译

```bash
# WSL / Linux
dotnet build CSharpMods/WkAccess/WkAccess.csproj

# 指定中间目录（解决 WSL /mnt/ 权限问题）
dotnet build CSharpMods/WkAccess/WkAccess.csproj \
  -p:BaseIntermediateOutputPath=/tmp/wkobj/ \
  -p:IntermediateOutputPath=/tmp/wkobj/net472/
```

编译产出 `CSharpMods/WkAccess/bin/Debug/net472/WkAccess.dll`，自动复制到游戏 Mod 目录。

## 日志

文件日志写入 Mod 所在目录下的 `WkAccess.log`，相对游戏根目录：

```
.\Mods-cs\WkAccess\WkAccess.log
```

映射关系：`Mods-cs` 对应 `CSharpLoader/Mods`。

日志特性：
- 零外部依赖，纯 `System.IO`
- 控制台 UTF-8 + 文件同时输出，调用 `A11yLog.Info` / `Warning` / `Error` 即可
- 文件超过 5MB 自动截断，保留末尾 1MB
- 按日志级别过滤（`Init` 时指定 `fileMinLevel`）
- 线程安全（`lock` + `?.` 兜底）

## 开发流程

1. 编辑 C# 代码（`CSharpMods/WkAccess/`）
2. 编译：`dotnet build CSharpMods/WkAccess/WkAccess.csproj`
3. 启动/重启游戏，或按 Ctrl+F5 热重载（需 `b1cs.ini` 中 `Develop=1`）
4. 查看日志确认行为

热重载注意：Develop 模式下每次重载生成新 Assembly，旧 Assembly 残留进程。
`DeInit()` 中应清理事件绑定、后台线程等资源。
