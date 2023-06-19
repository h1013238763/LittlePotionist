using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : PanelBase
{
    private Dictionary<string, Vector3> panel_position = new Dictionary<string, Vector3>();
    private int save_slot_assign = 0;
    
    protected override void Awake()
    {
        base.Awake();
        ShowSelf();
    }

    public override void ShowSelf()
    {
        panel_position.Add("BtnGrid", FindComponent<Image>("BtnGrid").transform.position);
        panel_position.Add("SaveGrid", FindComponent<Image>("SaveGrid").transform.position);
    }

    public void SaveSlotInitial()
    {   
        Debug.Log("Inital");
        SaveData[] save_info = OverallController.GetController().GetSaveInfo();
        
        foreach (SaveData data in save_info)
            Debug.Log(data);

        for(int i = 0; i < 3; i ++)
        {
            Transform slot = FindComponent<Image>("SaveSlot ("+i.ToString()+")").transform;
            if(save_info[i] == null)
            {
                slot.GetChild(0).gameObject.SetActive(false);
                slot.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                slot.GetChild(0).gameObject.SetActive(true);
                slot.GetChild(1).gameObject.SetActive(false);
            }     
        }
    }

    /// <summary>
    /// trigger the button click event of start panel
    /// </summary> 
    /// <param name="button_name">the name of button element</param>
    protected override void OnClick(string button_name)
    {
        if(char.IsDigit( button_name.ToCharArray()[button_name.Length-1] ))
        {
            save_slot_assign = int.Parse(button_name.Substring(button_name.Length-1));

            switch(button_name.Substring(0, button_name.IndexOf(" ")))
            {
                case "NewTip":          // create character
                    FindComponent<Image>("CharacterCreate").transform.position = new Vector3(Screen.width/2f, Screen.height/2f, 0);
                    break;
                case "DeleteButton":    // delete character
                    OverallController.GetController().DeleteSave(save_slot_assign);
                    SaveSlotInitial();
                    break;
                 case "LoadButton":     // load character
                    Debug.Log("Load" + save_slot_assign);
                    break;
            }
        }

        switch(button_name)
        {
            case "Start":
                SaveSlotInitial();
                TweenController.GetController().MoveToPosition(FindComponent<Image>("BtnGrid").transform, new Vector3(Screen.width+400, Screen.height/2f, 0), 0.15f);
                TweenController.GetController().MoveToPosition(FindComponent<Image>("SaveGrid").transform, new Vector3(Screen.width/2f, Screen.height/2f, 0), 0.15f);
                break;

            case "SaveBack":
                TweenController.GetController().MoveToPosition(FindComponent<Image>("BtnGrid").transform, new Vector3(Screen.width-102, Screen.height/2f, 0), 0.15f);
                TweenController.GetController().MoveToPosition(FindComponent<Image>("SaveGrid").transform, panel_position["SaveGrid"], 0.15f);
                break;

            case "CreateBack":
                FindComponent<InputField>("NameField").text = "Alex";
                FindComponent<Image>("CharacterCreate").transform.position = new Vector3(Screen.width+1000, Screen.height/2f, 0);
                break;
            case "CreateAccept":
                GUIController.GetController().ShowPanel<LoadingPanel>("LoadingPanel", 3);
                OverallController.GetController().CreateSave(save_slot_assign, FindComponent<InputField>("NameField").text);
                break;
            default:
                break;
        }
    }
}
