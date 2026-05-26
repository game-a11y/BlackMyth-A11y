# GSE.GSSdk 类导航

## 模块结构

```
GSE.GSSdk/
├── GSSDKClient.cs               # GSSDK HTTP 客户端核心
├── GSSDKEnv.cs                   # GSSDK 环境/全局状态
├── GSSDKPerf.cs                  # 性能采样模块
├── GSSDKPerfEvent.cs             # 性能事件数据结构
├── GSSDKReport.cs                # 数据上报核心模块
├── GSSDKReportQueue.cs           # 上报队列（线程/本地持久化）
├── GSSDKRpc.cs                   # 远程过程调用（RPC）封装层
├── GSSdkEnvironmentContextInitCallback.cs  # 环境初始化回调委托
├── HttpPostAsyncProxy.cs         # 异步 HTTP POST 代理委托
├── HttpRequestDelegate.cs        # HTTP 请求委托
├── HttpResponseFinishNotify.cs   # HTTP 响应完成通知委托
├── OnBytesResponse.cs            # 字节响应回调委托
├── OnJsonResponse.cs             # JSON 响应回调委托
├── OnProtobufResponse.cs         # Protobuf 响应回调委托
├── IFileOperationStrategy.cs     # 文件操作策略接口
├── CSharpFileOperationStrategy.cs# C# 标准文件操作实现
├── UGSFileOperationStrategy.cs   # Unreal UGS 文件操作实现
├── FileManager.cs                # 文件管理器（策略模式）
├── ZipUtils.cs                   # GZip 压缩/解压工具
├── BucketCfg.cs                  # 桶配置（采样分桶）
├── MonEvtCatogray.cs             # 监控事件分类枚举
├── ReportEventItemRaw.cs         # 原始上报事件项
├── ReportEventWorkerParam.cs     # 上报工作线程参数
├── ILRuntimeBinding.cs           # ILRuntime 运行时注册
├── Properties/
│   └── AssemblyInfo.cs           # 程序集元信息
└── Gssdk/                        # Protobuf 生成的协议类型 (namespace Gssdk)
    ├── 配置模型（5 个）
    ├── 实体/数据模型（15 个）
    ├── 请求/响应类型（80+ 个）
    ├── 枚举类型（9 个）
    └── 枚举包装类型（18 个）
```

**根目录 (GSE.GSSdk namespace)**: 包含 SDK 核心运行时、HTTP 通信、文件 I/O 策略、数据上报引擎、RPC 封装。

**Gssdk 子目录 (Gssdk namespace)**: 包含从 Protobuf 协议文件自动生成的 C# 消息类，涵盖配置、用户信息、认证、目录服务、版本管理、数据上报和开发者工具。

---

## 命名空间清单

| 命名空间 | 说明 |
|----------|------|
| `GSE.GSSdk` | 主命名空间，包含 SDK 核心类、委托、接口和基础设施 |
| `Gssdk` | Protobuf 协议数据模型命名空间 |

---

## 类/接口/结构/枚举清单

### 命名空间 `GSE.GSSdk`

#### 核心运行时

| 完整类型名称 | 类型 | 继承关系 | 说明 | 文件路径 |
|-------------|------|---------|------|---------|
| `GSE.GSSdk.GSSDKClient` | class | `object` | GSSDK HTTP 客户端。封装 JSON/Protobuf 请求的序列化与远程调用，支持 Json、Protobuf 两种编码格式，提供可配置的超时和加密传输 | `GSE.GSSdk/GSSDKClient.cs` |
| `GSE.GSSdk.GSSDKEnv` | class (static) | `object` | GSSDK 环境上下文。管理 SDK 服务端列表（含灾备切换）、用户信息全局单例、Service Key 管理，提供 `InitFromConfig()` 初始化入口 | `GSE.GSSdk/GSSDKEnv.cs` |
| `GSE.GSSdk.GSSDKRpc` | class (static) | `object` | RPC 静态网关。面向 ReportView/DevReport/Dir/Auth/Version/Report 等服务封装 `CallProtobufService<>` 调用，并为 ILRuntime 注册委托转换 | `GSE.GSSdk/GSSDKRpc.cs` |
| `GSE.GSSdk.GSSDKReport` | class (static) | `object` | 数据上报引擎。管理监控指标(Monitor)、用户事件(Event)、跟踪(Track)、文件上传(UploadFiles)的批量聚合与异步推送 | `GSE.GSSdk/GSSDKReport.cs` |
| `GSE.GSSdk.GSSDKReportQueue` | class (static) | `object` | 上报队列调度器。运行独立后台线程，消费 `ConcurrentQueue<ReportEventItemRaw>`，支持 Protobuf 编码、GZip 压缩、本地文件持久化(故障恢复)及 HTTP 重试 | `GSE.GSSdk/GSSDKReportQueue.cs` |
| `GSE.GSSdk.GSSDKPerf` | class (static) | `object` | 性能采样框架。基于 `MetricStastic` 按低频率 Tick 收集性能指标并自动上报 | `GSE.GSSdk/GSSDKPerf.cs` |
| `GSE.GSSdk.GSSDKPerfEvent` | class | `object` | 性能事件数据记录（Key/IntVal/StrVal/Props） | `GSE.GSSdk/GSSDKPerfEvent.cs` |
| `GSE.GSSdk.ILRuntimeBinding` | class (static) | `object` | ILRuntime 绑定注册器。统一注册 Auth/Dir/Version/Report 各模块的回调委托到 ILRuntime AppDomain | `GSE.GSSdk/ILRuntimeBinding.cs` |

#### 基础设施

