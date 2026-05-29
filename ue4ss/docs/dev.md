# Mod 开发

## Mod 开发环境

依赖:
- UE4SS https://www.nexusmods.com/blackmythwukong/mods/19
    - 可以用 nexusmods 的专用版本；或者按照子模块 commit 自行编译
- https://github.com/sig-a11y/tolk
- 建立游戏根目录的软连接
  - Win CMD:  `mklink /d GameDir  G:\Steam\steamapps\common\BlackMythWukong`

### 构建失败

特别是更新了 RE-UE4SS 版本后。

- 检查构建工具版本
- 清理构建文件夹

```bash
rm -rf .xmake
rm -rf Binaries
rm -rf Intermediates
xmake config --mode="Game__Shipping__Win64" --yes
xmake config --mode="Game__Debug__Win64" --yes

# if get error:  dllmain.cpp: No such file or directory #927
xmake build proxy_generator
# raw_pdb
#   https://github.com/xmake-io/xmake-repo/blob/44eb9538ac9bdbaa9eddb516fb6f295f22a535c1/packages/r/raw_pdb/xmake.lua
```
