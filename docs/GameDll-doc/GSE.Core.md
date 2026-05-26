# GSE.Core 类导航

## 模块结构

GSE.Core 是游戏《黑神话：悟空》的核心 C# 逻辑库，通过 Mono/ILRuntime 在 Unreal Engine 中运行。源码共 111 个 `.cs` 文件，分布在 17 个子目录中。以下是目录层级及用途：

| 目录 | 用途 |
|------|------|
| `GSE.Core/` (根) | 程序集信息 |
| `GSE.Core/b1/` | 主业务逻辑：工具类、数学库、原生容器、调试配置等 |
| `GSE.Core/b1/CppExport/` | C++ 函数指针绑定层，将 UE C++ 函数导出为 C# 委托 |
| `GSE.Core/b1/ECS/` | 实体组件系统 (Entity-Component System) |
| `GSE.Core/b1/ECS/Test/` | ECS 测试代码 |
| `GSE.Core/b1/GSFile/` | 文件系统工具 |
| `GSE.Core/b1/Prediction/` | 客户端预测系统 (网络权威回滚) |
| `GSE.Core/b1/Profile/` | 性能分析工具 |
| `GSE.Core/Microsoft/CodeAnalysis/` | 编译器内建标记特性 |
| `GSE.Core/System/Runtime/CompilerServices/` | 编译器内建特性 |
| `GSE.Core/Properties/` | 程序集元信息 |
| `GSE.Core/ResUpdator/` | 资源热更新 (下载/解压) |
| `GSE.Core/UnrealEngine/Runtime/` | UE 强引用指针 TStrongObjectPtr 的 C# 封装及 GC 管理 |

---

## 命名空间清单

- `b1`
- `b1.CppExport`
- `b1.ECS`
- `b1.ECS.Test`
- `b1.GSFile`
- `b1.Prediction`
- `b1.Profile`
- `Microsoft.CodeAnalysis`
- `ResUpdator`
- `System.Runtime.CompilerServices`
- `UnrealEngine.Runtime`

---

## 类 / 接口 / 结构 / 枚举 清单

### 命名空间 `Microsoft.CodeAnalysis`

| 完整类型名称 | 类型 | 继承关系 | 中文说明 | 文件 |
|-------------|------|---------|---------|------|
| `Microsoft.CodeAnalysis.EmbeddedAttribute` | class | `Attribute` | 编译器生成的标记特性，标记嵌入类型 | `Microsoft/CodeAnalysis/EmbeddedAttribute.cs` |

---

### 命名空间 `System.Runtime.CompilerServices`

| 完整类型名称 | 类型 | 继承关系 | 中文说明 | 文件 |
|-------------|------|---------|---------|------|
| `System.Runtime.CompilerServices.IsUnmanagedAttribute` | class | `Attribute` | 编译器生成的标记特性，标记非托管泛型约束 | `System/Runtime/CompilerServices/IsUnmanagedAttribute.cs` |

---

### 命名空间 `b1`

