using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : BaseController<ItemController>
{
    // Item dictionary, store all item data
    private XmlDictionary<string, Item> item_dict = new XmlDictionary<string, Item>();
    // inventory dictionary, store all inventory data
    private XmlDictionary<string, StoreItem[]> invent_dict = new XmlDictionary<string, StoreItem[]>();
    // total money player has
    public int wealth;
    


    public void TempTest()
    {
        
    }

    /// <summary>
    /// Initial the Item Dictionary
    /// </summary>
    public void InitialItemDict()
    {
        item_dict = XmlController.Controller().LoadData(typeof(XmlDictionary<string, Item>), "PotionistItems", "Data/Items/") as XmlDictionary<string, Item>;
    }

    public void InitialInvent(XmlDictionary<string, StoreItem[]> info)
    {
        invent_dict = info;
    }

    public XmlDictionary<string, StoreItem[]> GetInventInfo()
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
        StoreItem add_item = new StoreItem(id, num);

        if(!invent_dict.ContainsKey(invent_name))
            return 0;
        StoreItem[] invent = invent_dict[invent_name];

        for(int i = 0; i < invent.Length; i ++)
        {
            if(invent[i] == null)
            {
                invent[i] = new StoreItem();
                invent[i].item_id = id;
                invent[i].item_num += add_item.item_num;
                OverstackCheck(add_item, invent[i]);              
            }
            else if(invent[i].item_id == id && invent[i].item_num < item_dict[id].item_stack)
            {
                invent[i].item_num += add_item.item_num;
                OverstackCheck(add_item, invent[i]);
            }
            if(add_item.item_num <= 0)
            {
                break;
            }     
        }
        RefreshGUI(invent_name);
        return ( add_item.item_num <= 0)? num : num - add_item.item_num;
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
            {
                break;
            }   
        }
        RefreshGUI(invent_name);
        return ( num <= 0)? num_re : num_re - num;
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

        // remove item
        RemoveItem("QuickSlot", invent_dict["QuickSlot"][index].item_id, 1);
    }

    /// <summary>
    /// move the item from "from" invent to "to" invent
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="from_index"></param>
    /// <param name="to_index"></param>
    public void TransferItem(string from, string to, int from_index, int to_index = -1)
    {
        // inventory exist check
        if( !invent_dict.ContainsKey(from) || !invent_dict.ContainsKey(to) )
            return;
        // get item info and item exist check
        if(invent_dict[from][from_index] == null)
            return;
        Item item = item_dict[invent_dict[from][from_index].item_id];



        // quick move
        if(to_index == -1)
        {
            if(to == "QuickSlot")   // move to quick slot ( quick fill or set up )
            {
                for(int i = 0; i < 8; i ++) // try to find same item
                {
                    if(invent_dict[to][i] == null)
                    {
                        if(to_index == -1)
                            to_index = i;
                        else
                            continue;
                    }
                    else if(invent_dict[to][i].item_id == item.item_id)
                    {
                        to_index = i;
                        break;
                    }   
                }
                
                if(to_index == -1)    // if no slot available
                    return;

                // assign and set slot
                if(invent_dict[to][to_index] == null)
                    invent_dict[to][to_index] = new StoreItem();
                invent_dict[to][to_index].item_id = item.item_id;
                invent_dict[to][to_index].item_num += invent_dict[from][from_index].item_num;
                OverstackCheck(invent_dict[from][from_index], invent_dict[to][to_index]);
            }
            else    // quick move item to other inventory
            {
                invent_dict[from][from_index].item_num -= AddItem(to, item.item_id, invent_dict[from][from_index].item_num);
            }
        }
        // drag add or swap
        else
        {   // if they are same item
            if(invent_dict[from][from_index].item_id == invent_dict[to][to_index].item_id)
            {
                invent_dict[to][to_index].item_num += invent_dict[from][from_index].item_num;
                OverstackCheck(invent_dict[from][from_index], invent_dict[to][to_index]);
            }
            else
            {
                StoreItem swap_temp = invent_dict[from][from_index];
                invent_dict[from][from_index] = invent_dict[to][to_index];
                invent_dict[to][to_index] = swap_temp;
            }
        }

        if(invent_dict[from][from_index].item_num == 0)
            invent_dict[from][from_index] = null;

        RefreshGUI(from);
        RefreshGUI(to);
    }


    /// <summary>
    /// create a target item with given num at given position, if is drop, preform drop moving animation
    /// </summary>
    /// <param name="id"></param>
    /// <param name="num"></param>
    /// <param name="pos"></param>
    /// <param name="drop"></param>
    public void CreateDropItem(string id, int num, Vector3 pos)
    {
        if( !item_dict.ContainsKey(id))
            return;

        GameObject temp = PoolController.Controller().GetObject("DropItem");

        // set sprite
        SpriteRenderer renderer = temp.GetComponent<SpriteRenderer>();
        if(renderer == null)
            renderer = temp.AddComponent<SpriteRenderer>();
        renderer.sprite = GetItemSprite(id);

        // set StoreItem component
        DropItem item = temp.GetComponent<DropItem>();
        if(item == null)
            item = temp.AddComponent<DropItem>();
        item.SetItem(id, num);

        // collider check
        if(temp.GetComponent<BoxCollider2D>() == null)
            temp.AddComponent<BoxCollider2D>().isTrigger = true;

        // set position
        temp.transform.position = pos;
    }

    /// <summary>
    /// Pick up a on ground item to player invent
    /// </summary>
    /// <param name="item">target item</param>
    public void PickDropItem(DropItem drop)
    {
        drop.item.item_num -= AddItem("Player", drop.item.item_id, drop.item.item_num);
        if(drop.item.item_num <= 0 )
            PoolController.Controller().PushObject("DropItem", drop.gameObject);
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
        return ResourceController.Controller().Load<Sprite>("ItemImage/"+id);
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
    /// Handle item number over stack situation
    /// </summary>
    /// <param name="from">from store item</param>
    /// <param name="to">to store item</param>
    /// <returns>new "to" item number</returns>
    public void OverstackCheck(StoreItem from, StoreItem to)
    {
        int over_stack = to.item_num - item_dict[to.item_id].item_stack;
        if(over_stack > 0)
        {
            to.item_num = item_dict[to.item_id].item_stack;
            from.item_num = over_stack;
        }
        else
        {
            from.item_num = 0;
        }
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

    /// <summary>
    /// Refresh Inventory related GUI
    /// </summary>
    private void RefreshGUI(string invent)
    {
        switch(invent)
        {
            case "Player":
                if(GUIController.Controller().GetPanel<InventoryPanel>("InventoryPanel") != null)
                    GUIController.Controller().GetPanel<InventoryPanel>("InventoryPanel").Refresh();
                break;
            case "QuickSlot":
                if(GUIController.Controller().GetPanel<GeneralPanel>("GeneralPanel") != null)
                    GUIController.Controller().GetPanel<GeneralPanel>("GeneralPanel").Refresh();
                break;
            default:
                break;
        }
        // if inventory panel is showing, refresh it
        
    }
}
