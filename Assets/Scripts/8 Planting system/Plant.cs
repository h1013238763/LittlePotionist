using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour 
{
    public string plant_id;
    public string plant_name;

    public int plant_difficulty;
    public int plant_grow_time;
    private int plant_grow_curr;

    public int[] plant_grow_factor;
    public int plant_grow_quality;

    public string plant_type_grow;
    public string plant_type_environment;

    public string plant_harvest_time;
    public string plant_harvest_num;
    public int plant_harvest_item;

    public Plant()
    {

    }

    public void Grow(int[] factors)
    {
        
    }

    public void Harvest()
    {

    }

    public void Die()
    {
        
    }
}