| 完整类型名称 | 类型 | 继承关系 | 中文说明 | 文件 |
|-------------|------|---------|---------|------|
| `b1.ActorFuncLib` | static class | - | Actor 函数库，装载 C++ 导出的 Actor 操作委托 (位置/旋转/变换/速度等) | `b1/ActorFuncLib.cs` |
| `b1.AIFuncLib` | static class | - | AI 函数库，装载 C++ 导出的 AI 配置/感知/行为树操作委托 | `b1/AIFuncLib.cs` |
| `b1.AnimFuncLib` | static class | - | 动画函数库，装载 C++ 导出的动画状态机/Montage/曲线操作委托 | `b1/AnimFuncLib.cs` |
| `b1.BGW_LogUtil` | class | - | 日志工具类，按调试标签 (WOOD/HEX/DEBUG 等) 提供条件编译日志输出，支持颜色和 VLog | `b1/BGW_LogUtil.cs` |
| `b1.CVarFuncLib` | static class | - | 控制台变量 (CVar) 函数库，创建/设置/销毁句柄 | `b1/CVarFuncLib.cs` |
| `b1.DebugConfig` | static class | - | 调试配置中心，通过控制台变量提供数百个调试开关 (渲染/AI/UI/网络/内存等) | `b1/DebugConfig.cs` |
| `b1.EBGULogColor` | enum (`byte`) | - | 日志颜色枚举 (White/Black/Red/Green/Purple 等 13 色) | `b1/EBGULogColor.cs` |
| `b1.EChangeReason` | enum | - | 属性变更原因枚举 (Init/ManualSet/InputSet/InnerOp) | `b1/EChangeReason.cs` |
| `b1.FColorBufferPtrPtrHelper` | class | - | FColorBufferPtr 指针辅助转换 | `b1/FColorBufferPtrHelper.cs` |
| `b1.FIntPtrHelper` | class | - | FIntPtr <-> void*/IntPtr 互转辅助工具 | `b1/FIntPtrHelper.cs` |
| `b1.GameplayTagFuncLib` | static class | - | GameplayTag 函数库，C++ 导出的 Tag 创建/比较/容器操作委托 | `b1/GameplayTagFuncLib.cs` |
| `b1.GSBindProp<T>` | class | - | 可绑定属性包装，带有变更事件 `ValueChangedHandler` | `b1/GSBindProp.cs` |
| `b1.GsCvarData` | class | - | 控制台变量数据封装，提供游戏线程安全的读写 | `b1/GsCvarData.cs` |
| `b1.GsCvarFuncLib` | static class | - | GS 自定义 CVar 函数库 | `b1/GsCvarFuncLib.cs` |
| `b1.GSDeadlockDetector` | class | - | 死锁检测器，监控主线程计数器是否卡死 | `b1/GSDeadlockDetector.cs` |
| `b1.GSEMacro` | static class | - | 空宏工具，`TODO()` 标记编译为无操作 | `b1/GSEMacro.cs` |
| `b1.GSE_ProtobufHelper` | class | - | Protobuf 序列化帮助类，将 `IMessage` 写入/读出文件 | `b1/GSE_ProtobufHelper.cs` |
| `b1.GS_GCHelper` | class | - | GC 帮助器，定期清理 TStrongObjectPtr 弱引用，防止泄漏 | `b1/GS_GCHelper.cs` |
| `b1.GSLocation` | class | `IDisposable` | 3D 坐标类，支持运算符重载 (`+` `-` `*` `/` `|` `^`)，使用对象池 | `b1/GSLocation.cs` |
| `b1.GSLocationPool` | static class | - | GSLocation 对象池 | `b1/GSLocationPool.cs` |
| `b1.GSPathUtil` | class | - | 路径工具类，读取项目配置/JSON 文件 | `b1/GSPathUtil.cs` |
| `b1.GSRotation` | class | `IDisposable` | 旋转类 (Pitch/Yaw/Roll)，支持运算符重载和 FRotator 互转，使用对象池 | `b1/GSRotation.cs` |
| `b1.GSRotationPool` | static class | - | GSRotation 对象池 | `b1/GSRotationPool.cs` |
| `b1.GSThreadPool` | class | - | 线程池，分 FastTask 和 SlowTask 两个后台线程 | `b1/GSThreadPool.cs` |
| `b1.IAssemblyRef_GSECore` | interface | - | 空接口，用于程序集引用标记 | `b1/IAssemblyRef_GSECore.cs` |
| `b1.InputFuncLib` | static class | - | 输入函数库，C++ 导出输入操作值委托 | `b1/InputFuncLib.cs` |
| `b1.MathLib` | static class | - | 数学库，提供 Abs/Cos/Sin/Lerp/Clamp/VInterpTo/RInterpTo 等函数，部分调 C++ 实现 | `b1/MathLib.cs` |
| `b1.NativeArray<T>` | struct | `IDisposable`, `IEnumerable<T>`, `IEquatable<NativeArray<T>>` | 非托管数组，内存手动分配，元素类型必须为 blittable | `b1/NativeArray.cs` |
| `b1.NativeHashMapBase<TKey, TValue>` | internal struct | - | 哈希表底层实现，线程安全原子操作 | `b1/NativeHashMapBase.cs` |
| `b1.NativeHashMap<TKey, TValue>` | struct | `IDisposable` | 非托管哈希表 (键唯一)，支持并发访问 | `b1/NativeHashMap.cs` |
| `b1.NativeHashMapData` | internal struct | - | 哈希表内存数据结构，管理 buckets/keys/values/next 指针 | `b1/NativeHashMapData.cs` |
| `b1.NativeList<T>` | struct | `IDisposable` | 非托管动态数组，支持 Add/RemoveAtSwapBack/RemoveKeepOrder | `b1/NativeList.cs` |
| `b1.NativeListData` | internal struct | - | NativeList 内部内存数据结构 (buffer/length/capacity) | `b1/NativeListData.cs` |
| `b1.NativeListDebugView<T>` | internal sealed class | - | 调试器可视化代理，显示 NativeList 的 Items | `b1/NativeListDebugView.cs` |
| `b1.NativeListUnsafeUtility` | static class | - | NativeList 不安全指针访问扩展方法 | `b1/NativeListUnsafeUtility.cs` |
| `b1.NativeMultiHashMap<TKey, TValue>` | struct | `IDisposable` | 非托管多值哈希表 (一个键多个值)，支持并发访问 | `b1/NativeMultiHashMap.cs` |
| `b1.NativeMultiHashMapIterator<TKey>` | struct | - | 多值哈希表迭代器 | `b1/NativeMultiHashMapIterator.cs` |
| `b1.PerlinNoise` | static class | - | Perlin 噪声实现，支持 1D/2D/3D 噪声和 Fbm 分形噪声 | `b1/PerlinNoise.cs` |
| `b1.PriorityQueue<T>` | class | - | 基于 `SortedList` 的优先级队列，元素需实现 `IComparable<T>` | `b1/PriorityQueue.cs` |
| `b1.ProjBranches` | enum | - | 项目分支枚举 (`b1_release`) | `b1/ProjBranches.cs` |
| `b1.ProjConst` | static class | - | 项目常量，指定项目名称和分支 | `b1/ProjConst.cs` |
| `b1.ProjNames` | enum | - | 项目名称枚举 (`B1`/`X2`/`U3`) | `b1/ProjNames.cs` |
| `b1.ProfilerFuncLib` | static class | - | 性能分析函数库，C++ 导出 StatID 创建/采样开始/结束委托 | `b1/ProfilerFuncLib.cs` |
| `b1.SceneComponentFuncLib` | static class | - | 场景组件函数库，C++ 导出 Socket 位置/旋转/变换委托 | `b1/SceneComponentFuncLib.cs` |
| `b1.SplineComponentFuncLib` | static class | - | 样条组件函数库，C++ 导出查找最近点委托 | `b1/SplineComponentFuncLib.cs` |
| `b1.SysLogUtil` | static class | - | 系统日志工具，按标签分类 (ARCHIVE/GAME_PLAYER/GSRPC 等) | `b1/SysLogUtil.cs` |
| `b1.TodoDelete_EnvMapNetServiceAddr` | static class | - | 环境映射网络服务地址配置，读取 gse_base.conf 获取 SDK/KA/CDN 地址 | `b1/TodoDelete_EnvMapNetServiceAddr.cs` |
| `b1.UMGQuickFuncLib` | static class | - | UMG 快速函数库，C++ 导出 Widget 可见性判断委托 | `b1/UMGQuickFuncLib.cs` |
| `b1.UnorderedArray<T>` | class | - | 无序数组，支持迭代器模式、交换删除 | `b1/UnorderedArray.cs` |
| `b1.UnorderedDict<TKey, TValue>` | class | - | 无序字典，基于数组和 `Dictionary<TKey, int>` 索引，支持迭代器模式 | `b1/UnorderedDict.cs` |
| `b1.UnsafeUtility` | internal class | - | 不安全内存工具，Malloc/Free/MemCpy/SizeOf/ReadArrayElement/WriteArrayElement | `b1/UnsafeUtility.cs` |
| `b1.WeakReferenceList<T>` | class | - | 弱引用列表，自增容量 | `b1/WeakReferenceList.cs` |
| `b1.WeakReferenceList_PingPong<T>` | class | - | 三缓冲弱引用列表，支持线程安全生产-消费模式 | `b1/WeakReferenceList_PingPong.cs` |

