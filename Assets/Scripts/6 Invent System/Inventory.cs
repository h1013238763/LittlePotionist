using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int invent_id;
    public StoreItem[] item_list;
    public int invent_capa;

    public void Initial(int size)
    {
        invent_capa = size;
        item_list = new StoreItem[size];
    }
}
