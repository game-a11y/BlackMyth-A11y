# BlackMyth-A11y 自动打包脚本
# 生成带时间戳的发布包

param(
    [string]$ProjectRoot = "d:\a11y\BlackMyth-A11y",
    [string]$modBaseName = "WkA11y-黑神话悟空无障碍补丁"
)

# 设置错误处理
$ErrorActionPreference = "Stop"

# 批量复制并替换文件的函数
function Copy-ReplaceFiles {
    param(
        [hashtable]$FileMappings,
        [string]$Description = "文件批量复制"
    )
    
    $successCount = 0
    $failureCount = 0
    $totalCount = $FileMappings.Count
    
    Write-Host "开始 $Description (共 $totalCount 个文件)..." -ForegroundColor Cyan
    
    foreach ($mapping in $FileMappings.GetEnumerator()) {
        $RelativeSourcePath = $mapping.Key
        $RelativeDestinationPath = $mapping.Value
        
        $SourcePath = Join-Path $ProjectRoot $RelativeSourcePath
        $DestinationPath = Join-Path $ProjectRoot $RelativeDestinationPath
        
        if (Test-Path $SourcePath) {
            try {
                # 确保目标目录存在
                $destDir = Split-Path $DestinationPath -Parent
                if (!(Test-Path $destDir)) {
                    New-Item -ItemType Directory -Path $destDir -Force | Out-Null
                }
                
                # 复制文件并替换
                Copy-Item -Path $SourcePath -Destination $DestinationPath -Force
                Write-Host "  ✓ 已复制: $RelativeSourcePath -> $RelativeDestinationPath" -ForegroundColor Gray
                $successCount++
            } catch {
                Write-Warning "  ✗ 复制失败: $RelativeSourcePath -> $RelativeDestinationPath ($(_.Exception.Message))"
                $failureCount++
            }
        } else {
            Write-Warning "  ✗ 源文件不存在: $RelativeSourcePath"
            $failureCount++
        }
    }
    
    # 输出汇总信息
    # Write-Host "$Description 完成: 成功 $successCount 个, 失败 $failureCount 个" -ForegroundColor $(if ($failureCount -eq 0) { 'Green' } else { 'Yellow' })
}

# 批量复制并替换文件夹的函数
function Copy-ReplaceFolders {
    param(
        [hashtable]$FolderMappings,
        [string]$Description = "文件夹批量复制",
        [switch]$RemoveDestination = $false
    )
    
    $successCount = 0
    $failureCount = 0
    $totalCount = $FolderMappings.Count
    
    Write-Host "开始 $Description (共 $totalCount 个文件夹)..." -ForegroundColor Cyan
    
    foreach ($mapping in $FolderMappings.GetEnumerator()) {
        $RelativeSourcePath = $mapping.Key
        $RelativeDestinationPath = $mapping.Value
        
        $SourcePath = Join-Path $ProjectRoot $RelativeSourcePath
        $DestinationPath = Join-Path $ProjectRoot $RelativeDestinationPath
        
        if (Test-Path $SourcePath) {
            try {
                # 如果需要，删除目标目录
                if ($RemoveDestination -and (Test-Path $DestinationPath)) {
                    Remove-Item -Path $DestinationPath -Recurse -Force
                    Write-Host "  - 已删除现有目标目录: $RelativeDestinationPath" -ForegroundColor Gray
                }
                
                # 确保目标父目录存在
                $destParent = Split-Path $DestinationPath -Parent
                if (!(Test-Path $destParent)) {
                    New-Item -ItemType Directory -Path $destParent -Force | Out-Null
                }
                
                # 复制文件夹
                Copy-Item -Path $SourcePath -Destination $DestinationPath -Recurse -Force
                Write-Host "  ✓ 已复制: $RelativeSourcePath -> $RelativeDestinationPath" -ForegroundColor Gray
                $successCount++
            } catch {
                Write-Warning "  ✗ 复制失败: $RelativeSourcePath -> $RelativeDestinationPath ($(_.Exception.Message))"
                $failureCount++
            }
        } else {
            Write-Warning "  ✗ 源文件夹不存在: $RelativeSourcePath"
            $failureCount++
        }
    }
    
    # 输出汇总信息
    # Write-Host "$Description 完成: 成功 $successCount 个, 失败 $failureCount 个" -ForegroundColor $(if ($failureCount -eq 0) { 'Green' } else { 'Yellow' })
}

