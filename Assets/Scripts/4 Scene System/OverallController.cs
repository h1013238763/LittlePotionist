using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.IO;

public class OverallController : BaseControllerMono<OverallController>
{
    public SaveData current_save;
    public SettingConfig config;
    public string version = "Version: 0.0.90 Alpha";

    void Start()
    {
        GameInitial();
    }

    /// <summary>
    /// Initial the game setting and save by config file
    /// </summary>
    public void GameInitial()
    {
        // read config file and initial setting
        config = XmlController.Controller().LoadData(typeof(SettingConfig), "Config") as SettingConfig;
        if(config == null)
            config = new SettingConfig();
        // Audio initial
        AudioController.Controller().ChangeMusicVolume(config.music_volume/20);
        AudioController.Controller().ChangeSoundVolume(config.sound_volume/20);
        // Video initial
        Screen.fullScreen = config.full_screen;
        Screen.SetResolution(config.resolution.x, config.resolution.y, config.full_screen);

        // Start initial game datas
        EventController.Controller().AddEventListener("LoadingAnimeFinish", new SceneEvents().EnterStartScene);
        GUIController.Controller().ShowPanel<LoadingPanel>("LoadingPanel", 3);
        
        // initial item information
        ItemInitial();
        // initial player infomation

        // initial scene objects

        // initial item stored in scene objects
    }

    public void GameLoad()
    {
        
    }

    /// <summary>
    /// Change scene actions of Little Potionist
    /// </summary>
    /// <param name="name">name of scene</param>
    /// <param name="action">The action after finish loading</param>
    public void ChangeScene(string scene)
    {
        // regist event action with empty delegate
        UnityAction exit_action = (() => {});
        UnityAction enter_action = (() => {});

        // exit current scene action
        switch(SceneManager.GetActiveScene().name)  
        {
            case "StartScene":
                exit_action = new SceneEvents().ExitStartScene;
                break;
            case "HomeScene":
                exit_action = new SceneEvents().ExitHomeScene;
                break;
            default:
                break;
        }
        // enter target scene action
        switch(scene)
        {
            case "StartScene":
                enter_action = new SceneEvents().EnterStartScene;
                break;
            case "HomeScene":
                enter_action = new SceneEvents().EnterHomeScene;
                break;
            default:
                break;
        }

        // add registed action to event listener
        EventController.Controller().AddEventListener("LoadingAnimeFinish", exit_action);
        EventController.Controller().AddEventListener("LoadingAnimeFinish", () =>
        {
            SceneController.Controller().LoadSceneAsync(scene, enter_action);
        });

        // start changing
        GUIController.Controller().ShowPanel<LoadingPanel>("LoadingPanel", 3);
    }

    private void ItemInitial()
    {
        // read item file
        XmlDictionary<string, Item> items = new XmlDictionary<string, Item>();
        List<Item> item_list = XmlController.Controller().LoadData(typeof(List<Item>), "PotionistItems", "Data/Items/") as List<Item>;
        List<string> token;

        // handle each item in file
        foreach(Item item in item_list)
        {
            token = GetNextToken(item);
            switch(item.item_type)
            {
                case ItemType.Furniture:
                    Furniture temp_furniture = new Furniture(item);
                    // furniture.furn_size
                    temp_furniture.furn_size = new Vector2Int(
                        int.Parse(token[0].Substring(0, token[0].IndexOf(","))),
                        int.Parse(token[0].Substring(token[0].IndexOf(",")+1))
                    );
                    // furniture.furn_interact
                    temp_furniture.furn_interact = (token[1] == "true");
                    // furniture.furn_collision
                    temp_furniture.furn_collision = (token[2] == "true");
                    // furniture. BuildController.FurnitureType furn_type;
                    temp_furniture.furn_type = (BuildController.FurnitureType)int.Parse(token[3]);
                    // furniture BuildController.BuildType furn_build_type;
                    temp_furniture.furn_build_type = (BuildController.BuildType)int.Parse(token[4]);

                    // save into items
                    items.Add(temp_furniture.item_id, temp_furniture);
                    break;
                default:
                    items.Add(item.item_id, item);
                    break;
            }
        }
        ItemController.Controller().InitialItemDict(items);
    }

