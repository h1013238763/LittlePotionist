using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : BaseController<ItemController>
{
    // Item dictionary, store all item data
    public Dictionary<int, Item> item_dict = new Dictionary<int, Item>();
    // inventory dictionary, store all inventory data
    public Dictionary<string, StoreItem[]> invent_dict = new Dictionary<string, StoreItem[]>();


    /// <summary>
    /// add items into inventory
    /// </summary>
    /// <param name="invent">the inventory to add</param>
    /// <param name="id">the item to add</param>
    /// <param name="num">the number to add</param>
    /// <returns>how many items are added</returns>
    public int AddItem(string invent, int id, int num)
    {   
        if(!invent_dict.ContainsKey(invent))
            return 0;

        int num_re = num;
        foreach(StoreItem item in invent_dict[invent])
        {
            
            if(item.item_id == 0)
            {
                item.item_id = id;
                if(num > item_dict[id].item_stack)
                {
                    item.item_num = item_dict[id].item_stack;
                    num -= item_dict[id].item_stack; 
                } 
                else
                {
                    item.item_num = num;
                    num = 0;
                }
            }
            else if(item.item_id == id && item.item_num < item_dict[id].item_stack)
            {
                int temp = item_dict[id].item_stack - item.item_num;
                if(temp < num)
                {
                    item.item_num += temp;
                    num -= temp;
                }   
                else
                {
                    item.item_num += num;
                    num = 0;
                }
            }
            if(num == 0)
                return num_re;
        }
        return num_re - num;
    }

    /// <summary>
    /// remove items from inventory
    /// </summary>
    /// <param name="invent">the inventory to remove</param>
    /// <param name="id">the item to remove</param>
    /// <param name="num">the number to remove</param>
    /// <returns>how many items are removed</returns>
    public int RemoveItem(string invent, int id, int num)
    {
        if(!invent_dict.ContainsKey(invent))
            return 0;

        int num_re = num;
        foreach(StoreItem item in invent_dict[invent])
        {
            if(item.item_id == id)
            {
                if(item.item_num > num)
                {
                    item.item_num -= num;
                    num = 0;
                }
                else
                {
                    num -= item.item_num;
                    item.item_num = 0;
                    item.item_id = 0;
                }
            }
            if(num == 0)
                return num_re;
        }
        return num_re - num;
    }

    /// <summary>
    /// count the number of target item in inventory
    /// </summary>
    /// <param name="invent">the inventory to check</param>
    /// <param name="id">the target item</param>
    /// <returns>the number of target item</returns>
    public int ItemCount(string invent, int id)
    {
        if(!invent_dict.ContainsKey(invent))
            return 0;

        int num = 0;
        foreach(StoreItem item in invent_dict[invent])
        {
            if(item.item_id == id)
                num += item.item_num;
        }
        return num;
    }

    /// <summary>
    /// check whether there is enough Slots for adding
    /// </summary>
    /// <param name="invent">the inventory to check</param>
    /// <param name="id">item id to check</param>
    /// <param name="num">number to check</param>
    /// <returns>true if there is enough slots</returns>
    public bool CapacityCheck(string invent, int id, int num)
    {
        if(!invent_dict.ContainsKey(invent))
            return false;

        foreach(StoreItem item in invent_dict[invent])
        {
            if(item.item_id == 0 )
                num -= item_dict[id].item_stack;
            else if(item.item_id == id && item.item_num < item_dict[id].item_stack)
                num -= item_dict[id].item_stack - item.item_num;
            
            if(num <= 0)
                return true;
        }
        return false;
    }

    /// <summary>
    /// check whether there is enough items for removing or using
    /// </summary>
    /// <param name="invent">the inventory to check</param>
    /// <param name="id">item id to check</param>
    /// <param name="num">number to check</param>
    /// <returns>true if there is enough items</returns>
    public bool AvailableCheck(string invent, int id, int num)
    {
        if(!invent_dict.ContainsKey(invent))
            return false;

        foreach(StoreItem item in invent_dict[invent])
        {
            if(item.item_id == id )
                num -= item.item_num;
            if(num <= 0)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Add Inventory to inventory dictionary
    /// </summary>
    /// <param name="name">name of gameobject</param>
    /// <param name="invent">the number of slots of the new inventory assign to this object</param>
    public void AddInvent(string name, int num)
    {
        if(!invent_dict.ContainsKey(name))
            invent_dict.Add(name, new StoreItem[num]);
    }
}