# 删除文件列表的函数
function Remove-FileList {
    param(
        [string]$RelativeRootDirectory,
        [string[]]$RelativeFilePaths,
        [string]$Description = "文件清理"
    )
    
    $RootDirectory = Join-Path $ProjectRoot $RelativeRootDirectory
    
    $deletedCount = 0
    $notFoundCount = 0
    $failureCount = 0
    $totalCount = $RelativeFilePaths.Count
    
    Write-Host "开始 $Description (共 $totalCount 个文件)..." -ForegroundColor Cyan
    
    foreach ($relativeFile in $RelativeFilePaths) {
        $fullPath = Join-Path $RootDirectory $relativeFile
        
        if (Test-Path $fullPath) {
            try {
                Remove-Item -Path $fullPath -Force
                Write-Host "  ✓ 已删除: $relativeFile" -ForegroundColor Gray
                $deletedCount++
            } catch {
                Write-Warning "  ✗ 删除失败: $relativeFile ($(_.Exception.Message))"
                $failureCount++
            }
        } else {
            Write-Host "  - 文件不存在: $relativeFile" -ForegroundColor Gray
            $notFoundCount++
        }
    }
    
    # 输出汇总信息
    # $statusColor = if ($failureCount -eq 0) { 'Green' } else { 'Yellow' }
    # Write-Host "$Description 完成: 删除 $deletedCount 个, 不存在 $notFoundCount 个, 失败 $failureCount 个" -ForegroundColor $statusColor
}

# 创建 ZIP 压缩包的函数
function Create-ZipPackage {
    param(
        [string]$RelativeSourceFolder,
        [string]$RelativeZipFileName,
        [string]$Description = "创建 ZIP 压缩包"
    )
    
    $sourcePath = Join-Path $ProjectRoot $RelativeSourceFolder
    $zipPath = Join-Path $ProjectRoot $RelativeZipFileName
    
    Write-Host "开始 $Description..." -ForegroundColor Cyan
    # Write-Host "  源文件夹: $RelativeSourceFolder" -ForegroundColor Gray
    # Write-Host "  目标文件: $RelativeZipFileName" -ForegroundColor Gray
    
    # 检查源文件夹是否存在
    if (!(Test-Path $sourcePath)) {
        Write-Error "源文件夹不存在: $RelativeSourceFolder"
        return $false
    }
    

    # 删除已存在的 ZIP 文件
    if (Test-Path $zipPath) {
        Remove-Item -Path $zipPath -Force
        Write-Host "  ✓ 已删除旧的 ZIP 文件" -ForegroundColor Gray
    }
    
    # 确保目标目录存在
    $zipDir = Split-Path $zipPath -Parent
    if (!(Test-Path $zipDir)) {
        New-Item -ItemType Directory -Path $zipDir -Force | Out-Null
    }
    
    # 使用 .NET 压缩
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    [System.IO.Compression.ZipFile]::CreateFromDirectory($sourcePath, $zipPath)
    
    Write-Host "  ✓ ZIP 文件已创建: $RelativeZipFileName" -ForegroundColor Gray
    
    # 计算文件大小和 SHA256 哈希值
    $fileSize = [math]::Round((Get-Item $zipPath).Length / 1MB, 2)
    $hash = Get-FileHash -Path $zipPath -Algorithm SHA256
    
    Write-Host "  文件大小: $fileSize MB" -ForegroundColor White
    Write-Host "  SHA256: $($hash.Hash)" -ForegroundColor Yellow
}


# ==== 主函数
try {
    # 生成时间戳
    $timestamp = Get-Date -Format "yyyyMMdd+HHmm"
    $packageName = "$modBaseName-v$timestamp"

    Write-Host "开始打包 $modBaseName" -ForegroundColor Green

    # ==== 复制 BlackMythA11y Mod 文件夹到 release 目录中的正确位置
    $modFolderMappings = @{
        "BlackMythA11y" = "release\b1\Binaries\Win64\ue4ss\Mods\BlackMythA11y"
    }
    Copy-ReplaceFolders -FolderMappings $modFolderMappings -Description "复制 $modBaseName Mod 文件夹" -RemoveDestination

    # ==== 批量复制 UE4SS 文件和 dwmapi.dll 代理文件
    $fileMappings = @{
        "Binaries\Game__Shipping__Win64\UE4SS\UE4SS.dll" = "release\b1\Binaries\Win64\ue4ss\UE4SS.dll"
        "Binaries\Game__Shipping__Win64\UE4SS\UE4SS.pdb" = "release\b1\Binaries\Win64\ue4ss\UE4SS.pdb"
        "Binaries\Game__Shipping__Win64\proxy\dwmapi.dll" = "release\b1\Binaries\Win64\dwmapi.dll"
    }
    Copy-ReplaceFiles -FileMappings $fileMappings -Description "批量复制 UE4SS 和代理文件"

    # ==== 将准备好的 release 文件夹复制到最终的包目录
    $finalFolderMappings = @{
        "release" = $packageName
    }
    Copy-ReplaceFolders -FolderMappings $finalFolderMappings -Description "复制到最终包目录"

    # ==== 删除 $packageName 目录中不需要的文件
    $filesToRemove = @(
        ".gitignore",
        "00安装说明-性能测试工具版.txt"
    )
    Remove-FileList -RelativeRootDirectory $packageName -RelativeFilePaths $filesToRemove -Description "清理不需要的文件"

    # ==== 压缩为 ZIP 文件
    Create-ZipPackage -RelativeSourceFolder $packageName -RelativeZipFileName "$packageName.zip"

    Write-Host "=== 打包完成 ===" -ForegroundColor Green
} catch {
    Write-Error "打包过程中发生错误: $($_.Exception.Message)"
    exit 1
}