    /// <summary>
    /// break the string line into tokens
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private List<string> GetNextToken(Item item)
    {
        List<string> tokens = new List<string>();

        // split line into token till the last token
        while(item.item_info.IndexOf(" ") < item.item_info.Length-1 && item.item_info.IndexOf(" ") > 0)
        {
            tokens.Add(item.item_info.Substring(0, item.item_info.IndexOf(" ")));
            item.item_info = item.item_info.Substring(item.item_info.IndexOf(" ")+1);
        }
        // handle last token
        if(item.item_info != null)
        {
            tokens.Add(item.item_info.Substring(0));
            item.item_info = null;
        }
        return tokens;
    }
}

/// <summary>
/// class of setting data
/// </summary>
public class SettingConfig
{
    // music setting
    public float music_volume;
    public float sound_volume;

    // video setting
    public Vector2Int resolution;
    public bool full_screen;

    // constructor
    public SettingConfig()
    {
        music_volume = 50;
        sound_volume = 50;

        resolution = new Vector2Int( Screen.currentResolution.width, Screen.currentResolution.height );
        full_screen = false;
    }
}

/// <summary>
/// class of save data
/// </summary>
public class SaveData
{
    // Savadata variable
    public int save_index;          // the index of save file
    public string save_name;        // the name of saves
    // the time of gameworld of save
    public int save_year;
    public TimeSeason save_season;
    public int save_day;      
    
    // Player variables
    public string player_scene;     // the current scene of player
    public Vector2 player_position;  // the current position of player

    // Item variables
    public XmlDictionary<string, StoreItem[]> invent_dict;  // all item data of inventorys

    public SaveData(){}

    public SaveData(string name)
    {
        save_name = name;
        save_year = 0;
        save_season = TimeSeason.Spring;
        save_day = 1;
    
        // Player variables
        player_scene = "HomeScene";
        player_position = new Vector2(0, 0);

        // Item variables
        invent_dict = new XmlDictionary<string, StoreItem[]>();
    }
}

/// <summary>
/// Scene loading actions
/// </summary>
public struct SceneEvents
{
    /// <summary>
    /// Start Scene Related Actions
    /// </summary>
    public void EnterStartScene()
    {
        // start menu inital
        GUIController.Controller().ShowPanel<StartPanel>("StartPanel", 2);
        GUIController.Controller().ShowPanel<SavePanel>("SavePanel", 2, (p) =>
        {
            p.gameObject.SetActive(false);
        });
        GUIController.Controller().ShowPanel<SettingPanel>("SettingPanel", 3, (p) =>
        {
            p.gameObject.SetActive(false);
        });

        AudioController.Controller().PlayMusic("TitleBGM");

        GUIController.Controller().HidePanel("LoadingPanel");
    }
    public void ExitStartScene()
    {
        // Stop Start BGM
        AudioController.Controller().StopMusic();

        // Set GUI
        GUIController.Controller().RemovePanel("StartPanel");
        GUIController.Controller().RemovePanel("SavePanel");
        GUIController.Controller().RemovePanel("SettingPanel");
    }

    /// <summary>
    /// Home Scene Related Actions
    /// </summary>
    public void EnterHomeScene()
    {
        // Change scene
        GUIController.Controller().ShowPanel<BottomPanel>("BottomPanel", 1);

        // Start Music
        AudioController.Controller().PlayMusic("MorningBGM");

        GUIController.Controller().HidePanel("LoadingPanel");
    }
    public void ExitHomeScene()
    {
        AudioController.Controller().StopMusic();

        GUIController.Controller().RemovePanel("GeneralPanel");
    }
}

