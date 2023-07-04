using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GeneralPanel : PanelBase
{
    private int current_slot = -1;        // the index of current using quick slot
    private Dictionary<string, Vector3> button_pos = new Dictionary<string, Vector3>();
    
    protected override void Awake()
    {
        base.Awake();
        ShowSelf();
    }

    public override void ShowSelf()
    {
        ItemController.Controller().AddInvent("QuickSlot", 8);

        Button[] ctrls = this.GetComponentsInChildren<Button>();

        for(int i = 0; i < ctrls.Length; i++)
        {
            GUIController.AddCustomEventListener(ctrls[i], EventTriggerType.PointerEnter, (data) => {OnPointerEnter((PointerEventData)data); });
            GUIController.AddCustomEventListener(ctrls[i], EventTriggerType.PointerExit,  (data) => {OnPointerExit ((PointerEventData)data); });
            GUIController.AddCustomEventListener(ctrls[i], EventTriggerType.PointerClick, (data) => {OnPointerClick ((PointerEventData)data); });
            GUIController.AddCustomEventListener(ctrls[i], EventTriggerType.BeginDrag,    (data) => {OnBeginDrag ((PointerEventData)data); });
            GUIController.AddCustomEventListener(ctrls[i], EventTriggerType.EndDrag,      (data) => {OnEndDrag ((PointerEventData)data); });
        }

        // debug temp
        if( !GUIController.Controller().panel_dic.ContainsKey("GeneralPanel") )
            GUIController.Controller().panel_dic.Add("GeneralPanel", this);
        

        Refresh();
    }

    public void Use()
    {
        ItemController.Controller().UseItem(current_slot);
    }

    /// <summary>
    /// Button Click Event
    /// </summary>
    /// <param name="button_name"></param>
    protected override void OnButtonClick(string button_name)
    {
        // recond button origin position
        if(!button_pos.ContainsKey(button_name))
            button_pos.Add(button_name, FindComponent<Button>(button_name).transform.position);
        
        

        
        // if click a quick slot
        if(button_name.IndexOf(" ") > 0)
        {   
            AudioController.Controller().PlaySound("SliderDrag");

            string temp_curr = "Slot (" + current_slot.ToString() + ")";
            current_slot = int.Parse(button_name.Substring(6,1));

            if(temp_curr != button_name)
            {   
                // move upward
                TweenController.Controller().MoveToPosition(FindComponent<Button>(button_name).transform, button_pos[button_name]+new Vector3(0, 10, 0), 0.07f);
                if(button_pos.ContainsKey(temp_curr))    // move back to origin position
                    TweenController.Controller().MoveToPosition(FindComponent<Button>(temp_curr).transform, button_pos[temp_curr], 0.07f); 
            }              
        }
        else
        {
            AudioController.Controller().PlaySound("ButtonClick");

            switch(button_name)
            {
                case "InventoryPanel":
                    if(GUIController.Controller().GetPanel<InventoryPanel>("InventoryPanel") != null)
                        GUIController.Controller().RemovePanel("InventoryPanel");
                    else
                        GUIController.Controller().ShowPanel<InventoryPanel>("InventoryPanel", 2);
                    break;
                case "CharacterPanel":
                    ItemController.Controller().TempTest();
                    ItemController.Controller().CreateDropItem("Potionist@Seed", 1, new Vector3(1, 1, 0));
                    break;
                case "MapPanel":
                    break;
                // open or close setting panel
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
    }

    public void Refresh()
    {
        StoreItem[] invent = ItemController.Controller().GetInvent("QuickSlot");   

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
                slot.GetChild(0).gameObject.SetActive(true);
                if(invent[i].item_num > 1)
                    slot.GetChild(1).gameObject.SetActive(true);
                slot.GetChild(0).gameObject.GetComponent<Image>().sprite = ItemController.Controller().GetItemSprite(invent[i].item_id);
                slot.GetChild(1).gameObject.GetComponent<Text>().text = invent[i].item_num.ToString();
            }
        }
    }

    /// <summary>
    /// Refresh the money text gui
    /// </summary>
    public void RefreshMoneyText(int num)
    {
        FindComponent<Text>("MoneyText").text = num.ToString();
    }

    /// <summary>
    /// a interface for player controller to use and trigger
    /// </summary>
    /// <param name="button_name"></param>
    public void ShortCutClick(int num, bool scroll)
    {
        int temp_slot; 

        if(scroll)
            temp_slot = current_slot + num;
        else
            temp_slot = num;

        if(temp_slot < 0)
            temp_slot = 7;
        else if(temp_slot > 7)
            temp_slot = 0;

        string button_name = "Slot (" + temp_slot.ToString() + ")";
        OnButtonClick(button_name);
    }

    /// <summary>
    /// Pointer Enter Event
    /// </summary>
    private void OnPointerEnter(PointerEventData event_data)
    {
        string name = GetPointerObjectName(event_data);
        if(name != "")
            TweenController.Controller().MoveToPosition(FindComponent<Button>(name).transform, button_pos[name] + new Vector3(0, 5, 0), 0.07f);
    }

    /// <summary>
    /// Pointer Exit Event
    /// </summary>
    private void OnPointerExit(PointerEventData event_data)
    {
        string name = GetPointerObjectName(event_data);
        if(name != "")
            TweenController.Controller().MoveToPosition(FindComponent<Button>(name).transform, button_pos[name], 0.07f);
    }

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
            ItemController.Controller().TransferItem("QuickSlot", "Player", index);
    }

    /// <summary>
    /// save item data to a temp when mouse drag begin
    /// </summary>
    /// <param name="event_data">pointer data</param>
    private void OnBeginDrag(PointerEventData event_data)
    {
        Debug.Log("DragBegin");
    }

    /// <summary>
    /// load item data to this slot when mouse drag end
    /// </summary>
    /// <param name="event_data">pointer data</param>
    private void OnEndDrag(PointerEventData event_data)
    {
        Debug.Log("DragEnd");
    }

    private int GetPointerObjectIndex(PointerEventData event_data)
    {
        string name = event_data.pointerEnter.name;
        if(name == "Sprite" || name == "Num")
            name = event_data.pointerEnter.transform.parent.gameObject.name;
        if(name.IndexOf(" ") < 0)
            return -1;
        return int.Parse( name.Substring(name.IndexOf("(")+1, 1) );
    }

    /// <summary>
    /// Pointer data analyze and handle
    /// </summary>
    /// <returns>the name of triggered gui object</returns>
    private string GetPointerObjectName(PointerEventData event_data)
    {
        string name = event_data.pointerEnter.name;

        if(!button_pos.ContainsKey(name))
            button_pos.Add(name, FindComponent<Button>(name).transform.position);
        if(name.IndexOf(" ") > 0)
        {
            if(name[6] == current_slot.ToString()[0])
                return "";
        }
        
        return name;
    }
}
