# GSE.OnlineBase 类导航

## 命名空间清单

| 命名空间 | 说明 |
|----------|------|
| `GSE.OnlineBase` | 游戏后端引擎（Game Server Engine）在线基础库，提供日志、度量、Protobuf 编解码、URL 解析、压缩等通用工具类型 |

仅有一个命名空间，同程序集名称 `GSE.OnlineBase`（AssemblyTitle）。

---

## 枚举清单

### `GSE.OnlineBase.GSEOnlineLogLevel`

| 项目 | 值 |
|------|-----|
| **类型** | enum |
| **基类型** | `System.Enum`（隐式 `int`） |
| **所在文件** | `GSE.OnlineBase/GSEOnlineLogLevel.cs` |
| **说明** | 日志级别枚举，从低到高定义了 6 个级别 |

| 成员名 | 整数值 | 说明 |
|--------|--------|------|
| `LOG_TRACE` | 0 | 跟踪级别（最详细） |
| `LOG_DEBUG` | 1 | 调试级别 |
| `LOG_INFO` | 2 | 信息级别 |
| `LOG_WARNING` | 3 | 警告级别 |
| `LOG_ERROR` | 4 | 错误级别 |
| `LOG_FATAL` | 5 | 致命级别（最严重） |

---

## 委托清单

### `GSE.OnlineBase.LogHelper.OnLogWrite`

| 项目 | 值 |
|------|-----|
| **类型** | delegate |
| **签名** | `void OnLogWrite(GSEOnlineLogLevel lv, string msg)` |
| **所在文件** | `GSE.OnlineBase/LogHelper.cs` |
| **说明** | 日志写入回调委托，嵌套定义于 `LogHelper` 类中 |

---

## 类清单

### `GSE.OnlineBase.Adler32Crc`

| 项目 | 值 |
|------|-----|
| **类型** | class |
| **继承** | `System.Object` |
| **所在文件** | `GSE.OnlineBase/Adler32Crc.cs` |
| **说明** | Adler-32 校验和算法实现，提供增量计算和一次性计算两种模式 |

**成员概要：**
- 属性 `Checksum`（只读 `int`）：获取当前校验和值（`b * 65536 + a`）
- 方法 `Update(byte[], int, int)`：增量更新校验和
- 静态方法 `GetCheckSum(byte[], int, int)`：一次性计算字节数组的 Adler-32 校验和
- 常量 `Modulus = 65521`

---

### `GSE.OnlineBase.ILRuntimeBinding`

| 项目 | 值 |
|------|-----|
| **类型** | static class |
| **继承** | `System.Object` |
| **所在文件** | `GSE.OnlineBase/ILRuntimeBinding.cs` |
| **说明** | ILRuntime 热更新绑定注册类，为热更新环境注册委托转换器和方法委托，使热更代码能够使用 `ThreadStart` 和 `HttpStatusCode` 相关回调 |

**成员概要：**
- 静态方法 `Register(AppDomain)`：注册 `HttpStatusCode, byte[]` 方法委托及 `ThreadStart` 委托转换器

---

### `GSE.OnlineBase.LocalTime`

| 项目 | 值 |
|------|-----|
| **类型** | static class |
| **继承** | `System.Object` |
| **所在文件** | `GSE.OnlineBase/LocalTime.cs` |
| **说明** | 本地时间工具类，提供基于 UTC 的 Unix 时间戳获取方法 |

**成员概要：**
- 静态字段 `UNIX_TIME_STAMP_BEGIN`：Unix 纪元起点 `1970-01-01 00:00:00`
- 静态方法 `GetUnixTimeStamp()`：返回当前 Unix 时间戳（秒，`uint`）
- 静态方法 `GetUnixTimeStampMilliSeconds()`：返回当前 Unix 时间戳（毫秒，`long`）

---

### `GSE.OnlineBase.LogHelper`

| 项目 | 值 |
|------|-----|
| **类型** | static class |
| **继承** | `System.Object` |
| **所在文件** | `GSE.OnlineBase/LogHelper.cs` |
| **说明** | 日志输出静态工具类，支持通过委托回调输出日志，可按日志级别过滤；同时嵌套定义了 `OnLogWrite` 回调委托 |