#### 嵌套类型 (b1)

| 完整类型名称 | 类型 | 所属父类 | 中文说明 |
|-------------|------|---------|---------|
| `b1.GSBindProp<T>.ValueChangedHandler` | delegate | `GSBindProp<T>` | 属性值变更回调委托 `(EChangeReason, T OldValue, T NewValue)` |
| `b1.GSThreadPool.Del_VoidObj` | delegate | `GSThreadPool` | 无返回值任务委托 `(object Arg)` |
| `b1.GSThreadPool.ThreadParam` | class | `GSThreadPool` | 线程参数 |
| `b1.GSThreadPool.TaskData` | struct | `GSThreadPool` | 任务数据，支持前置条件 |
| `b1.GSThreadPool.TaskItem` | private struct | `GSThreadPool` | 内部任务项 |
| `b1.GSThreadPool.TaskThreadData` | private class | `GSThreadPool` | 线程数据 (任务队列/事件/线程实例) |
| `b1.NativeArray<T>.Enumerator` | struct | `NativeArray<T>` | 枚举器 |
| `b1.NativeHashMap<TKey,TValue>.Concurrent` | struct | `NativeHashMap` | 并发哈希表视图 |
| `b1.NativeHashMapData.\<firstFreeTLS\>e__FixedBuffer` | struct | `NativeHashMapData` | 线程本地存储固定缓冲 (8192 bytes) |
| `b1.NativeHashMapData.\<padding1\>e__FixedBuffer` | struct | `NativeHashMapData` | 填充固定缓冲 (60 bytes) |
| `b1.NativeMultiHashMap<TKey,TValue>.Concurrent` | struct | `NativeMultiHashMap` | 并发多值哈希表视图 |
| `b1.GSEFileSystem.FSType` | private enum | `GSEFileSystem` | 文件系统类型 (Unknown/File/Jar/Invalid) |
| `b1.GSEFileSystem.FileItem` | private class | `GSEFileSystem` | 文件缓存项 |
| `b1.SysLogUtil.SysLogInstance` | class | `SysLogUtil` | 系统日志实例，按标签输出到 `BGW_LogUtil` |
| `b1.GSE_ProfileUtil.EProfileTag` | static class | `GSE_ProfileUtil` | 性能分析标签常量 |
| `b1.UnsafeUtility.BlittableHelper<T>` | private static class | `UnsafeUtility` | Blittable 类型检测器 |

