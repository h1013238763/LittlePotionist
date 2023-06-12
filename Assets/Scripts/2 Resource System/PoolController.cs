using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 对象缓存池模块
/// </summary>
public class PoolController : BaseController<PoolController>
{
    private GameObject dictionary_obj;
    // 对象池总辞典
    public Dictionary<string, PoolData> dictionary_pool = new Dictionary<string, PoolData>();

    /// <summary>
    /// 从对象池中异步获取一个对应对象
    /// </summary>
    /// <param name="name">对象名称</param>
    /// <returns>获取到的对象</returns>
    public void GetObject(string name, UnityAction<GameObject> callback)
    {
        if(dictionary_obj == null)
            dictionary_obj = new GameObject("Pool Manager");

        // 查找和创建对象池
        if(!dictionary_pool.ContainsKey(name))
            dictionary_pool.Add(name, new PoolData(dictionary_obj, name));

        // 激活并返回对象
        if(dictionary_pool[name].pool_list.Count > 0)
        {
            callback(dictionary_pool[name].GetObj());
        }
        else
        {
            ResourceController.GetController().LoadAsync<GameObject>(name, (o) =>
            {
                o.name = name;
                callback(o);
            });
        } 
    }

    /// <summary>
    /// 从对象池中同步获取一个对应对象
    /// </summary>
    /// <param name="name">对象名称</param>
    public GameObject GetObject(string name)
    {
        if(dictionary_obj == null)
            dictionary_obj = new GameObject("Pool Manager");

        // 查找和创建对象池
        if(!dictionary_pool.ContainsKey(name))
            dictionary_pool.Add(name, new PoolData(dictionary_obj, name));

        if( dictionary_pool[name].pool_list.Count > 0 )
        {
            return dictionary_pool[name].GetObj();
        }
        else
        {
            return new GameObject(name);
        }
    }

    /// <summary>
    /// 将一个对象放回至物品池中
    /// </summary>
    /// <param name="name">对象名称</param>
    /// <param name="obj">返回的对象</param>
    public void PushObject(string name, GameObject obj)
    {
        // 失活对象
        obj.SetActive(false);

        // 将对象数据存放到对象池中
        dictionary_pool[name].PushObj(obj);
    }

    /// <summary>
    /// 清空对象池
    /// </summary>
    public void Clear(){
        dictionary_pool.Clear();
        dictionary_obj = null;
    }

}

/// <summary>
/// 自定义对象池类
/// </summary>
public class PoolData
{
    
    public GameObject pool_obj;         // 场景中收纳对象物体的物体
    public List<GameObject> pool_list;  // 对象池数据

    /// <summary>
    /// 构造器
    ///     初始化收纳物体并设置层级
    ///     存储当前物体
    /// </summary>
    /// <param name="obj"> 需要储存的对象 </param>
    /// <param name="dic_obj"> 辞典用的父物体 </param>
    public PoolData(GameObject dic_obj, string name)
    {
        pool_obj = new GameObject(name);
        pool_obj.transform.SetParent(dic_obj.transform);

        pool_list = new List<GameObject>();
    }

    /// <summary>
    /// 将一个对象放回至物品池中
    /// </summary>
    /// <param name="name">对象名称</param>
    /// <param name="obj">返回的对象</param>
    public void PushObj(GameObject obj)
    {
        pool_list.Add(obj);
        obj.transform.SetParent(pool_obj.transform);
    }

    /// <summary>
    /// 从对象池中获取一个对应对象
    /// </summary>
    /// <returns>获取到的对象</returns>
    public GameObject GetObj()
    {
        GameObject obj = obj = pool_list[0];
        pool_list.RemoveAt(0);  

        return obj;  
    }
}