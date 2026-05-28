# GSE.GSNet 类导航

## 模块结构

```
GSE.GSNet/                      -- 网络通信核心模块
├── Properties/
│   └── AssemblyInfo.cs         -- 程序集元信息
├── Gs/                         -- Protobuf 消息协议定义（命名空间 Gs）
│   ├── UxNetMsg.cs             -- 顶层消息体
│   ├── UxNetMsgCmd.cs          -- 消息命令枚举
│   ├── UxNetMsgCmdSyncWrapper.cs
│   ├── UxNetMsgCmdTupleWrapper.cs
│   ├── UxNetAuthReq.cs         -- 认证请求
│   ├── UxNetAuthRes.cs         -- 认证响应
│   ├── UxNetAuthToken.cs       -- 认证令牌
│   ├── UxNetAuthResultType.cs  -- 认证结果枚举
│   ├── UxNetAuthResultTypeSyncWrapper.cs
│   ├── UxNetAuthResultTypeTupleWrapper.cs
│   ├── UxNetConnxEnv.cs        -- 客户端连接环境信息
│   ├── UxNetProxyHeader.cs     -- 代理头信息
│   ├── UxNetUdpAsk.cs          -- UDP 连接请求
│   ├── UxNetUdpAck.cs          -- UDP 连接确认
│   ├── UxNetWaitReq.cs         -- 排队等待请求
│   └── UxNetWaitRes.cs         -- 排队等待响应
├── *.cs                        -- 根目录：网络层核心实现
│   ├── NetDriver.cs            -- 网络驱动（连接管理与 IO 调度）
│   ├── NetConnection.cs        -- 单条网络连接
│   ├── NetChannel.cs           -- 客户端网络通道（连接+认证+消息收发）
│   ├── ReverseProxyChannel.cs  -- 反向代理通道（服务端代理）
│   ├── ReverseProxyState.cs    -- 反向代理内部状态
│   ├── ReverseProxyContext.cs  -- 反向代理配置上下文
│   ├── ProxyRoleServerUpstream.cs  -- 代理上游服务器连接
│   ├── ProxyRoleClientConnx.cs     -- 代理客户端连接
│   ├── ChannelContext.cs       -- 通道配置上下文
│   ├── ChannelState.cs         -- 通道运行时状态
│   ├── ListenerContext.cs      -- TCP 监听器上下文
│   ├── ILRuntimeBinding.cs     -- ILRuntime 热更委托绑定
│   ├── NetFrameHead.cs         -- 网络帧头结构
│   ├── NetMsgHandlerException.cs  -- 消息处理异常
│   ├── NetProtoType.cs         -- 网络协议类型枚举
│   ├── NetClosedReason.cs      -- 连接关闭原因枚举
│   ├── ChannelConnectState.cs  -- 通道连接状态枚举
│   ├── ConnctionState.cs       -- 连接状态枚举
│   ├── TunnelConnectState.cs   -- 隧道连接状态枚举
│   ├── OnConnectStatusEventCallback.cs   -- 连接状态回调委托
│   ├── OnClosedEventCallback.cs          -- 连接关闭回调委托
│   ├── OnReceiveEventCallback.cs         -- 数据接收回调委托
│   ├── OnNewClientEventCallback.cs       -- 新客户端回调委托
│   ├── OnClientClosedEventCallback.cs    -- 客户端关闭回调委托
│   └── OnClientReceiveEventCallback.cs   -- 客户端数据接收回调委托
```

该模块实现了一套基于 TCP/UDP/KCP 协议的自定义帧协议网络通信层，支持客户端直连和反向代理两种通信模式，使用 Protobuf（Google.Protobuf）作为消息序列化方案。

---

## 命名空间清单

| 命名空间 | 说明 |
|----------|------|
| `GSE.GSNet` | 网络通信核心实现，包含驱动、连接、通道、代理、异常、委托等 |
| `Gs` | Protobuf 消息协议模型，由 `.proto` 文件生成，定义 uxnet 协议的消息体 |

---

## 类/接口/结构/枚举清单

### GSE.GSNet 命名空间

