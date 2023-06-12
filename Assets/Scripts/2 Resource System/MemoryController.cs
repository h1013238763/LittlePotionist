using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

/// <summary>
/// 内存管理与垃圾回收模块
/// </summary>
public class MemoryController : BaseController<MemoryController>
{
    public void ForceCollectGarbage(){
        System.GC.Collect();
    }

    public void ForceCollectGarbageAsync(ulong t){
        GarbageCollector.CollectIncremental(t);
    }
}
