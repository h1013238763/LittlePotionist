using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Decoration class
/// the decorations that be placed in house
/// </summary>
public class Decoration : Item
{
    public Vector2Int deco_size;
    public bool deco_usable;
    public bool deco_collision;
    public virtual void Action(string name){}
}