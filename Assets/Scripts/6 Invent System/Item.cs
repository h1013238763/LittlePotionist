using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int item_id;
    public Sprite item_sprite;
    public string item_name;
    public string item_describe;
    public int item_stack;
    public int item_price;
    public bool item_usable;

    public virtual void UseEffect(){}
}
