using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SavePanel : PanelBase
{
    public SaveData[] saves = new SaveData[3];
    private int acting_save;

    public override void ShowSelf()
    {
        for(int i = 0; i < 3; i ++)
        {
            // add event trigger
            GUIController.AddCustomEventListener(FindComponent<Image>("SaveSlot ("+i.ToString()+")"),
                EventTriggerType.PointerEnter, (data) => 
                {
                    PointerEnterSlot((PointerEventData)data); 
                });
        }
        FindComponent<Image>("CharacterCreate").gameObject.SetActive(false);

        RefreshSaveSlot();
    }

    public override void HideSelf()
    {
        gameObject.SetActive(false);
    }

    protected override void OnButtonClick(string button_name)
    {
        AudioController.Controller().PlaySound("ButtonClick");

        if(button_name == "BackButton")
        {
            HideSelf();
            GUIController.Controller().GetPanel<StartPanel>("StartPanel").ShowSelf();
        }
        // delete save file
        else if(button_name == "DeleteButton")
        {
            GUIController.Controller().ShowPanel<ConfirmPanel>("ConfirmPanel", 3, (panel) => 
            {
                EventController.Controller().AddEventListener("ConfirmPanelEvent", () =>
                {
                    saves[acting_save] = null;
                    XmlController.Controller().DeleteData("savedata_" + acting_save.ToString(), "Save/");
                    RefreshSaveSlot();
                });
                panel.SetPanel("Are you sure you want to delete "+saves[acting_save].save_name+" ?");
            });
        }
        // load save file and start game
        else if(button_name == "LoadButton")
        {
            OverallController.Controller().current_save = saves[acting_save];
            OverallController.Controller().ChangeScene(saves[acting_save].player_scene);
        }
        // create a new character
        else if(button_name == "NewButton")
        {
            FindComponent<Image>("CharacterCreate").gameObject.SetActive(true);
        }
        // cancel creating
        else if(button_name == "CreateBack")
        {
            FindComponent<InputField>("NameField").text = "Alex";
            FindComponent<Image>("CharacterCreate").gameObject.SetActive(false);
        }
        // finish creating and start game
        else if(button_name == "CreateAccept")
        {
            // Create a new save and start game
            SaveData save_file = new SaveData(FindComponent<InputField>("NameField").text);
            saves[acting_save] = save_file;
            XmlController.Controller().SaveData(save_file, "savedata_" + acting_save.ToString(), "Save/");

            // change scene
            OverallController.Controller().current_save = saves[acting_save];
            OverallController.Controller().ChangeScene("HomePanel");
        }
    }

    /// <summary>
    /// Refresh Save Slot GUI
    /// </summary>
    private void RefreshSaveSlot()
    {
        for(int i = 0; i < 3; i ++)
        {
            Transform current_slot = FindComponent<Image>("SaveSlot ("+i.ToString()+")").transform;
            // read save date from file
            saves[i] = XmlController.Controller().LoadData(typeof(SaveData), "savedata_"+i.ToString(), "Save/") as SaveData;
            // target save slot is empty
            if(saves[i] == null)    
            {
                current_slot.GetChild(0).gameObject.SetActive(false);
                current_slot.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                Transform save_info = current_slot.transform.GetChild(0);
                // set save name
                save_info.GetChild(0).GetComponent<Text>().text = saves[i].save_name;

                // set save time
                save_info.GetChild(1).GetComponent<Text>().text = 
                    WorldController.Controller().TimeText(saves[i].save_year, saves[i].save_season, saves[i].save_day);

                // set visibility
                current_slot.GetChild(0).gameObject.SetActive(true);
                current_slot.GetChild(1).gameObject.SetActive(false);
            }
        }
        
    }

    private void PointerEnterSlot(PointerEventData event_data)
    {
        acting_save = int.Parse(event_data.pointerEnter.name.Substring(10, 1));
    }
}
