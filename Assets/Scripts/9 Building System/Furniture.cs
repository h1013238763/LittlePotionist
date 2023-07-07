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

    public Furniture(Item item)
    {
        this.item_id = item.item_id;
        this.item_name = item.item_name;
        
        this.item_describe = item.item_name;
        this.item_stack = item.item_stack;
        this.item_price = item.item_price;
        this.item_usable = item.item_usable;
        this.item_type = item.item_type;
        this.item_info = null;
        
        // this.item_sprite;
    }
}