**成员概要：**
- 嵌套委托 `OnLogWrite(GSEOnlineLogLevel, string)`：日志写入回调
- 静态方法 `SetLogWriter(OnLogWrite, GSEOnlineLogLevel)`：设置日志写入器及过滤级别
- 静态方法 `SetLogLevel(GSEOnlineLogLevel)`：设置过滤级别
- 静态方法 `LogError(string msg)` / `LogError(string format, params object[])`：输出错误日志
- 静态方法 `LogWarn(string, params object[])`：输出警告日志
- 静态方法 `LogInfo(string, params object[])`：输出信息日志
- 静态方法 `LogFatal(string, params object[])`：输出致命日志
- 静态方法 `[Conditional("DEBUG")] LogDebug(string, params object[])`：调试日志（仅 DEBUG 编译）
- 静态方法 `[Conditional("DEBUG")] LogDbgSim(string, params object[])`：调试模拟日志（仅 DEBUG）
- 静态方法 `[Conditional("DEBUG")] LogTrace(string, params object[])`：跟踪日志（仅 DEBUG）
- 静态方法 `[Conditional("DEBUG")] LogTempDelTodo(string, params object[])`：临时调试日志（标记待删除）

---

### `GSE.OnlineBase.Metric`

| 项目 | 值 |
|------|-----|
| **类型** | class |
| **继承** | `System.Object` |
| **所在文件** | `GSE.OnlineBase/Metric.cs` |
| **说明** | 度量数据实体类，存储单个指标的统计值（总和、计数、最小值、最大值）及其标识键 |

**成员概要：**
- 字段 `Key`（`string`）：指标名称
- 字段 `Param`（`string`）：指标参数/标签
- 字段 `Sum`（`long`）：总和
- 字段 `Count`（`long`）：计数
- 字段 `Min`（`long`）：最小值
- 字段 `Max`（`long`）：最大值

---

### `GSE.OnlineBase.MetricStat`

| 项目 | 值 |
|------|-----|
| **类型** | class |
| **继承** | `System.Object` |
| **所在文件** | `GSE.OnlineBase/MetricStat.cs` |
| **说明** | 度量统计数据类，包装 `Metric` 并附带采集时间戳、采集间隔和聚合标记等元信息 |

**成员概要：**
- 字段 `metric`（`Metric`）：关联的度量数据
- 字段 `LastCollectTimeSecond`（`long`）：上次采集时间（秒）
- 字段 `MetricInterval`（`long`）：采集间隔（秒）
- 字段 `bAggreagte`（`bool`）：是否为聚合模式（`true` 聚合累计，`false` 直接赋值）

---

### `GSE.OnlineBase.MetricStastic`

| 项目 | 值 |
|------|-----|
| **类型** | class |
| **继承** | `System.Object` |
| **所在文件** | `GSE.OnlineBase/MetricStastic.cs` |
| **说明** | 度量统计管理器，以字典维护多组指标，支持定时采样与批量采集上报 |

**成员概要：**
- 方法 `Sample(string, long, string, int)`：采样一条指标数据，按间隔和聚合模式更新统计
- 方法 `TickCollectSeconds(bool, int, int)`：定时采集到期指标，返回待上报的 `Metric` 列表
- 字段 `MetricMaps`（`Dictionary<string, MetricStat>`）：指标字典
- 字段 `TimeNowSecond`（`long`）：当前模拟时间（秒）

---

### `GSE.OnlineBase.Misc`

| 项目 | 值 |
|------|-----|
| **类型** | static class |
| **继承** | `System.Object` |
| **所在文件** | `GSE.OnlineBase/Misc.cs` |
| **说明** | 杂项工具类，提供对象内存地址的获取方法 |

**成员概要：**
- 静态方法 `GetAddress(object)`：通过 `GCHandle`（弱跟踪）获取对象内存地址的十六进制字符串

---

### `GSE.OnlineBase.PbEncoding`

| 项目 | 值 |
|------|-----|
| **类型** | static class |
| **继承** | `System.Object` |
| **所在文件** | `GSE.OnlineBase/PbEncoding.cs` |
| **说明** | Protobuf 序列化/反序列化工具类，封装 `Google.Protobuf` 库的编解码及文本化方法 |

**成员概要：**
- 静态泛型方法 `Encode<T>(T)`：将 `IMessage` 实例序列化为 `byte[]`
- 静态泛型方法 `Decode<T>(byte[])`：从字节数组反序列化为 `T` 实例
- 静态泛型方法 `Decode<T>(byte[], T)`：从字节数组反序列化到已有 `T` 实例
- 静态方法 `ToString(IMessage)`：将 `IMessage` 转为多行可读文本（委托 `ProtobufTextDumper`）
- 静态方法 `ToStringInLine(IMessage)`：将 `IMessage` 转为单行可读文本

---

### `GSE.OnlineBase.ProtobufTextDumper`

| 项目 | 值 |
|------|-----|
| **类型** | class |
| **继承** | `System.Object` |
| **所在文件** | `GSE.OnlineBase/ProtobufTextDumper.cs` |
| **说明** | Protobuf 消息文本转储器，利用反射递归遍历消息对象，输出格式化的多行或单行文本 |

