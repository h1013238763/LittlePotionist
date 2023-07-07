using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : BaseControllerMono<Tester>
{
    // Start is called before the first frame update
    void Start()
    {
        // GUIController.Controller().ShowPanel<BottomPanel>("BottomPanel", 2);
        // test1();
    }

    private void test1()
    {
        Item furniture = new Item();
        furniture.item_id = "Potionist:Old_Chair";
        furniture.item_name = "Old Chair";
        furniture.item_describe = "An old chair, but still solid";
        furniture.item_stack = 1;
        furniture.item_price = 50;
        furniture.item_usable = true;
        furniture.item_type = ItemType.Furniture;
        furniture.item_info = "";
        furniture.item_info += "1,1 ";
        furniture.item_info += "true ";
        furniture.item_info += "true ";
        furniture.item_info += "Chair ";
        furniture.item_info += "Ground ";

        List<Item> item_list = new List<Item>();

        item_list.Add(furniture);
        XmlController.Controller().SaveData(item_list, "PotionistItems", "Data/Items/");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
