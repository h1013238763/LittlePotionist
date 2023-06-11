using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例模式基类模块
/// </summary>
/// <typeparam name="T"> 子类 </typeparam>
public class BaseController<T> where T: new()
{
    private static T controller;

    /// <summary>
    /// 定义并返回一个唯一静态管理器
    /// </summary>
    /// <returns></returns>
    public static T GetController(){
        if(controller == null)
            controller = new T();
        return controller;
    }
}