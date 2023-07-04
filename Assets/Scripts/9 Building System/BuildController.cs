using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class BuildController : BaseController<BuildController>
{
    private List<MonoDeco> deco_list = new List<MonoDeco>();
    private GameObject deco_holder;
    private GameObject deco_preview;
    private int direct;

    /// <summary>
    /// Initial the building controller
    /// </summary>
    /// <param name="map_name"></param>
    public void Initial()
    {
        deco_holder = GameObject.Find("Decoration");
        if(deco_holder == null)
            deco_holder = new GameObject("Decoration");
    }

    public void PlanDeco(Decoration deco, Vector3Int pos)
    {

    }

    /// <summary>
    /// create a gameobject of target decoration and name it by its id
    /// </summary>
    /// <param name="deco"></param>
    public void PlaceDeco(Decoration deco, Vector3Int pos)
    {
        // create a target gameobject by its id
        GameObject temp = new GameObject();

        temp.name = deco.item_id;

        if( deco is DecoStorage)
        {
            temp.name += "|" +pos.x.ToString()+","+pos.y.ToString();
            ItemController.Controller().AddInvent(temp.name, (deco as DecoStorage).storage_size);
        }

        // Add Component to it
        // Sprite Renderer
        temp.AddComponent<SpriteRenderer>();
        temp.GetComponent<SpriteRenderer>().sprite = ResourceController.Controller().Load<Sprite>(deco.item_id + "_" + direct.ToString());

        // MonoDeco
        MonoDeco deco_info = temp.AddComponent<MonoDeco>();
        deco_info.Assign(deco.item_id, temp.name, direct, deco.Action);
        deco_list.Add(deco_info);

        // Rigidbody
        temp.AddComponent<Rigidbody2D>();
        temp.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        // collider
        temp.AddComponent<BoxCollider2D>();

        // place it into scene and add it into deco_info
        temp.transform.position = pos;
    }

    public void RemoveDeco(GameObject deco)
    {

    }

    /// <summary>
    /// use object name as id to locate the decoration interaction functions and trigger
    /// </summary>
    /// <param name="deco"></param>
    public void UseDeco(GameObject deco)
    {

    }
}