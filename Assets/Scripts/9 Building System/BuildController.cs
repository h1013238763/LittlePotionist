using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class BuildController : BaseController<BuildController>
{

    private XmlDictionary<string, Furniture> furniture_list = new XmlDictionary<string, Furniture>();

    /// <summary>
    /// Initial the building controller
    /// </summary>
    /// <param name="map_name"></param>
    public void Initial()
    {
        
    }

    public void PlanFurniture(Furniture deco, Vector3Int pos)
    {

    }

    /// <summary>
    /// create a gameobject of target decoration and name it by its id
    /// </summary>
    /// <param name="deco"></param>
    public void PlaceFurniture(Furniture deco, Vector3Int pos)
    {
        
    }

    public void RemoveFurniture(GameObject deco)
    {

    }

    /// <summary>
    /// use object name as id to locate the decoration interaction functions and trigger
    /// </summary>
    /// <param name="deco"></param>
    public void UseFurniture(GameObject deco)
    {

    }

    public enum BuildType
    {
        Floor = 0,
        Wall = 1,
        Ground = 2,
        Decoration = 3
    }

    public enum FurnitureType
    {
        Carpet = 0,
        Mural = 1,
        Window = 2,
        Light = 3,
        Chair = 4,
        Table = 5,
        Storage = 6,
        Shelf = 7,
        Bed = 8
    }
}