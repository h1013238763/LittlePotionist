using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryPanel : PanelBase
{
    Item pointer_item;
    public string storage_name;

    public override void ShowSelf()
    {   
        Button[] ctrls = this.GetComponentsInChildren<Button>();

        for(int i = 0; i < ctrls.Length; i++)
        {
            GUIController.AddCustomEventListener(ctrls[i], EventTriggerType.PointerClick, (data) => {OnPointerClick((PointerEventData)data); });
            GUIController.AddCustomEventListener(ctrls[i], EventTriggerType.PointerEnter, (data) => {OnPointerEnter((PointerEventData)data); });
            GUIController.AddCustomEventListener(ctrls[i], EventTriggerType.PointerExit,  (data) => {OnPointerExit ((PointerEventData)data); });
        }

        if(storage_name == "Player")
        {
            FindComponent<Image>("PointerItem").gameObject.SetActive(false);
            GUIController.Controller().GetPanel<BottomPanel>("BottomPanel").invent_open = true;
        }
        else
        {
            GUIController.Controller().ShowPanel<InventoryPanel>("InventoryPanel", 2, (p) =>
            {
                p.storage_name = "Player";
            });
        }

        EventController.Controller().AddEventListener("ItemController/ItemChange", Refresh);

        Refresh();
    }

    public override void HideSelf()
    {
        if(storage_name == "Player")
        {
            GUIController.Controller().GetPanel<BottomPanel>("BottomPanel").invent_open = false;
            GUIController.Controller().RemovePanel("StoragePanel");
        }

        GUIController.Controller().RemovePanel(gameObject.name);
    }

    /// <summary>
    /// Refresh item icon and num of inventory
    /// </summary>
    public void Refresh()
    {
        StoreItem[] invent = ItemController.Controller().GetInvent(storage_name);     

        // set all slot items
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

        // set hold item image
        if(storage_name == "Player")
        {
            StoreItem item = ItemController.Controller().hold_item;
            if(item == null)
                return;
            if(item.item_num > 0)
                FindComponent<Image>("PointerItem").sprite = ItemController.Controller().GetItemSprite(item.item_id);
        }   
    }

    /// <summary>
    /// Transfer item
    /// </summary>
    /// <param name="event_data"></param>
    private void OnPointerClick(PointerEventData event_data)
    {
        int index = GetPointerObjectIndex(event_data);
        ItemController.Controller().TransferItem(storage_name, index, event_data.button == PointerEventData.InputButton.Left);
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
        
        pointer_item = ItemController.Controller().GetItemInfo(storage_name, index);
        if(pointer_item != null)
        {
            GUIController.Controller().ShowPanel<ItemDetailPanel>("ItemDetailPanel", 3, (p) =>
            {
                p.SetItem(pointer_item.item_id);
            });
        }
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