---

### 命名空间 `b1.CppExport`

| 完整类型名称 | 类型 | 继承关系 | 中文说明 | 文件 |
|-------------|------|---------|---------|------|
| `b1.CppExport.GSE_ActorFuncs` | class | - | Actor 原生函数绑定，通过反射从 C++ Map 装载委托 | `b1/CppExport/GSE_ActorFuncs.cs` |
| `b1.CppExport.GSE_AnimFuncs` | class | - | 动画原生函数绑定 | `b1/CppExport/GSE_AnimFuncs.cs` |
| `b1.CppExport.GSE_CVarFunc` | class | - | 控制台变量原生函数绑定 | `b1/CppExport/GSE_CVarFunc.cs` |
| `b1.CppExport.GSE_GSCVarFuncs` | class | - | GS 自定义 CVar 原生函数绑定 | `b1/CppExport/GSE_GSCVarFuncs.cs` |
| `b1.CppExport.GSE_NativeAIFuncs` | class | - | AI 原生函数绑定 | `b1/CppExport/GSE_NativeAIFuncs.cs` |
| `b1.CppExport.GSE_NativeAsyncLineTraceReqRefFuncs` | class | - | 异步线追踪请求引用原生函数绑定 | `b1/CppExport/GSE_NativeAsyncLineTraceReqRefFuncs.cs` |
| `b1.CppExport.GSE_NativeGameplayTagFuncs` | class | - | GameplayTag 原生函数绑定 | `b1/CppExport/GSE_NativeGameplayTagFuncs.cs` |
| `b1.CppExport.GSE_NativeInputFunc` | class | - | 输入原生函数绑定 | `b1/CppExport/GSE_NativeInputFunc.cs` |
| `b1.CppExport.GSE_NativeMathFuncs` | class | - | 数学原生函数绑定 | `b1/CppExport/GSE_NativeMathFuncs.cs` |
| `b1.CppExport.GSE_NativeProfilerFuncs` | class | - | 性能分析原生函数绑定 | `b1/CppExport/GSE_NativeProfilerFuncs.cs` |
| `b1.CppExport.GSE_NativeSceneComponentFuncs` | class | - | 场景组件原生函数绑定 | `b1/CppExport/GSE_NativeSceneComponentFuncs.cs` |
| `b1.CppExport.GSE_NativeTaskGraphFuncs` | class | - | 任务图原生函数绑定 | `b1/CppExport/GSE_NativeTaskGraphFuncs.cs` |
| `b1.CppExport.GSE_NiagaraFunc` | class | - | Niagara 特效原生函数绑定 | `b1/CppExport/GSE_NiagaraFunc.cs` |
| `b1.CppExport.GSE_RenderFunc` | class | - | 渲染原生函数绑定 | `b1/CppExport/GSE_RenderFunc.cs` |
| `b1.CppExport.GSE_SplineComponentFunc` | class | - | 样条组件原生函数绑定 | `b1/CppExport/GSE_SplineComponentFunc.cs` |
| `b1.CppExport.GSE_UMGFuncs` | class | - | UMG 原生函数绑定 | `b1/CppExport/GSE_UMGFuncs.cs` |
| `b1.CppExport.GSE_AsyncLineTraceReqFunc` | class | - | 异步线追踪请求原生函数绑定 | `b1/CppExport/GSE_AsyncLineTraceReqFunc.cs` |
| `b1.CppExport.GSE_LineTraceFuncs` | class | - | 线追踪原生函数绑定 | `b1/CppExport/GSE_LineTraceFuncs.cs` |
| `b1.CppExport.AsyncLineTraceReqLib` | static class | - | 异步线追踪请求库，C++ 导出的分配/添加/销毁委托 | `b1/CppExport/AsyncLineTraceReqLib.cs` |
| `b1.CppExport.AsyncLineTraceReqRef` | class | `IDisposable`, `IEnumerable` | 异步线追踪请求引用，包装 C++ 侧请求数组 | `b1/CppExport/AsyncLineTraceReqRef.cs` |
| `b1.CppExport.GameplayTagContainerRef` | class | `IDisposable`, `IEnumerable` | GameplayTag 容器引用，包装 C++ 侧 Tag 容器 | `b1/CppExport/GameplayTagContainerRef.cs` |
| `b1.CppExport.LineTraceFuncLib` | static class | - | 线追踪函数库，发起异步线追踪请求 | `b1/CppExport/LineTraceFuncLib.cs` |