| 完整类型名称 | 类型 | 继承关系 | 说明 | 所在文件 |
|---|---|---|---|---|
| `GSE.GSNet.NetDriver` | class | `object` | 网络驱动，管理所有连接的创建、监听、Tick 分发、关闭；支持 TCP/UDP/KCP 协议 | `NetDriver.cs` |
| `GSE.GSNet.NetConnection` | class | `object` | 单条网络连接抽象，包含 Socket、帧编解码、状态、错误信息、回调委托 | `NetConnection.cs` |
| `GSE.GSNet.NetChannel` | class | `object` | 客户端网络通道，封装连接、认证、消息收发、自动重连逻辑 | `NetChannel.cs` |
| `GSE.GSNet.NetChannel.ChannelNotifier` | class | `object` | NetChannel 的内部通知器类，持有多个事件回调委托字段 | `NetChannel.cs` |
| `GSE.GSNet.NetChannel.OnRecvMsg` | delegate | `MulticastDelegate` | 接收消息回调：`void(byte[] Buff)` | `NetChannel.cs` |
| `GSE.GSNet.NetChannel.OnClosed` | delegate | `MulticastDelegate` | 通道关闭回调：`void()` | `NetChannel.cs` |
| `GSE.GSNet.NetChannel.OnConnectFail` | delegate | `MulticastDelegate` | 连接失败回调：`void(int iRetryTimes, int iMaxRetryTimes)` | `NetChannel.cs` |
| `GSE.GSNet.NetChannel.OnConnectSuccess` | delegate | `MulticastDelegate` | 连接成功回调：`void()` | `NetChannel.cs` |
| `GSE.GSNet.NetChannel.OnAuthFail` | delegate | `MulticastDelegate` | 认证失败回调：`void(int code)` | `NetChannel.cs` |
| `GSE.GSNet.NetChannel.OnAuthSuccess` | delegate | `MulticastDelegate` | 认证成功回调：`void()` | `NetChannel.cs` |
| `GSE.GSNet.ReverseProxyChannel` | class | `object` | 反向代理通道（服务端），接收客户端连接、验证角色身份、中继消息到上游游戏服务器 | `ReverseProxyChannel.cs` |
| `GSE.GSNet.ReverseProxyChannel.OnClientMsgBuffRecvHandler` | delegate | `MulticastDelegate` | 客户端消息缓冲接收回调：`void(ulong roleid, byte[] buffer)` | `ReverseProxyChannel.cs` |
| `GSE.GSNet.ReverseProxyChannel.OnClientAuthSuccessHandler` | delegate | `MulticastDelegate` | 客户端认证成功回调：`void(ulong roleid)` | `ReverseProxyChannel.cs` |
| `GSE.GSNet.ReverseProxyState` | class | `object` | 反向代理的内部状态，包含驱动引用、绑定 URL、客户端连接列表和上游服务器列表（internal） | `ReverseProxyState.cs` |
| `GSE.GSNet.ReverseProxyContext` | struct | `ValueType` | 反向代理配置上下文，包含名称、URL、认证令牌、WorldId、登录环境、重试次数、超时秒数、uri | `ReverseProxyContext.cs` |
| `GSE.GSNet.ProxyRoleServerUpstream` | class | `object` | 代理上游服务器连接状态，包含服务器 NetConnection、认证 cookie、RoleId、会话令牌、委托事件 | `ProxyRoleServerUpstream.cs` |
| `GSE.GSNet.ProxyRoleServerUpstream.OnSererClientResponseHandler` | delegate | `MulticastDelegate` | 服务器客户端响应回调：`void(byte[] Buffer)` | `ProxyRoleServerUpstream.cs` |
| `GSE.GSNet.ProxyRoleServerUpstream.OnServerClosedHandler` | delegate | `MulticastDelegate` | 服务器连接关闭回调：`void()` | `ProxyRoleServerUpstream.cs` |
| `GSE.GSNet.ProxyRoleServerUpstream.OnServerConnectedHandler` | delegate | `MulticastDelegate` | 服务器连接成功回调：`void()` | `ProxyRoleServerUpstream.cs` |
| `GSE.GSNet.ProxyRoleClientConnx` | class | `object` | 代理客户端连接，包含客户端 NetConnection、上游引用、连接状态、状态变更时间和事件委托 | `ProxyRoleClientConnx.cs` |
| `GSE.GSNet.ProxyRoleClientConnx.OnClientClosedHandler` | delegate | `MulticastDelegate` | 客户端关闭回调：`void()` | `ProxyRoleClientConnx.cs` |
| `GSE.GSNet.ProxyRoleClientConnx.OnClientConnectedHandler` | delegate | `MulticastDelegate` | 客户端连接成功回调：`void()` | `ProxyRoleClientConnx.cs` |
| `GSE.GSNet.ChannelContext` | struct | `ValueType` | 通道启动配置上下文，包含名称、URL、认证令牌、代理标志、代理相关 ID、WorldId、登录环境、最大重试次数、超时秒数、uri、上游引用 | `ChannelContext.cs` |
| `GSE.GSNet.ChannelState` | struct | `ValueType` | 通道运行时状态，包含驱动、连接、已重试次数、连接状态枚举、状态变更时间、会话 roleId、会话密钥、正在连接标志（internal） | `ChannelState.cs` |
| `GSE.GSNet.ListenerContext` | class | `object` | TCP 监听器上下文，包含 TcpListener、最终监听地址、新客户端/接收/关闭回调、收发缓冲区大小、压缩阈值 | `ListenerContext.cs` |
| `GSE.GSNet.ILRuntimeBinding` | class | `object` | ILRuntime 热更环境绑定，注册委托转换器和 MethodDelegate，使热更 Dll 能访问网络层委托 | `ILRuntimeBinding.cs` |
| `GSE.GSNet.NetFrameHead` | struct | `ValueType` | 网络帧头结构，包含帧长度（frame_length）、魔数（magic）、标志位（flags） | `NetFrameHead.cs` |
| `GSE.GSNet.NetMsgHandlerException` | class | `Exception` | 消息处理异常封装，包装内部异常为统一异常类型，由 NetDriver 接收处理时抛出 | `NetMsgHandlerException.cs` |
| `GSE.GSNet.NetProtoType` | enum | `Enum` | 网络协议类型：`NET_PROTO_TCP`、`NET_PROTO_UDP`、`NET_PROTO_KCP` | `NetProtoType.cs` |
| `GSE.GSNet.NetClosedReason` | enum | `Enum` | 连接关闭原因：`NET_CLOSE_NONE`（-1）、`NET_CLOSE_PEER`、`NET_CLOSE_UNZIP`、`NET_CLOSE_SEND`、`NET_CLOSE_FRAME_MALFORMD`、`NET_CLOSE_RECEIVE` | `NetClosedReason.cs` |
| `GSE.GSNet.ChannelConnectState` | enum | `Enum` | 通道连接状态：`CHANNEL_STATE_CONNECT`、`CHANNEL_STATE_AUTH`、`CHANNEL_STATE_RELAY`、`CHANNEL_STATE_CLOSED` | `ChannelConnectState.cs` |
| `GSE.GSNet.ConnctionState` | enum | `Enum` | 连接状态（拼写保留源码）：`CONNECTION_CONNECTING`（1）、`CONNECTION_ESTABLISHED`、`CONNECTION_WAIT_CLOSE`、`CONNECTION_DESTROY` | `ConnctionState.cs` |
| `GSE.GSNet.TunnelConnectState` | enum | `Enum` | 隧道（代理通道）连接状态：`CHANNEL_STATE_AUTH`、`CHANNEL_STATE_AUTH_SUCCESS_WAIT_SERVER`、`CHANNEL_STATE_RELAY`、`CHANNEL_STATE_CLOSED` | `TunnelConnectState.cs` |
| `GSE.GSNet.OnConnectStatusEventCallback` | delegate | `MulticastDelegate` | 连接状态回调：`void(NetConnection connx, bool bSuccess, string Error)` | `OnConnectStatusEventCallback.cs` |
| `GSE.GSNet.OnClosedEventCallback` | delegate | `MulticastDelegate` | 连接关闭回调：`void()` | `OnClosedEventCallback.cs` |
| `GSE.GSNet.OnReceiveEventCallback` | delegate | `MulticastDelegate` | 数据接收回调：`void(byte[] Buffer, int Length)` | `OnReceiveEventCallback.cs` |
| `GSE.GSNet.OnNewClientEventCallback` | delegate | `MulticastDelegate` | 新客户端接入回调：`void(NetConnection connx)` | `OnNewClientEventCallback.cs` |
| `GSE.GSNet.OnClientClosedEventCallback` | delegate | `MulticastDelegate` | 客户端连接关闭回调：`void(NetConnection connx)` | `OnClientClosedEventCallback.cs` |
| `GSE.GSNet.OnClientReceiveEventCallback` | delegate | `MulticastDelegate` | 客户端数据接收回调：`void(NetConnection connx, byte[] Buffer, int Length)` | `OnClientReceiveEventCallback.cs` |

