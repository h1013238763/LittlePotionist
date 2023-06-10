using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public StoreItem[] item_list;
    public int invent_capa;

    public void Initial(int size)
    {
        invent_capa = size;
        item_list = new StoreItem[size];
    }
}
