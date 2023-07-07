using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Decoration class
/// the decorations that be placed in house
/// </summary>
public class Furniture : Item
{
    public Vector2Int furn_size;
    public bool furn_interact;
    public bool furn_collision;
    public BuildController.FurnitureType furn_type;
    public BuildController.BuildType furn_build_type;
    public UnityAction furn_action;
}