---

### Gs 命名空间

| 完整类型名称 | 类型 | 继承关系 | 说明 | 所在文件 |
|---|---|---|---|---|
| `Gs.UxNetMsg` | class | `object`, `IMessage<UxNetMsg>`, `IMessage`, `IEquatable<UxNetMsg>`, `IDeepCloneable<UxNetMsg>` | 顶层 Protobuf 消息，包含命令类型 Cmd、Payload 数据负载、以及可选子消息（UdpAsk/UdpAck/AuthReq/AuthRes/AuthToken） | `Gs/UxNetMsg.cs` |
| `Gs.UxNetMsgCmd` | enum | `Enum` | 消息命令枚举：`Payload`、`UdpAsk`、`UdpAck`、`AuthReq`、`AuthRes`、`WaitReq`、`WaitRes` | `Gs/UxNetMsgCmd.cs` |
| `Gs.UxNetMsgCmdSyncWrapper` | class | `object`, `IMessage<...>`, `IEquatable<...>`, `IDeepCloneable<...>` | Protobuf 消息，包装 `UxNetMsgCmd` 与同步标志（SyncFlag/SyncIdx），用于同步协议场景 | `Gs/UxNetMsgCmdSyncWrapper.cs` |
| `Gs.UxNetMsgCmdTupleWrapper` | class | `object`, `IMessage<...>`, `IEquatable<...>`, `IDeepCloneable<...>` | Protobuf 消息，包装两个 `UxNetMsgCmd` 值的元组（Item1/Item2） | `Gs/UxNetMsgCmdTupleWrapper.cs` |
| `Gs.UxNetAuthReq` | class | `object`, `IMessage<UxNetAuthReq>`, `IMessage`, `IEquatable<UxNetAuthReq>`, `IDeepCloneable<UxNetAuthReq>` | 认证请求消息，包含 Token、EnvPacked（登录环境）、WorldId、Proxy（代理头） | `Gs/UxNetAuthReq.cs` |
| `Gs.UxNetAuthRes` | class | `object`, `IMessage<UxNetAuthRes>`, `IMessage`, `IEquatable<UxNetAuthRes>`, `IDeepCloneable<UxNetAuthRes>` | 认证响应消息，包含 Ret（结果类型）、SubCode（子错误码）、Token（会话令牌）、RoleId（角色 ID） | `Gs/UxNetAuthRes.cs` |
| `Gs.UxNetAuthToken` | class | `object`, `IMessage<UxNetAuthToken>`, `IMessage`, `IEquatable<UxNetAuthToken>`, `IDeepCloneable<UxNetAuthToken>` | 认证令牌消息，包含 SessionMagic、SessionKey、ProxyAuthCookie、NetioAuthCookie | `Gs/UxNetAuthToken.cs` |
| `Gs.UxNetAuthResultType` | enum | `Enum` | 认证结果枚举：`UxnetAuthRetSuccess`、`UxnetAuthRetTokenError`、`UxnetAuthRetVerifyFail`、`UxnetAuthRetServerBusy`、`UxnetAuthRetWaitQueue`、`UxnetAuthRetSyncError`、`UxnetAuthRetInvalParam` | `Gs/UxNetAuthResultType.cs` |
| `Gs.UxNetAuthResultTypeSyncWrapper` | class | `object`, `IMessage<...>`, `IEquatable<...>`, `IDeepCloneable<...>` | Protobuf 消息，包装 `UxNetAuthResultType` 与同步标志，用于同步认证结果 | `Gs/UxNetAuthResultTypeSyncWrapper.cs` |
| `Gs.UxNetAuthResultTypeTupleWrapper` | class | `object`, `IMessage<...>`, `IEquatable<...>`, `IDeepCloneable<...>` | Protobuf 消息，包装两个 `UxNetAuthResultType` 值的元组（Item1/Item2） | `Gs/UxNetAuthResultTypeTupleWrapper.cs` |
| `Gs.UxNetConnxEnv` | class | `object`, `IMessage<UxNetConnxEnv>`, `IMessage`, `IEquatable<UxNetConnxEnv>`, `IDeepCloneable<UxNetConnxEnv>` | 客户端连接环境信息，包含平台、渠道、设备型号、系统版本、MAC 地址、UDID、运营商、网络类型、语言、时区、IP 等 22 个字段 | `Gs/UxNetConnxEnv.cs` |
| `Gs.UxNetProxyHeader` | class | `object`, `IMessage<UxNetProxyHeader>`, `IMessage`, `IEquatable<UxNetProxyHeader>`, `IDeepCloneable<UxNetProxyHeader>` | 代理头部消息，包含 RoleId、ClientEp（客户端端点）、DsAuthToken（DS 认证令牌） | `Gs/UxNetProxyHeader.cs` |
| `Gs.UxNetUdpAsk` | class | `object`, `IMessage<UxNetUdpAsk>`, `IMessage`, `IEquatable<UxNetUdpAsk>`, `IDeepCloneable<UxNetUdpAsk>` | UDP 连接请求消息，包含 ConnectHost（目标主机）和 Port（端口） | `Gs/UxNetUdpAsk.cs` |
| `Gs.UxNetUdpAck` | class | `object`, `IMessage<UxNetUdpAck>`, `IMessage`, `IEquatable<UxNetUdpAck>`, `IDeepCloneable<UxNetUdpAck>` | UDP 连接确认消息，包含 Code（状态码） | `Gs/UxNetUdpAck.cs` |
| `Gs.UxNetWaitReq` | class | `object`, `IMessage<UxNetWaitReq>`, `IMessage`, `IEquatable<UxNetWaitReq>`, `IDeepCloneable<UxNetWaitReq>` | 排队等待请求消息，包含 Dummy（占位字段） | `Gs/UxNetWaitReq.cs` |
| `Gs.UxNetWaitRes` | class | `object`, `IMessage<UxNetWaitRes>`, `IMessage`, `IEquatable<UxNetWaitRes>`, `IDeepCloneable<UxNetWaitRes>` | 排队等待响应消息，包含 WaitQueueOrder（队列顺序）和 EstimateTime（预计等待时间） | `Gs/UxNetWaitRes.cs` |