| 完整类型名称 | 类型 | 继承关系 | 说明 | 文件路径 |
|-------------|------|---------|------|---------|
| `GSE.GSSdk.IFileOperationStrategy` | interface | - | 文件操作策略接口。定义目录/文件存在检查、读写删除、时间戳、Protobuf消息序列化持久化等操作 | `GSE.GSSdk/IFileOperationStrategy.cs` |
| `GSE.GSSdk.CSharpFileOperationStrategy` | class | `IFileOperationStrategy` | C# 标准 `System.IO` 文件操作实现 | `GSE.GSSdk/CSharpFileOperationStrategy.cs` |
| `GSE.GSSdk.UGSFileOperationStrategy` | class | `IFileOperationStrategy` | Unreal UGS (Unreal Game Service) 文件辅助操作实现，通过 `UGSFileHelper` API | `GSE.GSSdk/UGSFileOperationStrategy.cs` |
| `GSE.GSSdk.FileManager` | class | `object` | 文件管理器（策略模式包装器）。持有 `IFileOperationStrategy` 实例，统一提供文件/目录操作接口 | `GSE.GSSdk/FileManager.cs` |
| `GSE.GSSdk.ZipUtils` | class (static) | `object` | GZip 压缩/解压工具。提供 Base64 字符串与 byte 数组的压缩和解压 | `GSE.GSSdk/ZipUtils.cs` |
| `GSE.GSSdk.BucketCfg` | class | `object` | 采样桶配置。包含 key 名称与分桶边界数组，用于性能指标的桶分布统计 | `GSE.GSSdk/BucketCfg.cs` |

#### 数据模型(Raw)

| 完整类型名称 | 类型 | 继承关系 | 说明 | 文件路径 |
|-------------|------|---------|------|---------|
| `GSE.GSSdk.ReportEventItemRaw` | class | `object` | 原始上报事件项。包含 service/api/meth/compress 元信息及各类 Protobuf 请求体引用（TrackReq/MonitorReq/EventReq/UserReq/UpdateFilesReq） | `GSE.GSSdk/ReportEventItemRaw.cs` |
| `GSE.GSSdk.ReportEventWorkerParam` | class (internal) | `object` | 上报工作线程参数。包含取消令牌、本地存储开关、原始事件队列、HTTP Post 代理 | `GSE.GSSdk/ReportEventWorkerParam.cs` |

#### 枚举

| 完整类型名称 | 类型 | 说明 | 文件路径 |
|-------------|------|------|---------|
| `GSE.GSSdk.MonEvtCatogray` | enum | 监控事件分类：None, GSSDK, ARCHIVE, CSRPC, ERROR, NET, GAMESVR, OSS | `GSE.GSSdk/MonEvtCatogray.cs` |

#### 委托

| 完整类型名称 | 类型 | 签名 | 说明 | 文件路径 |
|-------------|------|------|------|---------|
| `GSE.GSSdk.GSSdkEnvironmentContextInitCallback` | delegate | `void()` | GSSDK 环境初始化完成回调 | `GSE.GSSdk/GSSdkEnvironmentContextInitCallback.cs` |
| `GSE.GSSdk.HttpRequestDelegate` | delegate | `bool(string url, string method, string[] headers, byte[] body, HttpResponseFinishNotify onRspNotify, int reqTimeoutSecond)` | HTTP 请求委托 | `GSE.GSSdk/HttpRequestDelegate.cs` |
| `GSE.GSSdk.HttpResponseFinishNotify` | delegate | `void(int HttpStatusCode, byte[] ResponseContent)` | HTTP 响应完成通知 | `GSE.GSSdk/HttpResponseFinishNotify.cs` |
| `GSE.GSSdk.HttpPostAsyncProxy` | delegate | `bool(string RequestUrl, string[] Headers, byte[] PostData)` | 异步 HTTP POST 代理 | `GSE.GSSdk/HttpPostAsyncProxy.cs` |
| `GSE.GSSdk.OnBytesResponse` | delegate | `void(int Code, string ErrorMsg, byte[] RetBody)` | 字节数据响应回调 | `GSE.GSSdk/OnBytesResponse.cs` |
| `GSE.GSSdk.OnJsonResponse<T>` | delegate | `void(int Code, string ErrorMsg, T RetObj)` | JSON 泛型响应回调 | `GSE.GSSdk/OnJsonResponse.cs` |
| `GSE.GSSdk.OnProtobufResponse<T>` | delegate | `void(int Code, string ErrorMsg, T RetObj) where T : IMessage, new()` | Protobuf 泛型响应回调 | `GSE.GSSdk/OnProtobufResponse.cs` |

---

### 命名空间 `Gssdk`

所有类型均为 Protobuf 编译器自动生成的 `sealed class`，均实现 `IMessage<T>`, `IMessage`, `IEquatable<T>`, `IDeepCloneable<T>`。

#### 顶层配置

| 完整类型名称 | 类型 | 说明 | 文件路径 |
|-------------|------|------|---------|
| `Gssdk.GssdkConfig` | sealed class | GSSDK 顶级配置，聚合 Auth/Dir/Report/Version 四个子配置 | `GSE.GSSdk/Gssdk/GssdkConfig.cs` |
| `Gssdk.GssdkAuthConfig` | sealed class | 认证模块配置：AES密钥、MD5盐值、Token过期时间、私钥文件、Steam/XBL/Wegame 渠道配置、集群 ID、加密开关 | `GSE.GSSdk/Gssdk/GssdkAuthConfig.cs` |
| `Gssdk.GssdkDirConfig` | sealed class | 目录服务配置：Oaddrs（OAuth 地址） | `GSE.GSSdk/Gssdk/GssdkDirConfig.cs` |
| `Gssdk.GssdkReportConfig` | sealed class | 上报模块配置：Key 前缀、MQ 命名空间、事件前缀、集群 ID、监控存储开关 | `GSE.GSSdk/Gssdk/GssdkReportConfig.cs` |
| `Gssdk.GssdkVersionConfig` | sealed class | 版本模块配置：CDN 列表、环境/分支/平台/渠道过滤、加密开关 | `GSE.GSSdk/Gssdk/GssdkVersionConfig.cs` |

