using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : PanelBase
{
    public SettingConfig config;
    private Resolution[] resolutions;
    public bool start_scene = true;

    public override void ShowSelf()
    {
        config = XmlController.Controller().LoadData(typeof(SettingConfig), "Config") as SettingConfig;
        // resolution setting
        resolutions = Screen.resolutions;
        List<string> options = new List<string>();
        int config_resolution = 0;
        for(int i = 0; i < resolutions.Length; i ++)
        {
            options.Add( resolutions[i].width + " x " + resolutions[i].height );
            if(resolutions[i].width == config.resolution.x && resolutions[i].height == config.resolution.y)
                config_resolution = i;
        }
        // initial resolution dropdown
        Dropdown resolution_drop = FindComponent<Dropdown>("ResolutionDropdown");
        resolution_drop.ClearOptions();
        resolution_drop.AddOptions(options);
        resolution_drop.value = config_resolution;
        resolution_drop.RefreshShownValue();

        // finish loading initial
        FindComponent<Button>("ExitBtn").gameObject.SetActive(!start_scene);
    }

    public override void HideSelf()
    {
        XmlController.Controller().SaveData(config, "Config");

        if(start_scene)
            gameObject.SetActive(false);
        else
            GUIController.Controller().RemovePanel("SettingPanel");
    }

    protected override void OnButtonClick(string button_name)
    {
        ButtonClickSound();

        switch (button_name)
        {
            case "BackBtn": // close setting panel
                HideSelf();
                if(start_scene)
                    GUIController.Controller().GetPanel<StartPanel>("StartPanel").ShowSelf();
                break;
            case "ExitBtn": // back to start scene
                EventController.Controller().AddEventListener("ConfirmPanelEvent", () =>
                {
                    HideSelf();
                    OverallController.Controller().ChangeScene("StartScene");
                });
                GUIController.Controller().ShowPanel<ConfirmPanel>("ConfirmPanel", 3, (p) =>
                {
                    p.SetPanel("Unsaved game content will be lost,\nsure to return to the start menu?");
                });
                break;
            default:
                break;
        }
    }

    protected override void OnSliderValueChanged(string slider_name, float slider_value)
    {
        AudioController.Controller().PlaySound("SliderDrag");

        // set volume text
        FindComponent<Text>(slider_name+"Value").text = (slider_value*5).ToString();
        
        if(slider_name == "MusicVolume") // change music volume
        {
            AudioController.Controller().ChangeMusicVolume(slider_value/20);
            config.music_volume = slider_value;
        }
            
        else if(slider_name == "SoundVolume")   // change sound volume
        {
            AudioController.Controller().ChangeSoundVolume(slider_value/20);
            config.sound_volume = slider_value;
        }
    }

    protected override void OnToggleValueChanged(string toggle_name, bool is_check)
    {
        ButtonClickSound();
        // set full screen
        if(toggle_name == "FullscreenToggle")
        {
            Screen.fullScreen = is_check;
            config.full_screen = is_check;
        }
    }

    protected override void OnDropdownValueChanged(string drop_name, int drop_value)
    {
        // change resolution
        if(drop_name == "ResolutionDropdown")
        {
            config.resolution = new Vector2Int(resolutions[drop_value].width, resolutions[drop_value].height);
            Screen.SetResolution(resolutions[drop_value].width, resolutions[drop_value].height, config.full_screen);
        }
    }

    public void ButtonClickSound()
    {
        AudioController.Controller().PlaySound("ButtonClick");
    }
}