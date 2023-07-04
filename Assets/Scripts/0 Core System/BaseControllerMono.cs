using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  monobehavior unique object controller module
/// </summary>
/// <typeparam name="T"> child class </typeparam>
public class BaseControllerMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T controller;

    /// <summary>
    /// Find or create a gameobject with controller component and return the controller
    /// </summary>
    /// <returns> the controller</returns>
    public static T Controller()
    {
        if(controller == null)
        {
            GameObject obj = GameObject.Find("MonoController");
            if(obj == null)
                obj = new GameObject("MonoController");
            GameObject.DontDestroyOnLoad(obj);
            
            controller = obj.GetComponent<T>();
            if(controller == null)
                controller = obj.AddComponent<T>();            
        }
        return controller;
    }
}