#### 子配置
| 完整类型名称 | 类型 | 说明 | 文件路径 |
|-------------|------|------|---------|
| `Gssdk.GssdkCdnUnit` | sealed class | CDN 单元：名称、URL列表、Patch URL、VerJson URL | `GSE.GSSdk/Gssdk/GssdkCdnUnit.cs` |
| `Gssdk.GssdkChannelAuthSteamConfig` | sealed class | Steam 渠道认证配置 | `GSE.GSSdk/Gssdk/GssdkChannelAuthSteamConfig.cs` |
| `Gssdk.GssdkChannelAuthWegameConfig` | sealed class | WeGame 渠道认证配置 | `GSE.GSSdk/Gssdk/GssdkChannelAuthWegameConfig.cs` |
| `Gssdk.GssdkChannelAuthXblConfig` | sealed class | Xbox Live 渠道认证配置 | `GSE.GSSdk/Gssdk/GssdkChannelAuthXblConfig.cs` |

#### 实体/数据模型

| 完整类型名称 | 类型 | 说明 | 文件路径 |
|-------------|------|------|---------|
| `Gssdk.UserInfo` | sealed class | 用户信息聚合：设备信息、认证信息、客户端设置、游戏会话 | `GSE.GSSdk/Gssdk/UserInfo.cs` |
| `Gssdk.UserDevice` | sealed class | 用户设备信息：DeviceId/Mac/分辨率/OS/内存/CPU/GPU/硬盘/BIOS 等硬件信息 | `GSE.GSSdk/Gssdk/UserDevice.cs` |
| `Gssdk.UserAuthInfo` | sealed class | 用户认证信息：登录渠道与子渠道、渠道 UID、Aid/Roleid、SDK 扩展 | `GSE.GSSdk/Gssdk/UserAuthInfo.cs` |
| `Gssdk.UserClientSetting` | sealed class | 客户端设置：App渠道/版本、构建信息、语言/地区、时区、游玩模式、环境、图形 API | `GSE.GSSdk/Gssdk/UserClientSetting.cs` |
| `Gssdk.UserGameSession` | sealed class | 游戏会话信息：Session UUID、启动时间戳、登录 ID、机器 ID、进程 ID、引擎模式、崩溃 GUID、分辨率 | `GSE.GSSdk/Gssdk/UserGameSession.cs` |
| `Gssdk.ReportUserKey` | sealed class | 用户上报标识键：DeviceId/Aid/Roleid/SessionUuid/ShareArchiveUuid/Env/DevBranch | `GSE.GSSdk/Gssdk/ReportUserKey.cs` |
| `Gssdk.ServerInfo` | sealed class | 游戏服务器信息：ServerId/Name/Version/Load/Level/Addr/Flag/Oaddr/Alias/Sort/Isopen | `GSE.GSSdk/Gssdk/ServerInfo.cs` |
| `Gssdk.ServerUnit` | sealed class | 服务器单元（目录列表用）：ServerId/Name/Version/Load/Level/Addr/Sort | `GSE.GSSdk/Gssdk/ServerUnit.cs` |
| `Gssdk.GssdkBlackList` | sealed class | 黑名单条目：Id/Type/DeviceId/ChannelUid/Aid/Roleid | `GSE.GSSdk/Gssdk/GssdkBlackList.cs` |
| `Gssdk.GssdkWhiteList` | sealed class | 白名单条目：Id/Type/DeviceId/ChannelUid/Aid | `GSE.GSSdk/Gssdk/GssdkWhiteList.cs` |
| `Gssdk.GssdkReviewRec` | sealed class | 审核记录：AppChannel/SubChannel/Version/Status/Desc | `GSE.GSSdk/Gssdk/GssdkReviewRec.cs` |
| `Gssdk.GssdkCustomJsonConfig` | sealed class | 自定义 JSON 配置项：Id/Env/Branch/CfgType/Type/Value/Channel/Cfg/创建/修改信息 | `GSE.GSSdk/Gssdk/GssdkCustomJsonConfig.cs` |
| `Gssdk.GssdkRet` | sealed class | 通用返回结果：Code(错误码)/SubCode(子码)/Msg(消息) | `GSE.GSSdk/Gssdk/GssdkRet.cs` |
| `Gssdk.VersionConfigRec` | sealed class | 版本配置记录：Id/Config/Ctime | `GSE.GSSdk/Gssdk/VersionConfigRec.cs` |
| `Gssdk.VersionPatchRec` | sealed class | 版本补丁记录（旧格式）：版本/MD5/Tag/Env/Plist/Status 等 | `GSE.GSSdk/Gssdk/VersionPatchRec.cs` |
| `Gssdk.VersionPatchRecNew` | sealed class | 版本补丁记录（新格式）：含 PatchFile/Size 字段，增强版 | `GSE.GSSdk/Gssdk/VersionPatchRecNew.cs` |
| `Gssdk.VersionPatchCfg` | sealed class | 版本补丁配置：Tag/VerCurrent/Md5/VerBase | `GSE.GSSdk/Gssdk/VersionPatchCfg.cs` |
| `Gssdk.ReportEventItem` | sealed class | 上报事件项（队列持久化用）：Service/Api/Meth/Compress/Data(Bytes) | `GSE.GSSdk/Gssdk/ReportEventItem.cs` |
| `Gssdk.ReportTrack` | sealed class | 跟踪事件：EvtName/EvtContent/Time/RoleEx | `GSE.GSSdk/Gssdk/ReportTrack.cs` |
| `Gssdk.ReportTrackMisc` | sealed class | 跟踪杂项数据 | `GSE.GSSdk/Gssdk/ReportTrackMisc.cs` |
| `Gssdk.ReportMonitorKeyVal` | sealed class | 监控键值对 | `GSE.GSSdk/Gssdk/ReportMonitorKeyVal.cs` |
| `Gssdk.ReportMonitorAdd` | sealed class | 监控累计计数项 | `GSE.GSSdk/Gssdk/ReportMonitorAdd.cs` |
| `Gssdk.ReportMonitorSet` | sealed class | 监控采样设置项（含 Sum/Min/Max/Num） | `GSE.GSSdk/Gssdk/ReportMonitorSet.cs` |
| `Gssdk.ReportUploadFileOne` | sealed class | 单个文件上传项：KeyName/ValContent(ByteString) | `GSE.GSSdk/Gssdk/ReportUploadFileOne.cs` |
| `Gssdk.ReportUserEvent` | sealed class | 用户自定义事件 | `GSE.GSSdk/Gssdk/ReportUserEvent.cs` |
| `Gssdk.ReportLogEventLine` | sealed class | 日志事件行 | `GSE.GSSdk/Gssdk/ReportLogEventLine.cs` |
| `Gssdk.ReportLogEventSession` | sealed class | 日志事件会话 | `GSE.GSSdk/Gssdk/ReportLogEventSession.cs` |
| `Gssdk.ReportLogNotifyUnit` | sealed class | 日志通知单元 | `GSE.GSSdk/Gssdk/ReportLogNotifyUnit.cs` |
| `Gssdk.DevReportConfig` | sealed class | 开发者上报配置 | `GSE.GSSdk/Gssdk/DevReportConfig.cs` |
| `Gssdk.DevReportData` | sealed class | 开发者上报数据 | `GSE.GSSdk/Gssdk/DevReportData.cs` |
| `Gssdk.DevReportDataFull` | sealed class | 开发者完整上报数据 | `GSE.GSSdk/Gssdk/DevReportDataFull.cs` |
| `Gssdk.DevResData` | sealed class | 开发者资源数据 | `GSE.GSSdk/Gssdk/DevResData.cs` |
| `Gssdk.ByteDanceVerify` | sealed class | 字节跳动渠道验证 | `GSE.GSSdk/Gssdk/ByteDanceVerify.cs` |
| `Gssdk.AccountBinding` | sealed class | 账户绑定信息 | `GSE.GSSdk/Gssdk/AccountBinding.cs` |
| `Gssdk.AccountLogin` | sealed class | 账户登录信息 | `GSE.GSSdk/Gssdk/AccountLogin.cs` |
| `Gssdk.AccountProfile` | sealed class | 账户资料 | `GSE.GSSdk/Gssdk/AccountProfile.cs` |
| `Gssdk.AccountUser` | sealed class | 账户用户 | `GSE.GSSdk/Gssdk/AccountUser.cs` |
| `Gssdk.UserAuthResult` | sealed class | 用户认证结果 | `GSE.GSSdk/Gssdk/UserAuthResult.cs` |
| `Gssdk.AuthUserChannelVerify` | sealed class | 认证用户渠道验证 | `GSE.GSSdk/Gssdk/AuthUserChannelVerify.cs` |
| `Gssdk.VersionCheckConfigUserInfo` | sealed class | 版本检查配置中的用户信息 | `GSE.GSSdk/Gssdk/VersionCheckConfigUserInfo.cs` |

