using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : BaseController<ItemController>
{
    // Item dictionary, store all item data
    private XmlDictionary<string, Item> item_dict = new XmlDictionary<string, Item>();
    // inventory dictionary, store all inventory data
    private XmlDictionary<string, StoreItem[]> invent_dict = new XmlDictionary<string, StoreItem[]>();
    // item hold in pointer
    public StoreItem hold_item;
    // total money player has
    public int wealth;

    /// <summary>
    /// Initial the Item Dictionary
    /// </summary>
    public void InitialItemDict(XmlDictionary<string, Item> items)
    {
        item_dict = items;
        foreach(var item in item_dict)
            Debug.Log(item);
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
        EventController.Controller().EventTrigger("ItemController/ItemChange");
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
        EventController.Controller().EventTrigger("ItemController/ItemChange");
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
    /// Transfer Item between inventorys
    /// </summary>
    public void TransferItem(string invent, int index, bool all)
    {
        // pick one item
        if(hold_item == null)
        {
            if(!SlotCheck(invent, index))    // no item to interact with
                return;
            
            // play sound
            AudioController.Controller().PlaySound("SliderDrag");

            if(all) // pick all item
            {
                hold_item = invent_dict[invent][index];
                invent_dict[invent][index] = null;
            }
            else    // pick one item
            {
                hold_item = invent_dict[invent][index];
                hold_item.item_num = 1;
                invent_dict[invent][index].item_num --;
                if(invent_dict[invent][index].item_num <= 0)
                    invent_dict[invent][index] = null;
            }
        }
        else
        {
            if(invent == "World")// drop item into world
            {
                // get player face position
                bool left = PlayerController.Controller().gameObject.GetComponent<SpriteRenderer>().flipX;
                Vector3 pos = PlayerController.Controller().transform.position;
                pos += (left) ? new Vector3(-4, 0, 0) : new Vector3(4, 0, 0);
                // drop item into world
                CreateDropItem(hold_item.item_id, hold_item.item_num, pos);
                // remove item in hold
                hold_item = null;
            }
            else if(all)    // try to drop all item into this slot
            {
                // play sound
                AudioController.Controller().PlaySound("SliderDrag");

                // if slot is empty
                if(invent_dict[invent][index] == null)
                {
                    invent_dict[invent][index] = hold_item;
                    hold_item = null;
                }
                else if(invent_dict[invent][index].item_num <= 0)
                {
                    invent_dict[invent][index] = hold_item;
                    hold_item = null;
                }
                // if slot contains same item
                else if(invent_dict[invent][index].item_id == hold_item.item_id)
                {
                    invent_dict[invent][index].item_num += hold_item.item_num;
                    OverstackCheck(hold_item, invent_dict[invent][index]);
                    if(hold_item.item_num <= 0)
                        hold_item = null;
                }
            }
            // pick one more same item
            else if(invent_dict[invent][index] != null)
            {
                // play sound
                AudioController.Controller().PlaySound("SliderDrag");

                if(invent_dict[invent][index].item_id == hold_item.item_id && invent_dict[invent][index].item_num > 0)
                {
                    hold_item.item_num ++;
                    invent_dict[invent][index].item_num --;
                    if(invent_dict[invent][index].item_num <= 0)
                        invent_dict[invent][index] = null;
                }
            }
        }
        EventController.Controller().EventTrigger("ItemController/ItemChange");
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

    public Item GetItemInfo(string invent, int index)
    {
        if(SlotCheck(invent, index))
            return GetItemInfo(invent_dict[invent][index].item_id);
        else
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
    /// check if the target slot contains any item
    /// </summary>
    /// <param name="invent">target invent</param>
    /// <param name="index">target slot of invent</param>
    public bool SlotCheck(string invent, int index)
    {
        // invent check
        if(!invent_dict.ContainsKey(invent))
            return false;
        // index check
        if(index < 0 || index >= invent_dict[invent].Length)
            return false;
        // slot check
        if(invent_dict[invent][index] == null)
            return false;
        // slot item num check
        if(invent_dict[invent][index].item_num <= 0)
            return false;

        // pass all checks
        return true;
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