---

## 架构概览

### 网络层次

```
应用层
  └─ NetChannel / ReverseProxyChannel    -- 会话层，处理认证、状态机、消息路由
       └─ NetDriver                      -- 传输层，管理 Socket 连接、IO 分发
            └─ NetConnection             -- 连接层，帧编解码、Socket 读写
                 └─ Socket               -- 系统 Socket
```

### 通信模式

1. **直连模式（NetChannel）**：客户端通过 URL 连接到服务器，经历 `CONNECT -> AUTH -> RELAY` 三个阶段，支持自动重连和超时检测。
2. **反向代理模式（ReverseProxyChannel）**：服务端监听端口，接收客户端连接，通过角色 ID 和认证令牌验证客户端身份，将客户端消息中继到上游游戏服务器，同时将上游响应转发回客户端。

### 帧协议

- 固定 12 字节帧头：`[frame_length(4)] [magic=0x55AAAAAA(4)] [flags(4)]`
- flags bit0=1 表示 Payload 经过 Zlib 压缩
- 最大帧大小：655360 字节

### Protobuf 消息（Gs 命名空间）

所有消息由 Google.Protobuf 库生成，遵循 `IMessage<T>` 接口。顶层消息 `UxNetMsg` 根据 `Cmd` 字段分发到不同的子消息：
- `Payload` -- 业务数据负载
- `AuthReq` / `AuthRes` -- 认证流程
- `UdpAsk` / `UdpAck` -- UDP 连接协商
- `WaitReq` / `WaitRes` -- 排队等待
