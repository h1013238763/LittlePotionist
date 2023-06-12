using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildController : BaseController<BuildController>
{
    private Dictionary<string, Decoration> deco_list = new Dictionary<string, Decoration>();
    private Dictionary<string, Tile> tile_list = new Dictionary<string, Tile>();
    private List<Tilemap> map_list = new List<Tilemap>();

    public string build_mode = "";

    /// <summary>
    /// Initial the building controller
    /// </summary>
    /// <param name="map_name"></param>
    public void Initial(string map_name = "TileGrid")
    {
        // Assign dictionarys
        Transform tile_grid = GameObject.Find(map_name).transform;
        if(tile_grid == null)
            tile_grid = ResourceController.GetController().Load<GameObject>("Decoration/TileMap/"+map_name).transform;

        tile_list.Add("Floor/Default", ResourceController.GetController().Load<Tile>("Decoration/Floor/Default"));
        tile_list.Add("Celling", ResourceController.GetController().Load<Tile>("Decoration/Celling/Celling"));
         
        for(int i = 0; i < tile_grid.childCount; i ++)
        {
            map_list.Add(tile_grid.GetChild(i).gameObject.GetComponent<Tilemap>());
        }

        // Assign to Event System
        EventController.GetController().AddEventListener("EnterBuildingMode", () =>
        {
            PlayerController.GetController().SetActionMap(PlayerActionMode.Building);
        });
    }

    /// <summary>
    /// Set current build action mode and assign action function
    /// </summary>
    /// <param name="type">the name of action</param>
    public void SetAction(BuildActionMode action)
    {
        // remove previous action event
        if(build_mode != "")
            EventController.GetController().RemoveEventKey(build_mode);

        // assign current action name
        build_mode = "BuildingController."+action.ToString();
        switch (action)
        {
            case BuildActionMode.PlaceSpace:
                EventController.GetController().AddEventListener<Vector2>(build_mode, PlaceSpace);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Add a new space into tilemap which allow player move and place other decoration
    /// </summary>
    /// <param name="pos">mouse position</param>
    private void PlaceSpace(Vector2 pos)
    {
        // coordinate transformation
        Vector2 world_pos = Camera.main.ScreenToWorldPoint(pos);
        Vector3Int tile_pos;
        tile_pos = map_list[0].WorldToCell(Camera.main.ScreenToWorldPoint(pos));

        // availability check
        if(map_list[0].GetTile(tile_pos) == null)
            return;
        
        // place space processing
        map_list[0].SetTile(tile_pos, null);

    }

    private void MoveSpace()
    {

    }
}

public enum BuildActionMode
{
    PlaceSpace,
    RemoveSpace
}
