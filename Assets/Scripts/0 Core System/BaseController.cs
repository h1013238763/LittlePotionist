using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// unique object controller module
/// </summary>
/// <typeparam name="T">child class </typeparam>
public class BaseController<T> where T: new()
{
    private static T controller;

    /// <summary>
    /// Define and return a unique static controller
    /// </summary>
    /// <returns>The controller</returns>
    public static T Controller()
    {
        if(controller == null)
            controller = new T();
        return controller;
    }
}