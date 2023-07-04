using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : PanelBase
{    
    protected override void Awake()
    {
        base.Awake();
    }

    public override void ShowSelf()
    {
        FindComponent<Text>("VersionText").text = OverallController.Controller().version;
        FindComponent<Image>("Container").gameObject.SetActive(true);
    }

    /// <summary>
    /// trigger the button click event of start panel
    /// </summary> 
    /// <param name="button_name">the name of button element</param>
    protected override void OnButtonClick(string button_name)
    {
        AudioController.Controller().PlaySound("ButtonClick");

        FindComponent<Image>("Container").gameObject.SetActive(false);
        switch(button_name)
        {
            case "Start":   // move to save slots menu
                GUIController.Controller().GetPanel<SavePanel>("SavePanel").gameObject.SetActive(true);
                break;
            case "Setting": // move to setting menu
                GUIController.Controller().GetPanel<SettingPanel>("SettingPanel").start_scene = true;
                GUIController.Controller().GetPanel<SettingPanel>("SettingPanel").gameObject.SetActive(true);
                break;
            case "Quit":    // quit game
                Application.Quit();
                break;
        }
    }
}