---

### 命名空间 `b1.ECS`

| 完整类型名称 | 类型 | 继承关系 | 中文说明 | 文件 |
|-------------|------|---------|---------|------|
| `b1.ECS.Chunk` | class | `IDisposable` | ECS Chunk，固定 64 个实体的内存块，管理原始数据/SafeData/组件/对象 | `b1/ECS/Chunk.cs` |
| `b1.ECS.EntityArchetype` | class | `IDisposable` | ECS 原型，定义实体包含的数据类型、安全数据类型和组件类型 | `b1/ECS/EntityArchetype.cs` |
| `b1.ECS.Entity` | struct | `IEquatable<Entity>` | ECS 实体句柄，32 位索引编码 (ManagerIdx/ArchIndex/ChunkIndex/Version/IndexInChunk) | `b1/ECS/Entity.cs` |
| `b1.ECS.EntityManager` | class | - | ECS 实体管理器，创建/销毁实体，读写数据/组件，Tick 组件 | `b1/ECS/EntityManager.cs` |
| `b1.ECS.IEntityComponent` | interface | - | ECS 组件接口，定义 Tick、网络角色等生命周期方法 | `b1/ECS/IEntityComponent.cs` |
| `b1.ECS.IEntitySafeData` | interface | - | 安全数据接口，提供 `SetPtr(IntPtr)` 绑定到非托管内存 | `b1/ECS/IEntitySafeData.cs` |
| `b1.ECS.IPersistentECSData` | interface | - | 持久化 ECS 数据标记接口 | `b1/ECS/IPersistentECSData.cs` |
| `b1.ECS.IPersistentECSDataWithDestroyCB` | interface | `IPersistentECSData` | 带销毁回调的持久化 ECS 数据接口 | `b1/ECS/IPersistentECSDataWithDestroyCB.cs` |
| `b1.ECS.NativeStream` | struct | `IDisposable` | 非托管二进制流，提供 Reader/Writer 用于数据序列化 | `b1/ECS/NativeStream.cs` |
| `b1.ECS.TypeManager` | static class | - | ECS 类型管理器，类型与索引双向映射 | `b1/ECS/TypeManager.cs` |

