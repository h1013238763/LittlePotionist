using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Decoration class
/// the decorations that be placed in house
/// </summary>
public class MonoDeco : MonoBehaviour
{
    public string item_id;
    public string deco_name;
    public Vector3 deco_pos;
    public int deco_direct;
    public UnityAction<string> action;

    public MonoDeco(){}

    public void Assign(string id, string name, int dir, UnityAction<string> action)
    {
        item_id = id;
        deco_name = name;
        deco_direct = dir;
        this.action = action;
    }
}