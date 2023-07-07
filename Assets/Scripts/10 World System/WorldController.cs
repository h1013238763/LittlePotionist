using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : BaseController<WorldController>
{
    private float total_day_time;
    private int hour_time;
    private int minute_time;
    private int day;
    private int season;
    private int year;

    public void NextDay()
    {
        // hour_time = 6;
        // minute_time = 0;
        day ++;
        if(day > 28)
        {
            day = 1;
            season ++;
        }
        if(season > 4)
        {
            season = 1;
            year ++;
        }
        EventController.Controller().EventTrigger("World/NextDay");
    }

    public void SetDate(int year, TimeSeason season, int day)
    {
        this.year = year;
        this.season = (int)season;
        this.day = day;
    }

    public string GetDateText()
    {
        string time_text = "";

        time_text += "Year " + year.ToString();
        time_text += " . " + season;
        time_text += " . " + day;
        if(day%10 == 1)
            time_text += " st";
        else if(day%10 == 2)
            time_text += " nd";
        else if(day%10 == 3)
            time_text += " rd";
        else
            time_text += " th";

        return time_text;
    }

    public string GetTimeText()
    {
        return hour_time + " : " + minute_time;
    }
}

public enum TimeSeason
{
    Spring = 1,
    Summer = 2,
    Fall = 3,
    Winter = 4
}