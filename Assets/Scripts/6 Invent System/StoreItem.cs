using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItem : MonoBehaviour
{
    public int item_id;
    public int item_num;

    public StoreItem()
    {
        item_id = -1;
        item_num = 0;
    }

    public StoreItem(StoreItem item)
    {
        item_id = item.item_id;
        item_num = item.item_num;
    }

    public StoreItem(int id, int num)
    {
        item_id = id;
        item_num = num;
    }
}
