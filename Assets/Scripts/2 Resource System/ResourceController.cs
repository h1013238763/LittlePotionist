using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源加载模块
/// </summary>
public class ResourceController : BaseController<ResourceController>
{
    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <param name="name">资源名称</param>
    public T Load<T>(string name) where T : Object
    {
        T res = Resources.Load<T>(name);

        // 如果对象为GameObject, 实例化后返回该对象
        if( res is GameObject )
            return GameObject.Instantiate(res);
        else
            return res;
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="name">资源名称</param>
    /// <param name="callback"> 资源使用函数 </param>
    /// <typeparam name="T"></typeparam>
    public void LoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        MonoController.GetController().StartCoroutine(ILoadAsync(name, callback));
    }

    /// <summary>
    /// 携程函数
    /// </summary>
    /// <param name="name">资源名称</param>
    /// <param name="callback"> 资源使用函数 </param>
    /// <returns></returns>
    private IEnumerator ILoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);
        yield return r;

        if( r.asset is GameObject )
            callback( GameObject.Instantiate(r.asset) as T );
        else
            callback( r.asset as T );
    } 
}
