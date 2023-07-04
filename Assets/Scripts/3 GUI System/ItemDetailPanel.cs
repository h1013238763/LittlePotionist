using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailPanel : PanelBase
{
    Item item;

    public override void ShowSelf()
    {
        gameObject.SetActive(true);
    }

    public void SetItem(string id)
    {
        item = ItemController.Controller().GetItemInfo(id);
        FindComponent<Text>("ItemName").text = item.item_name;
        FindComponent<Text>("ItemType").text = item.item_type.ToString();
        FindComponent<Text>("ItemDescribe").text = item.item_describe;
    }
}