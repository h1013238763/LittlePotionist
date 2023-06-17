using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string item_id;
    public string item_name;
    public string item_describe;
    public int item_stack;
    public int item_price;
    public bool item_usable;

    public virtual void UseEffect(){}

    public override string ToString()
    {
        return "Item[ id="+item_id+" ]";
    }
}
