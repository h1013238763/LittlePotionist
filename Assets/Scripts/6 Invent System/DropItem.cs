using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public StoreItem item = new StoreItem();

    public void SetItem(string id, int num)
    {
        item.item_id = id;
        item.item_num = num;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        ItemController.Controller().PickDropItem(this);
    }
}
