using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  可挂载单例模式基类模块
/// </summary>
/// <typeparam name="T"> 子类 </typeparam>
public class BaseControllerMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T controller;

    /// <summary>
    /// 创建一个挂载该脚本的物体，定义并返回一个唯一静态管理器
    /// </summary>
    /// <returns></returns>
    public static T GetController()
    {
        if(controller == null)
        {
            GameObject obj = new GameObject();
            obj.name = typeof(T).ToString();
            controller = obj.AddComponent<T>();

            GameObject.DontDestroyOnLoad(obj);
        }
        return controller;
    }


}