#### 请求/响应类型

按服务模块分组：

**Auth 认证服务**

| 完整类型名称 | 类型 | 说明 | 文件路径 |
|-------------|------|------|---------|
| `Gssdk.AuthLoginDirReq` | sealed class | 认证-登录目录请求 | `GSE.GSSdk/Gssdk/AuthLoginDirReq.cs` |
| `Gssdk.AuthLoginDirRes` | sealed class | 认证-登录目录响应 | `GSE.GSSdk/Gssdk/AuthLoginDirRes.cs` |
| `Gssdk.AuthLoginReq` | sealed class | 认证-登录请求 | `GSE.GSSdk/Gssdk/AuthLoginReq.cs` |
| `Gssdk.AuthLoginRes` | sealed class | 认证-登录响应 | `GSE.GSSdk/Gssdk/AuthLoginRes.cs` |
| `Gssdk.AuthBindReq` | sealed class | 认证-绑定请求 | `GSE.GSSdk/Gssdk/AuthBindReq.cs` |
| `Gssdk.AuthBindRes` | sealed class | 认证-绑定响应 | `GSE.GSSdk/Gssdk/AuthBindRes.cs` |
| `Gssdk.AuthForceBindReq` | sealed class | 认证-强制绑定请求 | `GSE.GSSdk/Gssdk/AuthForceBindReq.cs` |
| `Gssdk.AuthForceBindRes` | sealed class | 认证-强制绑定响应 | `GSE.GSSdk/Gssdk/AuthForceBindRes.cs` |
| `Gssdk.AuthUnbindReq` | sealed class | 认证-解绑请求 | `GSE.GSSdk/Gssdk/AuthUnbindReq.cs` |
| `Gssdk.AuthUnbindRes` | sealed class | 认证-解绑响应 | `GSE.GSSdk/Gssdk/AuthUnbindRes.cs` |
| `Gssdk.AuthGetAllReq` | sealed class | 认证-获取全部请求 | `GSE.GSSdk/Gssdk/AuthGetAllReq.cs` |
| `Gssdk.AuthGetAllRes` | sealed class | 认证-获取全部响应 | `GSE.GSSdk/Gssdk/AuthGetAllRes.cs` |
| `Gssdk.AuthGetOneReq` | sealed class | 认证-获取单条请求 | `GSE.GSSdk/Gssdk/AuthGetOneReq.cs` |
| `Gssdk.AuthGetOneRes` | sealed class | 认证-获取单条响应 | `GSE.GSSdk/Gssdk/AuthGetOneRes.cs` |
| `Gssdk.AuthInsertOneReq` | sealed class | 认证-插入单条请求 | `GSE.GSSdk/Gssdk/AuthInsertOneReq.cs` |
| `Gssdk.AuthInsertOneRes` | sealed class | 认证-插入单条响应 | `GSE.GSSdk/Gssdk/AuthInsertOneRes.cs` |
| `Gssdk.AuthUpdateOneReq` | sealed class | 认证-更新单条请求 | `GSE.GSSdk/Gssdk/AuthUpdateOneReq.cs` |
| `Gssdk.AuthUpdateOneRes` | sealed class | 认证-更新单条响应 | `GSE.GSSdk/Gssdk/AuthUpdateOneRes.cs` |
| `Gssdk.AuthGetUserInfoReq` | sealed class | 认证-获取用户信息请求 | `GSE.GSSdk/Gssdk/AuthGetUserInfoReq.cs` |
| `Gssdk.AuthGetUserInfoRes` | sealed class | 认证-获取用户信息响应 | `GSE.GSSdk/Gssdk/AuthGetUserInfoRes.cs` |
| `Gssdk.AuthGetCustomCfgReq` | sealed class | 认证-获取自定义配置请求 | `GSE.GSSdk/Gssdk/AuthGetCustomCfgReq.cs` |
| `Gssdk.AuthGetCustomCfgRes` | sealed class | 认证-获取自定义配置响应 | `GSE.GSSdk/Gssdk/AuthGetCustomCfgRes.cs` |
| `Gssdk.AuthOpUserJsonConfigReq` | sealed class | 认证-操作用户 JSON 配置请求 | `GSE.GSSdk/Gssdk/AuthOpUserJsonConfigReq.cs` |
| `Gssdk.AuthOpUserJsonConfigRes` | sealed class | 认证-操作用户 JSON 配置响应 | `GSE.GSSdk/Gssdk/AuthOpUserJsonConfigRes.cs` |
| `Gssdk.AuthSetUserJsonConfigReq` | sealed class | 认证-设置用户 JSON 配置请求 | `GSE.GSSdk/Gssdk/AuthSetUserJsonConfigReq.cs` |
| `Gssdk.AuthSetUserJsonConfigRes` | sealed class | 认证-设置用户 JSON 配置响应 | `GSE.GSSdk/Gssdk/AuthSetUserJsonConfigRes.cs` |

