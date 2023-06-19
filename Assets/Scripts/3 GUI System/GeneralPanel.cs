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

    void Start()
    {
        
    }

    public override void ShowSelf()
    {
        ItemController.GetController().AddInvent("QuickSlot", 8);

        Button[] ctrls = this.GetComponentsInChildren<Button>();

        for(int i = 0; i < ctrls.Length; i++)
        {
            GUIController.AddCustomEventListener(ctrls[i], EventTriggerType.PointerEnter, (data) => {OnPointerEnter((PointerEventData)data); });
            GUIController.AddCustomEventListener(ctrls[i], EventTriggerType.PointerExit,  (data) => {OnPointerExit ((PointerEventData)data); });
        }

        if(!GUIController.GetController().panel_dic.ContainsKey("GeneralPanel"))
            GUIController.GetController().panel_dic.Add("GeneralPanel", gameObject.GetComponent<GeneralPanel>());
    }

    public void Use()
    {
        ItemController.GetController().UseItem(current_slot);
    }

    /// <summary>
    /// Button Click Event
    /// </summary>
    /// <param name="button_name"></param>
    protected override void OnClick(string button_name)
    {
        // recond button origin position
        if(!button_pos.ContainsKey(button_name))
            button_pos.Add(button_name, FindComponent<Button>(button_name).transform.position);

        
        // if click a quick slot
        if(button_name.IndexOf(" ") > 0)
        {   
            string temp_curr = "QuickSlot (" + current_slot.ToString() + ")";
            current_slot = int.Parse(button_name.Substring(11,1));

            if(temp_curr != button_name)
            {   
                // move upward
                TweenController.GetController().MoveToPosition(FindComponent<Button>(button_name).transform, button_pos[button_name]+new Vector3(0, 10, 0), 0.07f);
                if(button_pos.ContainsKey(temp_curr))    // move back to origin position
                    TweenController.GetController().MoveToPosition(FindComponent<Button>(temp_curr).transform, button_pos[temp_curr], 0.07f); 
            }              
        }
        else
        {
            switch(button_name)
            {
                case "InventoryPanel":
                    if(GUIController.GetController().GetPanel<InventoryPanel>("InventoryPanel") != null)
                        GUIController.GetController().HidePanel("InventoryPanel");
                    else
                        GUIController.GetController().ShowPanel<InventoryPanel>("InventoryPanel", 2);
                    break;
                case "CharacterPanel":
                    ItemController.GetController().Initial();
                    break;
                case "MapPanel":
                    break;
                default:
                    break;
            }     
        }  
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

        string button_name = "QuickSlot (" + temp_slot.ToString() + ")";
        OnClick(button_name);
    }

    /// <summary>
    /// Pointer Enter Event
    /// </summary>
    private void OnPointerEnter(PointerEventData event_data)
    {
        string name = PointerEventHandle(event_data);
        if(name != "")
            TweenController.GetController().MoveToPosition(FindComponent<Button>(name).transform, button_pos[name] + new Vector3(0, 5, 0), 0.07f);
    }

    /// <summary>
    /// Pointer Exit Event
    /// </summary>
    private void OnPointerExit(PointerEventData event_data)
    {
        string name = PointerEventHandle(event_data);
        if(name != "")
            TweenController.GetController().MoveToPosition(FindComponent<Button>(name).transform, button_pos[name], 0.07f);
    }

    /// <summary>
    /// Pointer data analyze and handle
    /// </summary>
    /// <returns>the name of triggered gui object</returns>
    private string PointerEventHandle(PointerEventData event_data)
    {
        string name = event_data.pointerEnter.name;

        if(!button_pos.ContainsKey(name))
            button_pos.Add(name, FindComponent<Button>(name).transform.position);
        if(name.IndexOf(" ") > 0)
        {
            if(name[11] == current_slot.ToString()[0])
                return "";
        }
        

        return name;
    }
}
