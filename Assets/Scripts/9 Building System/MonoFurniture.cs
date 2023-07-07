using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoFurniture : MonoBehaviour
{
    public Furniture furniture;
    public Vector2Int furn_pos;
    public int furn_direction;

    public MonoFurniture(Furniture furniture, Vector2Int position, int direction)
    {
        this.furniture = furniture;
        furn_pos = position;
        furn_direction = direction;
    }
}