**Dir 目录服务**

| 完整类型名称 | 类型 | 说明 | 文件路径 |
|-------------|------|------|---------|
| `Gssdk.DirListReq` | sealed class | 目录-列表请求 | `GSE.GSSdk/Gssdk/DirListReq.cs` |
| `Gssdk.DirListRes` | sealed class | 目录-列表响应 | `GSE.GSSdk/Gssdk/DirListRes.cs` |
| `Gssdk.DirGetAllReq` | sealed class | 目录-获取所有请求 | `GSE.GSSdk/Gssdk/DirGetAllReq.cs` |
| `Gssdk.DirGetAllRes` | sealed class | 目录-获取所有响应 | `GSE.GSSdk/Gssdk/DirGetAllRes.cs` |
| `Gssdk.DirGetOneReq` | sealed class | 目录-获取单条请求 | `GSE.GSSdk/Gssdk/DirGetOneReq.cs` |
| `Gssdk.DirGetOneRes` | sealed class | 目录-获取单条响应 | `GSE.GSSdk/Gssdk/DirGetOneRes.cs` |
| `Gssdk.DirInsertOneReq` | sealed class | 目录-插入单条请求 | `GSE.GSSdk/Gssdk/DirInsertOneReq.cs` |
| `Gssdk.DirInsertOneRes` | sealed class | 目录-插入单条响应 | `GSE.GSSdk/Gssdk/DirInsertOneRes.cs` |
| `Gssdk.DirUpdateOneReq` | sealed class | 目录-更新单条请求 | `GSE.GSSdk/Gssdk/DirUpdateOneReq.cs` |
| `Gssdk.DirUpdateOneRes` | sealed class | 目录-更新单条响应 | `GSE.GSSdk/Gssdk/DirUpdateOneRes.cs` |
| `Gssdk.DirReportReq` | sealed class | 目录-报表请求 | `GSE.GSSdk/Gssdk/DirReportReq.cs` |
| `Gssdk.DirReportRes` | sealed class | 目录-报表响应 | `GSE.GSSdk/Gssdk/DirReportRes.cs` |
| `Gssdk.LoadCfgReq` | sealed class | 目录-加载配置请求 | `GSE.GSSdk/Gssdk/LoadCfgReq.cs` |
| `Gssdk.LoadCfgRes` | sealed class | 目录-加载配置响应 | `GSE.GSSdk/Gssdk/LoadCfgRes.cs` |
| `Gssdk.ReloadCfgReq` | sealed class | 目录-重新加载配置请求 | `GSE.GSSdk/Gssdk/ReloadCfgReq.cs` |
| `Gssdk.ReloadCfgRes` | sealed class | 目录-重新加载配置响应 | `GSE.GSSdk/Gssdk/ReloadCfgRes.cs` |

**Report 上报服务**

