using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : BaseController<ItemController>
{
    // Item dictionary, store all item data
    private XmlDictionary<string, Item> item_dict = new XmlDictionary<string, Item>();
    // inventory dictionary, store all inventory data
    private XmlDictionary<string, StoreItem[]> invent_dict = new XmlDictionary<string, StoreItem[]>();

    public int wealth;


    public void Initial()
    {
        Item temp_1 = new Item();
        temp_1.item_id = "Potionist: Bed";
        temp_1.item_name = "Bed";
        temp_1.item_describe = "A nice bed";
        temp_1.item_stack = 1;
        temp_1.item_price = 100;
        temp_1.item_usable = true;
        item_dict.Add(temp_1.item_id, temp_1);

        Item temp_2 = new Item();
        temp_2.item_id = "Potionist: Chair";
        temp_2.item_name = "Chair";
        temp_2.item_describe = "A nice Chair";
        temp_2.item_stack = 1;
        temp_2.item_price = 100;
        temp_2.item_usable = true;
        item_dict.Add(temp_2.item_id, temp_2);

        Item temp_3 = new Item();
        temp_3.item_id = "Potionist: Seed";
        temp_3.item_name = "Seed";
        temp_3.item_describe = "A nice Seed";
        temp_3.item_stack = 20;
        temp_3.item_price = 25;
        temp_3.item_usable = true;
        item_dict.Add(temp_3.item_id, temp_3);

        XmlController.GetController().SaveData(item_dict, "PotionistItems", "Item/");
    }

    public XmlDictionary<string, StoreItem[]> GetInventInfo()
    {
        return invent_dict;
    }

    public XmlDictionary<string, StoreItem[]> InventData()
    {
        return invent_dict;
    }

    /// <summary>
    /// add items into inventory
    /// </summary>
    /// <param name="invent">the inventory to add</param>
    /// <param name="id">the item to add</param>
    /// <param name="num">the number to add</param>
    /// <returns>how many items are added</returns>
    public int AddItem(string invent_name, string id, int num)
    {   
        if(!invent_dict.ContainsKey(invent_name))
            return 0;
        StoreItem[] invent = invent_dict[invent_name];

        int num_re = num;
        for(int i = 0; i < invent.Length; i ++)
        {
            
            if(invent[i] == null)
            {
                invent[i] = new StoreItem();
                invent[i].item_id = id;
                if(num > item_dict[id].item_stack)
                {
                    invent[i].item_num = item_dict[id].item_stack;
                    num -= item_dict[id].item_stack; 
                } 
                else
                {
                    invent[i].item_num = num;
                    num = 0;
                }
            }
            else if(invent[i].item_id == id && invent[i].item_num < item_dict[id].item_stack)
            {
                int temp = item_dict[id].item_stack - invent[i].item_num;
                if(temp < num)
                {
                    invent[i].item_num += temp;
                    num -= temp;
                }   
                else
                {
                    invent[i].item_num += num;
                    num = 0;
                }
            }
            if(num <= 0)
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
    public int RemoveItem(string invent_name, string id, int num)
    {
        if(!invent_dict.ContainsKey(invent_name))
            return 0;
        StoreItem[] invent = invent_dict[invent_name];

        int num_re = num;
        for(int i = 0; i < invent.Length; i ++)
        {
            if(invent[i] == null)
                continue;
            if(invent[i].item_id == id)
            {
                if(invent[i].item_num > num)
                {
                    invent[i].item_num -= num;
                    num = 0;
                }
                else
                {
                    num -= invent[i].item_num;
                    invent[i] = null;
                }
            }
            if(num <= 0)
                return num_re;
        }
        return num_re - num;
    }

    /// <summary>
    /// player use the item in quick slot
    /// </summary>
    /// <param name="index">the index of quick slot</param>
    public void UseItem(int index)
    {
        // check if anything to use
        if(invent_dict["QuickSlot"][index] == null)
            return;

        // trigger use event
        string id = invent_dict["QuickSlot"][index].item_id;
        EventController.GetController().EventTrigger("Item/" + id);

        // remove item
        RemoveItem("QuickSlot", id, 1);
    }

    /// <summary>
    /// Return the item for other class by its id
    /// </summary>
    /// <param name="id">the id of item</param>
    /// <returns>target item</returns>
    public Item GetItemInfo(string id)
    {
        if(item_dict.ContainsKey(id))
            return item_dict[id];
        return null;
    }

    /// <summary>
    /// return the sprite of item for other class by its id
    /// </summary>
    /// <param name="id">the id of item</param>
    /// <returns>target item sprite</returns>
    public Sprite GetItemSprite(string id)
    {
        return ResourceController.GetController().Load<Sprite>("ItemImage/"+id);
    }

    /// <summary>
    /// count the number of target item in inventory
    /// </summary>
    /// <param name="invent">the inventory to check</param>
    /// <param name="id">the target item</param>
    /// <returns>the number of target item</returns>
    public int ItemCount(string invent_name, string id)
    {
        if(!invent_dict.ContainsKey(invent_name))
            return 0;
        StoreItem[] invent = invent_dict[invent_name];

        int num = 0;
        for(int i = 0; i < invent.Length; i ++)
        {
            if(invent[i].item_id == id)
                num += invent[i].item_num;
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
    public bool CapacityCheck(string invent_name, string id, int num)
    {
        if(!invent_dict.ContainsKey(invent_name))
            return false;
        StoreItem[] invent = invent_dict[invent_name];

        for(int i = 0; i < invent.Length; i ++)
        {
            if(invent[i] == null)
                num -= item_dict[id].item_stack;
            else if(invent[i].item_id == id && invent[i].item_num < item_dict[id].item_stack)
                num -= item_dict[id].item_stack - invent[i].item_num;
            
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
    public bool AvailableCheck(string invent_name, string id, int num)
    {
        if(!invent_dict.ContainsKey(invent_name))
            return false;
        StoreItem[] invent = invent_dict[invent_name];

        for(int i = 0; i < invent.Length; i ++)
        {
            if(invent[i] == null)
                continue;
            else if(invent[i].item_id == id )
                num -= invent[i].item_num;

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

    /// <summary>
    /// Get Inventory by its name
    /// </summary>
    /// <param name="name">the name of inventory</param>
    /// <returns>target inventory</returns>
    public StoreItem[] GetInvent(string name)
    {
        if(invent_dict.ContainsKey(name))
            return invent_dict[name];
        return null;
    }
}
