using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例模式基类模块
/// unique object controller module
/// </summary>
/// <typeparam name="T"> 子类 - child class </typeparam>
public class BaseController<T> where T: new()
{
    private static T controller;

    /// <summary>
    /// 定义并返回一个唯一静态管理器
    /// Define and return a unique static controller
    /// </summary>
    /// <returns>控制器 - The controller</returns>
    public static T GetController()
    {
        if(controller == null)
            controller = new T();
        return controller;
    }
}