| 完整类型名称 | 类型 | 说明 | 文件路径 |
|-------------|------|------|---------|
| `Gssdk.ReportUserReq` | sealed class | 上报-用户请求 | `GSE.GSSdk/Gssdk/ReportUserReq.cs` |
| `Gssdk.ReportUserRes` | sealed class | 上报-用户响应 | `GSE.GSSdk/Gssdk/ReportUserRes.cs` |
| `Gssdk.ReportEventReq` | sealed class | 上报-事件请求 | `GSE.GSSdk/Gssdk/ReportEventReq.cs` |
| `Gssdk.ReportEventRes` | sealed class | 上报-事件响应 | `GSE.GSSdk/Gssdk/ReportEventRes.cs` |
| `Gssdk.ReportMonitorReq` | sealed class | 上报-监控请求 | `GSE.GSSdk/Gssdk/ReportMonitorReq.cs` |
| `Gssdk.ReportMonitorRes` | sealed class | 上报-监控响应 | `GSE.GSSdk/Gssdk/ReportMonitorRes.cs` |
| `Gssdk.ReportTrackReq` | sealed class | 上报-跟踪请求 | `GSE.GSSdk/Gssdk/ReportTrackReq.cs` |
| `Gssdk.ReportTrackRes` | sealed class | 上报-跟踪响应 | `GSE.GSSdk/Gssdk/ReportTrackRes.cs` |
| `Gssdk.ReportUploadFilesReq` | sealed class | 上报-上传文件请求 | `GSE.GSSdk/Gssdk/ReportUploadFilesReq.cs` |
| `Gssdk.ReportUploadFilesRes` | sealed class | 上报-上传文件响应 | `GSE.GSSdk/Gssdk/ReportUploadFilesRes.cs` |

**DevReport 开发者上报服务**

| 完整类型名称 | 类型 | 说明 | 文件路径 |
|-------------|------|------|---------|
| `Gssdk.ReportLogEventReq` | sealed class | 开发者-日志事件请求 | `GSE.GSSdk/Gssdk/ReportLogEventReq.cs` |
| `Gssdk.ReportLogEventRes` | sealed class | 开发者-日志事件响应 | `GSE.GSSdk/Gssdk/ReportLogEventRes.cs` |
| `Gssdk.ReportLogEventUploadReq` | sealed class | 开发者-日志事件上传请求 | `GSE.GSSdk/Gssdk/ReportLogEventUploadReq.cs` |
| `Gssdk.ReportLogEventUploadRes` | sealed class | 开发者-日志事件上传响应 | `GSE.GSSdk/Gssdk/ReportLogEventUploadRes.cs` |

**ReportView 日志查看服务**

| 完整类型名称 | 类型 | 说明 | 文件路径 |
|-------------|------|------|---------|
| `Gssdk.ReportLogQueryReq` | sealed class | 日志-查询请求 | `GSE.GSSdk/Gssdk/ReportLogQueryReq.cs` |
| `Gssdk.ReportLogQueryRes` | sealed class | 日志-查询响应 | `GSE.GSSdk/Gssdk/ReportLogQueryRes.cs` |
| `Gssdk.ReportLogDownloadReq` | sealed class | 日志-下载请求 | `GSE.GSSdk/Gssdk/ReportLogDownloadReq.cs` |
| `Gssdk.ReportLogDownloadRes` | sealed class | 日志-下载响应 | `GSE.GSSdk/Gssdk/ReportLogDownloadRes.cs` |
| `Gssdk.ReportLogNotifyReq` | sealed class | 日志-通知请求 | `GSE.GSSdk/Gssdk/ReportLogNotifyReq.cs` |
| `Gssdk.ReportLogNotifyRes` | sealed class | 日志-通知响应 | `GSE.GSSdk/Gssdk/ReportLogNotifyRes.cs` |

**Version 版本服务**

