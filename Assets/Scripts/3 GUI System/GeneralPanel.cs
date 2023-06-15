using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GeneralPanel : PanelBase
{
    private string current_slot = "";        // the index of current using quick slot
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
    }

    public void Use()
    {
        ItemController.GetController().UseItem(int.Parse(current_slot.Substring(11,1)));
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

        string temp_slot = current_slot;
        // if click a quick slot
        if(button_name.IndexOf(" ") > 0)
        {   
            current_slot = button_name;

            if(temp_slot != current_slot || temp_slot == "")    // move upward
                TweenController.GetController().MoveToPosition(FindComponent<Button>(button_name).transform, button_pos[button_name]+new Vector3(0, 10, 0), 0.07f);
            if(temp_slot != "" && temp_slot != current_slot)    // move back to origin position
                TweenController.GetController().MoveToPosition(FindComponent<Button>(temp_slot).transform, button_pos[temp_slot], 0.07f);   
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
                default:
                    break;
            }     
        }    
    }

    /// <summary>
    /// a interface for player controller to use and trigger
    /// </summary>
    /// <param name="button_name"></param>
    public void ShortCutClick(string button_name)
    {
        OnClick(button_name);
    }

    /// <summary>
    /// Pointer Enter Event
    /// </summary>
    private void OnPointerEnter(PointerEventData event_data)
    {
        string name = PointerEventHandle(event_data);
        TweenController.GetController().MoveToPosition(FindComponent<Button>(name).transform, button_pos[name] + new Vector3(0, 5, 0), 0.07f);
    }

    /// <summary>
    /// Pointer Exit Event
    /// </summary>
    private void OnPointerExit(PointerEventData event_data)
    {
        string name = PointerEventHandle(event_data);
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
        if(name == current_slot)
            return "";

        return name;
    }
}
