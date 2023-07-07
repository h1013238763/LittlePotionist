using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string item_id;
    public string item_name;
    public Sprite item_sprite;
    public string item_describe;
    public int item_stack;
    public int item_price;
    public bool item_usable;
    public ItemType item_type;
    public string item_info;

    public virtual void UseEffect(){}

    public override string ToString()
    {
        return "Item[ id="+item_id+" ]";
    }
}

public enum ItemType
{
    Material,
    Seed,
    Tool,
    Herb,
    Potion,
    Furniture
}