| 完整类型名称 | 类型 | 说明 | 文件路径 |
|-------------|------|------|---------|
| `Gssdk.VersionCheckConfigReq` | sealed class | 版本-检查配置请求 | `GSE.GSSdk/Gssdk/VersionCheckConfigReq.cs` |
| `Gssdk.VersionCheckConfigRes` | sealed class | 版本-检查配置响应 | `GSE.GSSdk/Gssdk/VersionCheckConfigRes.cs` |
| `Gssdk.VersionGetCustomCfgReq` | sealed class | 版本-获取自定义配置请求 | `GSE.GSSdk/Gssdk/VersionGetCustomCfgReq.cs` |
| `Gssdk.VersionGetCustomCfgRes` | sealed class | 版本-获取自定义配置响应 | `GSE.GSSdk/Gssdk/VersionGetCustomCfgRes.cs` |
| `Gssdk.VersionTestCheckConfigReq` | sealed class | 版本-测试检查配置请求 | `GSE.GSSdk/Gssdk/VersionTestCheckConfigReq.cs` |
| `Gssdk.VersionTestCheckConfigRes` | sealed class | 版本-测试检查配置响应 | `GSE.GSSdk/Gssdk/VersionTestCheckConfigRes.cs` |
| `Gssdk.VersionOpServerJsonConfigReq` | sealed class | 版本-操作服务器 JSON 配置请求 | `GSE.GSSdk/Gssdk/VersionOpServerJsonConfigReq.cs` |
| `Gssdk.VersionOpServerJsonConfigRes` | sealed class | 版本-操作服务器 JSON 配置响应 | `GSE.GSSdk/Gssdk/VersionOpServerJsonConfigRes.cs` |
| `Gssdk.VersionSetServerJsonConfigReq` | sealed class | 版本-设置服务器 JSON 配置请求 | `GSE.GSSdk/Gssdk/VersionSetServerJsonConfigReq.cs` |
| `Gssdk.VersionSetServerJsonConfigRes` | sealed class | 版本-设置服务器 JSON 配置响应 | `GSE.GSSdk/Gssdk/VersionSetServerJsonConfigRes.cs` |
| `Gssdk.VersionUpdateConfigReq` | sealed class | 版本-更新配置请求 | `GSE.GSSdk/Gssdk/VersionUpdateConfigReq.cs` |
| `Gssdk.VersionUpdateConfigRes` | sealed class | 版本-更新配置响应 | `GSE.GSSdk/Gssdk/VersionUpdateConfigRes.cs` |
| `Gssdk.VersionAddPatchReq` | sealed class | 版本-添加补丁请求 | `GSE.GSSdk/Gssdk/VersionAddPatchReq.cs` |
| `Gssdk.VersionAddPatchRes` | sealed class | 版本-添加补丁响应 | `GSE.GSSdk/Gssdk/VersionAddPatchRes.cs` |
| `Gssdk.VersionChangePatchReq` | sealed class | 版本-变更补丁请求 | `GSE.GSSdk/Gssdk/VersionChangePatchReq.cs` |
| `Gssdk.VersionChangePatchRes` | sealed class | 版本-变更补丁响应 | `GSE.GSSdk/Gssdk/VersionChangePatchRes.cs` |
| `Gssdk.VersionGetPatchListReq` | sealed class | 版本-获取补丁列表请求 | `GSE.GSSdk/Gssdk/VersionGetPatchListReq.cs` |
| `Gssdk.VersionGetPatchListRes` | sealed class | 版本-获取补丁列表响应 | `GSE.GSSdk/Gssdk/VersionGetPatchListRes.cs` |
| `Gssdk.VersionGetPlistContentReq` | sealed class | 版本-获取 Plist 内容请求 | `GSE.GSSdk/Gssdk/VersionGetPlistContentReq.cs` |
| `Gssdk.VersionGetPlistContentRes` | sealed class | 版本-获取 Plist 内容响应 | `GSE.GSSdk/Gssdk/VersionGetPlistContentRes.cs` |
| `Gssdk.VersionBlackListReq` | sealed class | 版本-黑名单请求 | `GSE.GSSdk/Gssdk/VersionBlackListReq.cs` |
| `Gssdk.VersionBlackListRes` | sealed class | 版本-黑名单响应 | `GSE.GSSdk/Gssdk/VersionBlackListRes.cs` |
| `Gssdk.VersionWhiteListReq` | sealed class | 版本-白名单请求 | `GSE.GSSdk/Gssdk/VersionWhiteListReq.cs` |
| `Gssdk.VersionWhiteListRes` | sealed class | 版本-白名单响应 | `GSE.GSSdk/Gssdk/VersionWhiteListRes.cs` |
| `Gssdk.VersionReviewRecReq` | sealed class | 版本-审核记录请求 | `GSE.GSSdk/Gssdk/VersionReviewRecReq.cs` |
| `Gssdk.VersionReviewRecRes` | sealed class | 版本-审核记录响应 | `GSE.GSSdk/Gssdk/VersionReviewRecRes.cs` |

**DevReport 开发者配置**

| 完整类型名称 | 类型 | 说明 | 文件路径 |
|-------------|------|------|---------|
| `Gssdk.DevReportConfig` | sealed class | 开发上报配置 | `GSE.GSSdk/Gssdk/DevReportConfig.cs` |
| `Gssdk.DevReportData` | sealed class | 开发上报数据 | `GSE.GSSdk/Gssdk/DevReportData.cs` |
| `Gssdk.DevReportDataFull` | sealed class | 开发上报完整数据 | `GSE.GSSdk/Gssdk/DevReportDataFull.cs` |
| `Gssdk.DevResData` | sealed class | 开发资源数据 | `GSE.GSSdk/Gssdk/DevResData.cs` |

#### 枚举

| 完整类型名称 | 类型 | 说明 | 文件路径 |
|-------------|------|------|---------|
| `Gssdk.GssdkErrCode` | enum | GSSDK 错误码枚举：Success, InvalidParam(1001), AuthFail(1101), Order(1201), CheatingPaymentDevice(133) 等 | `GSE.GSSdk/Gssdk/GssdkErrCode.cs` |
| `Gssdk.GssdkBlackListType` | enum | 黑名单类型：Logindir | `GSE.GSSdk/Gssdk/GssdkBlackListType.cs` |
| `Gssdk.GssdkWhiteListType` | enum | 白名单类型：Logindir, ResUpdate | `GSE.GSSdk/Gssdk/GssdkWhiteListType.cs` |
| `Gssdk.GssdkReviewStatus` | enum | 审核状态：Normal, InReview | `GSE.GSSdk/Gssdk/GssdkReviewStatus.cs` |
| `Gssdk.GssdkJsonConfigType` | enum | JSON 配置类型：GcctGlobal, GcctPlatform, GcctUser, GcctMachine | `GSE.GSSdk/Gssdk/GssdkJsonConfigType.cs` |
| `Gssdk.GssdkJsonConfigDataType` | enum | JSON 配置数据类型：Default, GccdtServerBase, GccdtServerSpec, GccdtUserBase, GccdtUserSpec, GccdtExJson | `GSE.GSSdk/Gssdk/GssdkJsonConfigDataType.cs` |
| `Gssdk.OpServerJsonConfigType` | enum | 操作服务器 JSON 配置类型 | `GSE.GSSdk/Gssdk/OpServerJsonConfigType.cs` |
| `Gssdk.OpUserJsonConfigType` | enum | 操作用户 JSON 配置类型 | `GSE.GSSdk/Gssdk/OpUserJsonConfigType.cs` |
| `Gssdk.ReportEvtType` | enum | 上报事件类型：CustomName, LaunchGame, VersionCheckConf, ChannelLogin, LoginDir | `GSE.GSSdk/Gssdk/ReportEvtType.cs` |
| `Gssdk.ReprotMonitorAddType` | enum | 监控累加类型 | `GSE.GSSdk/Gssdk/ReprotMonitorAddType.cs` |
| `Gssdk.ReprotMonitorSetType` | enum | 监控设置类型 | `GSE.GSSdk/Gssdk/ReprotMonitorSetType.cs` |
| `Gssdk.VersionPatchStatus` | enum | 补丁状态：Disabled, Gray, Inuse | `GSE.GSSdk/Gssdk/VersionPatchStatus.cs` |

