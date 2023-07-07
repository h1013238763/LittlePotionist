using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BottomPanel : PanelBase
{
    private int current_slot = -1;        // the index of current using quick slot
    private Dictionary<string, Vector3> button_pos = new Dictionary<string, Vector3>();

    public bool invent_open;    // if player using inventory now

    public override void ShowSelf()
    {
        ItemController.Controller().AddInvent("QuickSlot", 8);

        invent_open = false;

        Button[] ctrls = this.GetComponentsInChildren<Button>();

        for(int i = 0; i < ctrls.Length; i++)
        {
            GUIController.AddCustomEventListener(ctrls[i], EventTriggerType.PointerClick, (data) => {OnPointerClick((PointerEventData)data); });
            GUIController.AddCustomEventListener(ctrls[i], EventTriggerType.PointerEnter, (data) => {OnPointerEnter((PointerEventData)data); });
            GUIController.AddCustomEventListener(ctrls[i], EventTriggerType.PointerExit,  (data) => {OnPointerExit ((PointerEventData)data); });
        }

        FindComponent<Text>("SlotTip").gameObject.SetActive(false);
    
        EventController.Controller().AddEventListener("ItemController/ItemChange", RefreshQuickSlot);
        EventController.Controller().AddEventListener("WorldController/NextDay", RefreshDateText);
        EventController.Controller().AddEventListener("WorldController/TimeChange", RefreshTimeText);
    }

    /// <summary>
    /// Handle quick slot on click event
    /// </summary>
    /// <param name="button_name">name of button</param>
    private void ClickQuickSlot(string button_name)
    {
        // check previous slot and assign current interact slot
        string temp_curr = "Slot (" + current_slot.ToString() + ")";
        current_slot = int.Parse(button_name.Substring(6,1));

        if(temp_curr != button_name)
        {
            // play sound
            AudioController.Controller().PlaySound("SliderDrag");

            // slot move animation
            TweenController.Controller().MoveToPosition(
                FindComponent<Button>(button_name).transform, 
                new Vector3(button_pos[button_name].x, button_pos[button_name].y*1.2f, button_pos[button_name].z), 
                0.07f, true);
            if(button_pos.ContainsKey(temp_curr))    // move back to origin position
                TweenController.Controller().MoveToPosition(
                    FindComponent<Button>(temp_curr).transform, button_pos[temp_curr], 0.07f, true);
        }  
    }

    /// <summary>
    /// While inventory is open, click to transfer items
    /// </summary>
    /// <param name="button_name"></param>
    /// <param name="all"></param>
    private void ClickQuickSlotTransfer(string button_name, bool all)
    {
        // assign item to item controller hold item
        int index = int.Parse(button_name.Substring(6,1));
        ItemController.Controller().TransferItem("QuickSlot", index, all);
    }

    /// <summary>
    /// Handle menu button on click event
    /// </summary>
    /// <param name="button_name">name of button</param>
    private void ClickMenu(string button_name)
    {
        // play sound
        AudioController.Controller().PlaySound("ButtonClick");

        // find target button, close panel if it's open
        switch(button_name)
        {
            // character panel action
            case "CharacterPanel":
                ItemController.Controller().TempTest();
                ItemController.Controller().CreateDropItem("Potionist@Seed", 1, new Vector3(1, 1, 0));
                break;
            // inventory panel action
            case "InventoryPanel":
                if(GUIController.Controller().GetPanel<InventoryPanel>("InventoryPanel") != null)
                    GUIController.Controller().HidePanel("InventoryPanel");
                else
                    GUIController.Controller().ShowPanel<InventoryPanel>("InventoryPanel", 2, (p) =>
                    {
                        p.storage_name = "Player";
                    });
                break;
            // map panel action
            case "MapPanel":
                break;
            // setting panel action
            case "SettingPanel":
                if(GUIController.Controller().GetPanel<SettingPanel>("SettingPanel") != null)
                    GUIController.Controller().GetPanel<SettingPanel>("SettingPanel").HideSelf();
                else
                    GUIController.Controller().ShowPanel<SettingPanel>("SettingPanel", 2, (p) =>
                    {
                        p.start_scene = false;
                    });
                break;
            default:
                break;
        }   
    }

    /// <summary>
    /// refresh all quick slot gui
    /// </summary>
    private void RefreshQuickSlot()
    {
        StoreItem[] invent = ItemController.Controller().GetInvent("QuickSlot");   

        for(int i = 0; i < invent.Length; i ++)
        {
            Transform slot = FindComponent<Button>("Slot (" + i + ")").transform;

            if(invent[i] == null)   // hide empty item
            {
                slot.GetChild(0).gameObject.SetActive(false);
                slot.GetChild(1).gameObject.SetActive(false);
            }
            else    // show exist item
            {
                slot.GetChild(0).gameObject.SetActive(true);
                if(invent[i].item_num > 1)
                    slot.GetChild(1).gameObject.SetActive(true);
                slot.GetChild(0).gameObject.GetComponent<Image>().sprite = ItemController.Controller().GetItemSprite(invent[i].item_id);
                slot.GetChild(1).gameObject.GetComponent<Text>().text = invent[i].item_num.ToString();
            }
        }
    }

    /// <summary>
    /// Refresh date text
    /// </summary>
    private void RefreshDateText()
    {
        FindComponent<Text>("DateText").text = WorldController.Controller().GetDateText();
    }

    /// <summary>
    /// Refresh time text
    /// </summary>
    private void RefreshTimeText()
    {
        FindComponent<Text>("TimeText").text = WorldController.Controller().GetTimeText();
    }

    /// <summary>
    /// a interface for player controller to use and trigger
    /// </summary>
    /// <param name="button_name"></param>
    public void KeyboardInput(string button_name)
    {
        if(button_name == "Axis:/Mouse/scroll/up")
        {
            int temp_slot = current_slot;
            temp_slot --;
            if(temp_slot < 0)
                temp_slot = 7;
            button_name = "Slot ("+temp_slot.ToString()+")";
        }
        else if(button_name == "Axis:/Mouse/scroll/down")
        {
            int temp_slot = current_slot;
            temp_slot ++;
            if(temp_slot > 7)
                temp_slot = 0;
            button_name = "Slot ("+temp_slot.ToString()+")";
        }

        if(!button_pos.ContainsKey(button_name))
            button_pos.Add(button_name, FindComponent<Button>(button_name).transform.position);

        if(button_name.IndexOf(" ") > 0)
            ClickQuickSlot(button_name);
        else
            ClickMenu(button_name);
    }

    /// <summary>
    /// Pointer Click Event
    /// </summary>
    private void OnPointerClick(PointerEventData event_data)
    {
        string name = GetPointerComponenetName(event_data);

        if(!button_pos.ContainsKey(name))
            button_pos.Add(name, FindComponent<Button>(name).transform.position);

        // if click a quick slot
        if(name.IndexOf(" ") > 0)
        {
            if(invent_open)
                ClickQuickSlotTransfer(name, event_data.button == PointerEventData.InputButton.Left);
            else
                ClickQuickSlot(name);
        }                  
        else
        {
            ClickMenu(name);
        }
    }

    /// <summary>
    /// Pointer Enter Event
    /// </summary>
    private void OnPointerEnter(PointerEventData event_data)
    {
        // break event into component name
        string name = GetPointerComponenetName(event_data);

        // regist origin position
        if(!button_pos.ContainsKey(name))
            button_pos.Add(name, FindComponent<Button>(name).transform.position);

        // component animation
        if(name != "")
        {
            if(current_slot.ToString() != name.Substring(6, 1))
                AudioController.Controller().PlaySound("SliderDrag");

            TweenController.Controller().MoveToPosition(
                FindComponent<Button>(name).transform, 
                new Vector3(button_pos[name].x, button_pos[name].y*1.2f, button_pos[name].z), 
                0.07f, true);
        }
            
        // componet tip
        Text slot_tip = FindComponent<Text>("SlotTip");
        slot_tip.gameObject.SetActive(true);
        slot_tip.transform.position = new Vector3( button_pos[name].x, button_pos[name].y*2.5f, 0);

        if(name.IndexOf(" ") > 0)    // if it's a quick slot
        {
            int index = int.Parse(name.Substring(6,1));
            Item item = ItemController.Controller().GetItemInfo("QuickSlot", index);

            slot_tip.text = (item == null) ? "" : item.item_name;
        }
        else
        {
            slot_tip.text = name.Substring(0, name.IndexOf("P"));
        }
    }

    /// <summary>
    /// Pointer Exit Event
    /// </summary>
    private void OnPointerExit(PointerEventData event_data)
    {
        // break event into component name
        string name = GetPointerComponenetName(event_data);

        if(name == "Slot (" + current_slot.ToString() + ")")
            return;
        // component animation
        if(name != "")
            TweenController.Controller().MoveToPosition(
                FindComponent<Button>(name).transform, button_pos[name], 0.07f, true);

        // hide component tip
        FindComponent<Text>("SlotTip").gameObject.SetActive(false);
    }

    /// <summary>
    /// avoid null-reference bug
    /// </summary>
    /// <param name="event_data"></param>
    /// <returns></returns>
    private string GetPointerComponenetName(PointerEventData event_data)
    {
        string name = event_data.pointerEnter.name;

        if(FindComponent<Button>(name) == null)
        {
            name = FindComponent<Text>(name).transform.parent.parent.gameObject.name;
        }
        
        return (FindComponent<Button>(name) != null) ? name : "";
    }
}
