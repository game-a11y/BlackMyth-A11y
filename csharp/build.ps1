# C# Mod 构建脚本 (CSharpMods)
# 要求：.NET 8+ SDK (dotnet)
# 用法：双击或在终端中运行

$ErrorActionPreference = "Stop"
Push-Location $PSScriptRoot

# 检查 dotnet SDK
try {
    $dotnet = Get-Command dotnet -ErrorAction Stop
} catch {
    Write-Host "[ERROR] 未找到 dotnet SDK。请安装 .NET 8+ SDK:" -ForegroundColor Red
    Write-Host "        https://dotnet.microsoft.com/download"
    Pop-Location
    exit 1
}

Write-Host "[INFO] dotnet SDK: $(& dotnet --version)" -ForegroundColor Cyan

# 编译解决方案（包含 WkAccess 等所有 C# Mod 项目）
Write-Host "[BUILD] dotnet build CSharpMods.sln ..." -ForegroundColor Yellow
& dotnet build CSharpMods.sln

if ($LASTEXITCODE -eq 0) {
    Write-Host "[DONE] 编译成功" -ForegroundColor Green
} else {
    Write-Host "[FAIL] 编译失败 (exit code: $LASTEXITCODE)" -ForegroundColor Red
}

Pop-Location
exit $LASTEXITCODE
