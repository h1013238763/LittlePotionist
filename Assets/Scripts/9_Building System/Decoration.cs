using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Decoration class
/// the decorations that be placed in house
/// </summary>
public class Decoration
{
    public int deco_id;
    public string deco_name;
    public string deco_describe;
    public DecoType deco_type;
    public int deco_price;
    public string deco_call;        // the name of function to call while interacting
}

public enum DecoType
{
    Special,
    Wallpaper,
    Floor,
    Furniture,
    Device,
    Farmland,
    Carpet,
    Wall
}