**成员概要：**
- 静态方法 `DumpAsString(object, string)`：将对象转储为带缩进的多行文本
- 静态方法 `DumpAsStringInLine(object, string)`：将对象转储为单行文本
- 私有静态方法 `DoDump(object, int, int)`：递归转储核心逻辑，处理 `IList`、`ByteString`、值类型/字符串、数组、类对象等
- 私有静态字段 `_text`（`StringBuilder`）：内部文本缓冲区

---

### `GSE.OnlineBase.UrlParser`

| 项目 | 值 |
|------|-----|
| **类型** | class |
| **继承** | `System.Object` |
| **所在文件** | `GSE.OnlineBase/UrlParser.cs` |
| **说明** | URL 解析器，通过正则表达式解析 `tcp://`、`udp://`、`http://`、`kcp://` 协议格式的连接字符串，提取协议、主机、端口、路径和查询参数 |

**支持协议格式：**
```
<proto>://<host>:<port>/<path>?<key>=<value>&<key>=<value>
```

**成员概要：**
- 静态字段 `UrlRegex`（`Regex`）：编译型 URL 正则表达式
- 方法 `Parse(string)`：解析 URL 字符串，提取分组和查询参数
- 方法 `GetHost(string)`：获取主机名（默认 `127.0.0.1`）
- 方法 `GetProto(string)`：获取协议名
- 方法 `GetPort(short)`：获取端口号
- 方法 `GetPath(string)`：获取路径（默认 `/`）
- 方法 `GetOption(string, string)`：获取查询参数值
- 私有字段 `options`（`Dictionary<string, string>`）：查询参数字典
- 私有字段 `groups`（`GroupCollection`）：正则分组集合

---

### `GSE.OnlineBase.ZlibHelper`

| 项目 | 值 |
|------|-----|
| **类型** | static class |
| **继承** | `System.Object` |
| **所在文件** | `GSE.OnlineBase/ZlibHelper.cs` |
| **说明** | Zlib 压缩/解压缩工具类，使用 .NET 内置的 `DeflateStream` 实现，添加标准的 zlib 头部（`0x78 0x9C`）和 Adler-32 校验尾 |

**成员概要：**
- 静态方法 `ZlibCompress(byte[])`：压缩数据，添加 zlib 头（2 字节）和 Adler-32 校验和（4 字节）
- 静态方法 `ZlibDecompress(byte[])`：解压 zlib 格式数据，跳过头部 2 字节和尾部 4 字节
- 静态方法 `GetAddress(object)`：获取对象内存地址的十六进制字符串（与 `Misc.GetAddress` 功能相同）

---

## 文件索引

| 文件名 | 主类型 | 路径（相对于 GameDll-src） |
|--------|--------|---------------------------|
| `Adler32Crc.cs` | `Adler32Crc` | `GSE.OnlineBase/Adler32Crc.cs` |
| `GSEOnlineLogLevel.cs` | `GSEOnlineLogLevel` | `GSE.OnlineBase/GSEOnlineLogLevel.cs` |
| `ILRuntimeBinding.cs` | `ILRuntimeBinding` | `GSE.OnlineBase/ILRuntimeBinding.cs` |
| `LocalTime.cs` | `LocalTime` | `GSE.OnlineBase/LocalTime.cs` |
| `LogHelper.cs` | `LogHelper` + `LogHelper.OnLogWrite` | `GSE.OnlineBase/LogHelper.cs` |
| `Metric.cs` | `Metric` | `GSE.OnlineBase/Metric.cs` |
| `MetricStat.cs` | `MetricStat` | `GSE.OnlineBase/MetricStat.cs` |
| `MetricStastic.cs` | `MetricStastic` | `GSE.OnlineBase/MetricStastic.cs` |
| `Misc.cs` | `Misc` | `GSE.OnlineBase/Misc.cs` |
| `PbEncoding.cs` | `PbEncoding` | `GSE.OnlineBase/PbEncoding.cs` |
| `ProtobufTextDumper.cs` | `ProtobufTextDumper` | `GSE.OnlineBase/ProtobufTextDumper.cs` |
| `UrlParser.cs` | `UrlParser` | `GSE.OnlineBase/UrlParser.cs` |
| `ZlibHelper.cs` | `ZlibHelper` | `GSE.OnlineBase/ZlibHelper.cs` |

---

## 类型统计

| 类别 | 数量 | 类型列表 |
|------|------|----------|
| **class** | 8 | `Adler32Crc`、`Metric`、`MetricStat`、`MetricStastic`、`ProtobufTextDumper`、`UrlParser` |
| **static class** | 6 | `ILRuntimeBinding`、`LocalTime`、`LogHelper`、`Misc`、`PbEncoding`、`ZlibHelper` |
| **enum** | 1 | `GSEOnlineLogLevel` |
| **delegate** | 1 | `LogHelper.OnLogWrite` |
| **合计** | **16** | （含 1 个嵌套委托） |