#### 嵌套类型 (b1.ECS)

| 完整类型名称 | 类型 | 所属父类 | 中文说明 |
|-------------|------|---------|---------|
| `b1.ECS.Chunk.CompTickStatId` | struct | `Chunk` | 组件 Tick 统计 ID |
| `b1.ECS.Chunk.CompList` | class | `Chunk` | 组件列表，包含 TickGroup 和 StatId |
| `b1.ECS.EntityManager.IterateCompFunc` | delegate | `EntityManager` | 组件迭代回调 `(IEntityComponent)` |
| `b1.ECS.EntityManager.IterateRawDataFunc<T>` | unsafe delegate | `EntityManager` | 原始数据迭代回调 `(T* Data)` |
| `b1.ECS.TypeManager.StaticLookUp<T>` | private static class | `TypeManager` | 泛型类型索引查找器 |
| `b1.ECS.NativeStream.Writer` | struct | `NativeStream` | 非托管流写入器 |
| `b1.ECS.NativeStream.Reader` | struct | `NativeStream` | 非托管流读取器 |

---

### 命名空间 `b1.ECS.Test`

| 完整类型名称 | 类型 | 继承关系 | 中文说明 | 文件 |
|-------------|------|---------|---------|------|
| `b1.ECS.Test.ECSTestFuncs` | static class | - | ECS 单元测试函数，测试实体索引和 Chunk 计数 | `b1/ECS/Test/ECSTestFuncs.cs` |
| `b1.ECS.Test.TestData` | internal struct | - | 测试用原始数据类型 | `b1/ECS/Test/TestData.cs` |
| `b1.ECS.Test.TestDataSafe` | internal class | `IEntitySafeData` | 测试用安全数据类型，包装 TestData 指针 | `b1/ECS/Test/TestDataSafe.cs` |

---

### 命名空间 `b1.GSFile`

| 完整类型名称 | 类型 | 继承关系 | 中文说明 | 文件 |
|-------------|------|---------|---------|------|
| `b1.GSFile.GSEFileSystem` | class | - | 文件系统，管理 Patch/Extract/Temp 路径的文件读取和缓存 | `b1/GSFile/GSEFileSystem.cs` |
| `b1.GSFile.GSEFileUtil` | class | - | 文件工具类，提供读写/MD5/创建目录/路径规范化等静态方法 | `b1/GSFile/GSEFileUtil.cs` |

---

### 命名空间 `b1.Prediction`

| 完整类型名称 | 类型 | 继承关系 | 中文说明 | 文件 |
|-------------|------|---------|---------|------|
| `b1.Prediction.GSEventPredictionNode` | class | - | 事件预测节点，构成事件树 (Parent/Child) | `b1/Prediction/GSEventPredictionNode.cs` |
| `b1.Prediction.GSPredictionKey` | class | - | 预测键，管理预测相关对象和事件树，支持回滚/确认操作 | `b1/Prediction/GSPredictionKey.cs` |
| `b1.Prediction.IPredictableObject` | interface | - | 可预测对象接口，定义 `OnRollback` 和 `OnConfirm` | `b1/Prediction/IPredictableObject.cs` |
| `b1.Prediction.NormalPredictionDataDeltaOne<T>` | class | - | 普通预测数据增量 (单键单值) | `b1/Prediction/NormalPredictionDataDeltaOne.cs` |
| `b1.Prediction.NormalPredictionDataSet<T>` | class | `IPredictableObject` | 普通预测数据集，管理基于预测键的数据覆盖 | `b1/Prediction/NormalPredictionDataSet.cs` |
| `b1.Prediction.OpratablePredictionDataDeltaOne<T>` | class | - | 可运算预测数据增量 (单键单增量) | `b1/Prediction/OpratablePredictionDataDeltaOne.cs` |
| `b1.Prediction.OpratablePredictionDataSet<T>` | class | `IPredictableObject` | 可运算预测数据集，通过动态运算累加增量值 | `b1/Prediction/OpratablePredictionDataSet.cs` |

---

### 命名空间 `b1.Profile`

