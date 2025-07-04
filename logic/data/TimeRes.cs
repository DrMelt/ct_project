using Godot;
using Godot.Collections;
using System;
using System.Text.Json;


[GlobalClass]
public partial class TimeRes : Resource
{
    public TimeRes(TimeResJson timeResJson)
    {
        Dictionary timeDict = new Dictionary()
        {
            ["year"] = timeResJson.year,
            ["month"] = timeResJson.month,
            ["day"] = timeResJson.day,
            ["hour"] = timeResJson.hour,
            ["minute"] = timeResJson.minute,
            ["second"] = timeResJson.second
        };


        long time = Time.GetUnixTimeFromDatetimeDict(timeDict);
        _timeZone = Time.GetTimeZoneFromSystem()["bias"].AsInt64() * 60;
        _unixTime = time - _timeZone;
        UpdateTimeShow();
    }
    public TimeResJson ToJson()
    {
        return new TimeResJson()
        {
            year = year,
            month = month,
            day = day,
            hour = hour,
            minute = minute,
            second = second,
        };
    }
    public string ToJsonString()
    {
        return JsonSerializer.Serialize(ToJson());
    }

    public TimeRes()
    {
        _unixTime = Time.GetUnixTimeFromSystem();
        _timeZone = Time.GetTimeZoneFromSystem()["bias"].AsInt64() * 60;
        UpdateTimeShow();
    }


    public TimeRes(double unixTime)
    {
        _unixTime = unixTime;
        _timeZone = Time.GetTimeZoneFromSystem()["bias"].AsInt64() * 60;
        UpdateTimeShow();
    }

    public TimeRes(Dictionary unixTimeDict)
    {
        _unixTime = Time.GetUnixTimeFromDatetimeDict(unixTimeDict);
        _timeZone = Time.GetTimeZoneFromSystem()["bias"].AsInt64() * 60;
        UpdateTimeShow();
    }


    [Export]
    double _unixTime;
    public double UnixTime
    {
        get => _unixTime;
        set
        {
            if (value != _unixTime)
            {
                _unixTime = value;
                UpdateTimeShow();
            }
        }
    }
    [Export]
    long _timeZone; // seconds
    public void UpdateTimeZone()
    {
        _timeZone = Time.GetTimeZoneFromSystem()["bias"].AsInt64() * 60;
        UpdateTimeShow();
    }


    int year;
    public int Year => year;
    int month;
    public int Month => month;
    int day;
    public int Day => day;
    int hour;
    public int Hour => hour;
    int minute;
    public int Minute => minute;
    int second;
    public int Second => second;
    public string TimeString_HMS => $"{hour.ToString("D2")}:{minute.ToString("D2")}:{second.ToString("D2")}";

    public string TimeString_HM => $"{hour.ToString("D2")}:{minute.ToString("D2")}";

    public string TimeString_YMD_HMS => $"{year}/{month}/{day} {TimeString_HMS}";


    void UpdateTimeShow()
    {
        Dictionary timeDict = Time.GetDatetimeDictFromUnixTime((long)_unixTime + _timeZone);
        year = timeDict["year"].AsInt32();
        month = timeDict["month"].AsInt32();
        day = timeDict["day"].AsInt32();
        hour = timeDict["hour"].AsInt32();
        minute = timeDict["minute"].AsInt32();
        second = timeDict["second"].AsInt32();
    }
}


public class TimeResJson
{
    public int year { get; set; }
    public int month { get; set; }
    public int day { get; set; }
    public int hour { get; set; }
    public int minute { get; set; }
    public int second { get; set; }
}