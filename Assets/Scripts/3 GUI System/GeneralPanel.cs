using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralPanel : PanelBase
{
    private string current_slot = "";        // the index of current using quick slot
    private Dictionary<string, Vector3> button_pos = new Dictionary<string, Vector3>();

    protected override void Awake()
    {
        base.Awake();
        Initial();
    }

    void Start()
    {
        
    }

    /// <summary>
    /// Initial the General Panel
    /// </summary>
    public void Initial()
    {
        ItemController.GetController().AddInvent("QuickSlot", 8);
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


    public void PointerEnter(Transform transform)
    {
        string name = PointerEventCheck(transform);
        
        if(name != "")
            TweenController.GetController().MoveToPosition(transform, button_pos[name]+new Vector3(0, 5, 0), 0.07f);
    }

    public void PointerExit(Transform transform)
    {
        string name = PointerEventCheck(transform);
        
        if(name != "")
            TweenController.GetController().MoveToPosition(transform, button_pos[name], 0.07f);
    }

    private string PointerEventCheck(Transform transform)
    {
        string name = transform.gameObject.name;

        if(!button_pos.ContainsKey(name))
            button_pos.Add(name, FindComponent<Button>(name).transform.position);
        if(name == current_slot)
            return "";

        return name;
    }
}
