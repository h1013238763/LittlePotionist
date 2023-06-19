using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverallController : BaseControllerMono<OverallController>
{
    private SettingData setting;
    private SaveData[] saves = new SaveData[3];

    void Start()
    {
        GameInitial();
    }

    /// <summary>
    /// Initial the game setting and save by config file
    /// </summary>
    public void GameInitial()
    {
        // config initial
        setting = XmlController.GetController().LoadData(typeof(SettingData), "Config") as SettingData;
        if(setting == null)
            DefaultInitial();
        
        // savefile initial
        for(int i = 0; i < 3; i ++)
        {
            saves[i] = XmlController.GetController().LoadData(typeof(SaveData), "savedata_"+i.ToString(), "Save/") as SaveData;
        }

        
    }

    /// <summary>
    /// Set all settings as default and create a config file
    /// </summary>
    public void DefaultInitial()
    {   
        setting = new SettingData();

        // volume setting
        setting.master_volume = 1f;
        setting.bgm_volume = 1f;
        setting.sound_volume = 1f;
        // video setting
        setting.resolution = "1920x1080";
        setting.screen_mode = "fullscreen";
        // key setting
        setting.keymap = "default";
        // language setting
        setting.language = "en_us";

        // create setting file
        SaveSetting();
    }

    public void GameLoad()
    {
        BuildController.GetController().Initial();
    }

    public void GameSave()
    {

    }
    
    public void GameEnd()
    {

    }

    public void CreateSave(int index, string name)
    {
        SaveData save_file = new SaveData(name);
        saves[index] = save_file;
        XmlController.GetController().SaveData(save_file, "savedata_" + index.ToString(), "Save/");
    }
    
    /// <summary>
    /// delete the save at the given index
    /// </summary>
    /// <param name="index"></param>
    public void DeleteSave(int index)
    {
        saves[index] = null;
        XmlController.GetController().DeleteData("savedata_" + index.ToString(), "Save/");
    }

    /// <summary>
    /// Save setting to file
    /// </summary>
    public void SaveSetting()
    {
        XmlController.GetController().SaveData(setting, "Config");
    }

    public void ModifySetting()
    {

    }

    

    /// <summary>
    /// return the save info for other class to use
    /// </summary>
    /// <returns> the array of saves</returns>
    public SaveData[] GetSaveInfo()
    {
        return saves;
    }

    /// <summary>
    /// class of setting data
    /// </summary>
    public class SettingData
    {
        // game setting data
        // volume setting
        public float master_volume;
        public float bgm_volume;
        public float sound_volume;

        // video setting
        public string resolution;
        public string screen_mode;

        // key setting
        public string keymap;

        // language setting
        public string language;
    }
}

/// <summary>
/// class of save data
/// </summary>
public class SaveData
{
    // Savadata variable
    public string save_name;        // the name of saves
    public string save_time;        // the time of gameworld of save
    
    // Player variables
    public string player_scene;     // the current scene of player
    public string player_position;  // the current position of player

    // Item variables
    public XmlDictionary<string, StoreItem[]> invent_dict;  // all item data of inventorys

    public SaveData(){}

    public SaveData(string name)
    {
        save_name = name;
        save_time = "Year 0. Spring 1th";
    
        // Player variables
        player_scene = "ShopScene";
        player_position = "";

        // Item variables
        invent_dict = new XmlDictionary<string, StoreItem[]>();
    }
}