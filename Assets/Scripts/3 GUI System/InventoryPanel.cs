using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : PanelBase
{
    public override void ShowSelf()
    {
        Refresh();
    }

    public void Refresh()
    {
        StoreItem[] player_invent = ItemController.GetController().GetInvent("Player");

        for(int i = 0; i < player_invent.Length; i ++)
        {
            Transform slot = FindComponent<Button>("Slot (" + i + ")").transform;

            if(player_invent[i] == null)
            {
                slot.GetChild(0).gameObject.SetActive(false);
                slot.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                slot.GetChild(0).gameObject.GetComponent<Image>().sprite = ItemController.GetController().GetItemInfo(player_invent[i].item_id).item_sprite;
                slot.GetChild(1).gameObject.GetComponent<Text>().text = player_invent[i].item_num.ToString();
            }
        }

        FindComponent<Text>("MoneyText").text = ItemController.GetController().wealth.ToString();
    }

    protected override void OnClick(string button_name)
    {
        if(button_name == "Close")
        {
            GUIController.GetController().HidePanel("InventoryPanel");
        }
            
            
        
    }
}