| 完整类型名称 | 类型 | 继承关系 | 中文说明 | 文件 |
|-------------|------|---------|---------|------|
| `b1.Profile.GSE_ProfileScope` | class | `IDisposable` | 性能分析作用域，构造开始采样、析构结束采样 | `b1/Profile/GSE_ProfileScope.cs` |
| `b1.Profile.GSE_ProfileUtil` | static class | - | 性能分析工具，支持按标签和 TickGroup 的嵌套式采样 | `b1/Profile/GSE_ProfileUtil.cs` |

---

### 命名空间 `ResUpdator`

| 完整类型名称 | 类型 | 继承关系 | 中文说明 | 文件 |
|-------------|------|---------|---------|------|
| `ResUpdator.GSEFileDownloader` | class | `IDisposable` | 文件下载器，支持 HTTP/HTTPS 下载、断点续传、进度回调 | `ResUpdator/GSEFileDownloader.cs` |
| `ResUpdator.GSEPatchUnzipper` | class | `IDisposable` | 补丁解压器，支持 ZIP 解压和 MD5 校验 | `ResUpdator/GSEPatchUnzipper.cs` |

#### 嵌套类型 (ResUpdator)

| 完整类型名称 | 类型 | 所属父类 | 中文说明 |
|-------------|------|---------|---------|
| `ResUpdator.GSEFileDownloader.DownloaderCallback` | delegate | `GSEFileDownloader` | 下载完成回调 |
| `ResUpdator.GSEFileDownloader.State` | enum | `GSEFileDownloader` | 下载状态 (None/Downloading/LostConnection/Failed/Done) |

---

### 命名空间 `UnrealEngine.Runtime`

| 完整类型名称 | 类型 | 继承关系 | 中文说明 | 文件 |
|-------------|------|---------|---------|------|
| `UnrealEngine.Runtime.FGSInternalReferenceCollector` | struct | - | 内部引用收集器结构体，标记引用是否已添加 | `UnrealEngine/Runtime/FGSInternalReferenceCollector.cs` |
| `UnrealEngine.Runtime.StrongPtrGCCollector` | static class | - | 强指针 GC 收集器，定期检查 TStrongObjectPtr 有效性并清理失效指针 | `UnrealEngine/Runtime/StrongPtrGCCollector.cs` |
| `UnrealEngine.Runtime.StrongPtrLeakDetection` | static class | - | 强指针泄漏检测工具，跟踪所有注册的 TStrongObjectPtr | `UnrealEngine/Runtime/StrongPtrLeakDetection.cs` |
| `UnrealEngine.Runtime.TStrongObjectPtrBase` | abstract class | - | TStrongObjectPtr 基类，定义 `GetUObject/IsValid/SetNull/GCClear` 抽象方法 | `UnrealEngine/Runtime/TStrongObjectPtrBase.cs` |
| `UnrealEngine.Runtime.TStrongObjectPtr<T>` | class | `TStrongObjectPtrBase`, `IDisposable`, `IEquatable<TStrongObjectPtr<T>>` | 强引用对象指针，包装 UE TStrongObjectPtr，支持自动 GC 收集 | `UnrealEngine/Runtime/TStrongObjectPtr.cs` |
| `UnrealEngine.Runtime.TStrongObjectPtr_NoCollect<T>` | class | `TStrongObjectPtr<T>` | 不参与 GC 收集的强引用对象指针 | `UnrealEngine/Runtime/TStrongObjectPtr_NoCollect.cs` |
| `UnrealEngine.Runtime.TStrongPtrNativeLayout` | struct | - | 强指针原生内存布局，与 C++ 侧 `FGSInternalReferenceCollector` 对应 | `UnrealEngine/Runtime/TStrongPtrNativeLayout.cs` |

#### 嵌套类型 (UnrealEngine.Runtime)

| 完整类型名称 | 类型 | 所属父类 | 中文说明 |
|-------------|------|---------|---------|
| `UnrealEngine.Runtime.StrongPtrGCCollector.WeakRefAllocator` | private class | `StrongPtrGCCollector` | 弱引用分配器，使用对象池减少 GC 压力 |

---

## 统计概览

- **文件总数**: 111
- **命名空间**: 11
- **class**: 约 70+ (含嵌套类)
- **struct**: 约 20+
- **interface**: 7
- **enum**: 5
- **delegate**: 约 10+
