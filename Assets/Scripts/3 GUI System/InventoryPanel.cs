using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryPanel : PanelBase
{
    StoreItem pointer_item;
    string current_storage;

    public override void ShowSelf()
    {   
        Button[] ctrls = this.GetComponentsInChildren<Button>();

        for(int i = 0; i < ctrls.Length; i++)
        {
            {
                GUIController.AddCustomEventListener(ctrls[i], EventTriggerType.PointerEnter, (data) => {OnPointerEnter((PointerEventData)data); });
                GUIController.AddCustomEventListener(ctrls[i], EventTriggerType.PointerExit,  (data) => {OnPointerExit ((PointerEventData)data); });
                GUIController.AddCustomEventListener(ctrls[i], EventTriggerType.PointerClick, (data) => {OnPointerClick ((PointerEventData)data); });
            }
        }
        Refresh();
    }

    /// <summary>
    /// Refresh item icon and num of inventory
    /// </summary>
    public void Refresh()
    {
        StoreItem[] invent = ItemController.Controller().GetInvent(current_storage);     

        for(int i = 0; i < invent.Length; i ++)
        {
            Transform slot = FindComponent<Button>("Slot (" + i + ")").transform;

            if(invent[i] == null)
            {
                slot.GetChild(0).gameObject.SetActive(false);
                slot.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                // set component visibility
                slot.GetChild(0).gameObject.SetActive(true);
                if( invent[i].item_num > 1 )
                    slot.GetChild(1).gameObject.SetActive(true);

                // set component data
                slot.GetChild(0).gameObject.GetComponent<Image>().sprite = ItemController.Controller().GetItemSprite(invent[i].item_id);
                slot.GetChild(1).gameObject.GetComponent<Text>().text = invent[i].item_num.ToString();
            }
        }
    }

    public void SetStorage(string name)
    {
        current_storage = name;
    }

    /// <summary>
    /// Show Item details while pointer in
    /// </summary>
    /// <param name="event_data">pointer data</param>
    private void OnPointerEnter(PointerEventData event_data)
    {
        int index = GetPointerObjectIndex(event_data);
        if(index < 0)
            return;
        
        pointer_item = ItemController.Controller().GetInvent("Player")[index];
        if(pointer_item == null)
            return;

        GUIController.Controller().ShowPanel<ItemDetailPanel>("ItemDetailPanel", 3);
        GUIController.Controller().GetPanel<ItemDetailPanel>("ItemDetailPanel").SetItem(pointer_item.item_id);
    }

    /// <summary>
    /// hide item detail panel when pointer out
    /// </summary>
    /// <param name="event_data">pointer data</param>
    private void OnPointerExit(PointerEventData event_data)
    {
        pointer_item = null;
        GUIController.Controller().HidePanel("ItemDetailPanel");
    }

    // TODO: Fix InventoryPanel make it able to use by other storage
    /// <summary>
    /// quick move item to quick slot or another invent when mouse right click
    /// </summary>
    /// <param name="event_data">pointer data</param>
    private void OnPointerClick(PointerEventData event_data)
    {
        int index = GetPointerObjectIndex(event_data);
        if(index < 0)
            return;

        if(event_data.button == PointerEventData.InputButton.Right )
        {
            InventoryPanel storage = GUIController.Controller().GetPanel<InventoryPanel>("StoragePanel");
            if( storage == null)
                ItemController.Controller().TransferItem("Player", "QuickSlot", index);
            else
                ItemController.Controller().TransferItem("Player", "Storage", index);
        }
    }

    /// <summary>
    /// break pointer event data into inventory index
    /// </summary>
    /// <param name="event_data"></param>
    /// <returns></returns>
    private int GetPointerObjectIndex(PointerEventData event_data)
    {
        string name = event_data.pointerEnter.name;
        if(name == "Sprite" || name == "Num")
            name = event_data.pointerEnter.transform.parent.gameObject.name;
        if(name.IndexOf(" ") < 0)
            return -1;
        return int.Parse( name.Substring(name.IndexOf("(")+1, 1) );
    }
}
