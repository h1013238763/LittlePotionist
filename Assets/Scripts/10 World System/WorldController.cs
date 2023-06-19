using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : BaseControllerMono<WorldController>
{
    private float time_radio;
    private int hour_time;
    private int minute_time;
    private int day;
    private int days_in_season;
    private int season;
    private int year;

    public void NextDay()
    {
        hour_time = 6;
        minute_time = 0;
        day ++;
        if(day > days_in_season)
        {
            day = 1;
            season ++;
        }
        if(season > 4)
        {
            season = 1;
            year ++;
        }
        EventController.GetController().EventTrigger("World/NextDay");
    }
}