#### 枚举包装类型（ILRuntime）

以下类型用于 ILRuntime 环境中枚举值的 Protobuf 序列化包装：

| 完整类型名称 | 类型 | 封装对象 | 文件路径 |
|-------------|------|---------|---------|
| `Gssdk.GssdkErrCodeSyncWrapper` | sealed class | `GssdkErrCode` | `GSE.GSSdk/Gssdk/GssdkErrCodeSyncWrapper.cs` |
| `Gssdk.GssdkErrCodeTupleWrapper` | sealed class | `GssdkErrCode` x2 | `GSE.GSSdk/Gssdk/GssdkErrCodeTupleWrapper.cs` |
| `Gssdk.GssdkBlackListTypeSyncWrapper` | sealed class | `GssdkBlackListType` | `GSE.GSSdk/Gssdk/GssdkBlackListTypeSyncWrapper.cs` |
| `Gssdk.GssdkBlackListTypeTupleWrapper` | sealed class | `GssdkBlackListType` x2 | `GSE.GSSdk/Gssdk/GssdkBlackListTypeTupleWrapper.cs` |
| `Gssdk.GssdkWhiteListTypeSyncWrapper` | sealed class | `GssdkWhiteListType` | `GSE.GSSdk/Gssdk/GssdkWhiteListTypeSyncWrapper.cs` |
| `Gssdk.GssdkWhiteListTypeTupleWrapper` | sealed class | `GssdkWhiteListType` x2 | `GSE.GSSdk/Gssdk/GssdkWhiteListTypeTupleWrapper.cs` |
| `Gssdk.GssdkReviewStatusSyncWrapper` | sealed class | `GssdkReviewStatus` | `GSE.GSSdk/Gssdk/GssdkReviewStatusSyncWrapper.cs` |
| `Gssdk.GssdkReviewStatusTupleWrapper` | sealed class | `GssdkReviewStatus` x2 | `GSE.GSSdk/Gssdk/GssdkReviewStatusTupleWrapper.cs` |
| `Gssdk.GssdkJsonConfigTypeSyncWrapper` | sealed class | `GssdkJsonConfigType` | `GSE.GSSdk/Gssdk/GssdkJsonConfigTypeSyncWrapper.cs` |
| `Gssdk.GssdkJsonConfigTypeTupleWrapper` | sealed class | `GssdkJsonConfigType` x2 | `GSE.GSSdk/Gssdk/GssdkJsonConfigTypeTupleWrapper.cs` |
| `Gssdk.GssdkJsonConfigDataTypeSyncWrapper` | sealed class | `GssdkJsonConfigDataType` | `GSE.GSSdk/Gssdk/GssdkJsonConfigDataTypeSyncWrapper.cs` |
| `Gssdk.GssdkJsonConfigDataTypeTupleWrapper` | sealed class | `GssdkJsonConfigDataType` x2 | `GSE.GSSdk/Gssdk/GssdkJsonConfigDataTypeTupleWrapper.cs` |
| `Gssdk.ReportEvtTypeSyncWrapper` | sealed class | `ReportEvtType` | `GSE.GSSdk/Gssdk/ReportEvtTypeSyncWrapper.cs` |
| `Gssdk.ReportEvtTypeTupleWrapper` | sealed class | `ReportEvtType` x2 | `GSE.GSSdk/Gssdk/ReportEvtTypeTupleWrapper.cs` |
| `Gssdk.ReprotMonitorAddTypeSyncWrapper` | sealed class | `ReprotMonitorAddType` | `GSE.GSSdk/Gssdk/ReprotMonitorAddTypeSyncWrapper.cs` |
| `Gssdk.ReprotMonitorAddTypeTupleWrapper` | sealed class | `ReprotMonitorAddType` x2 | `GSE.GSSdk/Gssdk/ReprotMonitorAddTypeTupleWrapper.cs` |
| `Gssdk.ReprotMonitorSetTypeSyncWrapper` | sealed class | `ReprotMonitorSetType` | `GSE.GSSdk/Gssdk/ReprotMonitorSetTypeSyncWrapper.cs` |
| `Gssdk.ReprotMonitorSetTypeTupleWrapper` | sealed class | `ReprotMonitorSetType` x2 | `GSE.GSSdk/Gssdk/ReprotMonitorSetTypeTupleWrapper.cs` |
| `Gssdk.VersionPatchStatusSyncWrapper` | sealed class | `VersionPatchStatus` | `GSE.GSSdk/Gssdk/VersionPatchStatusSyncWrapper.cs` |
| `Gssdk.VersionPatchStatusTupleWrapper` | sealed class | `VersionPatchStatus` x2 | `GSE.GSSdk/Gssdk/VersionPatchStatusTupleWrapper.cs` |
| `Gssdk.OpServerJsonConfigTypeSyncWrapper` | sealed class | `OpServerJsonConfigType` | `GSE.GSSdk/Gssdk/OpServerJsonConfigTypeSyncWrapper.cs` |
| `Gssdk.OpServerJsonConfigTypeTupleWrapper` | sealed class | `OpServerJsonConfigType` x2 | `GSE.GSSdk/Gssdk/OpServerJsonConfigTypeTupleWrapper.cs` |
| `Gssdk.OpUserJsonConfigTypeSyncWrapper` | sealed class | `OpUserJsonConfigType` | `GSE.GSSdk/Gssdk/OpUserJsonConfigTypeSyncWrapper.cs` |
| `Gssdk.OpUserJsonConfigTypeTupleWrapper` | sealed class | `OpUserJsonConfigType` x2 | `GSE.GSSdk/Gssdk/OpUserJsonConfigTypeTupleWrapper.cs` |
