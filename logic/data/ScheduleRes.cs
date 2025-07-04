using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


[GlobalClass]
public partial class ScheduleRes : Resource
{
    public event Action<ScheduleRes> OnScheduleResChangedEvent;

    Stack<ScheduleResJson> changeHistory = new Stack<ScheduleResJson>();
    public void SaveHistory()
    {
        changeHistory.Push(ToJson());
    }


    public ScheduleRes() { }

    public ScheduleRes(ScheduleResJson scheduleResJson)
    {
        items = new Array<ItemRes>();
        foreach (ItemResJson itemResJson in scheduleResJson.items)
        {
            items.Add(new ItemRes(itemResJson));
        }
    }

    public void FromJsonStringWithHistory(string json)
    {
        try
        {
            ScheduleResJson scheduleResJson = ScheduleResJson.Deserialize(json);
            FromJsonWithHistory(scheduleResJson);
        }
        catch (Exception e)
        {
            GD.PrintErr($"JsonSerializer error: {e.Message}");
        }
    }


    public void FromJsonWithHistory(ScheduleResJson scheduleResJson)
    {
        SaveHistory();

        FromJson(scheduleResJson);
    }

    void FromJson(ScheduleResJson scheduleResJson)
    {
        items = new Array<ItemRes>();
        foreach (ItemResJson itemResJson in scheduleResJson.items)
        {
            items.Add(new ItemRes(itemResJson));
        }
        TimeSort();

        OnScheduleResChangedEvent?.Invoke(this);
    }


    public void Undo()
    {
        if (changeHistory.Count > 0)
        {
            var top = changeHistory.Pop();
            FromJson(top);
        }
    }


    public ScheduleResJson ToJson()
    {
        ScheduleResJson scheduleResJson = new ScheduleResJson()
        {
            items = new List<ItemResJson>(),
        };
        foreach (ItemRes itemRes in items)
        {
            scheduleResJson.items.Add(itemRes.ToJson());
        }
        return scheduleResJson;
    }

    public string ToJsonString()
    {
        return JsonSerializer.Serialize(ToJson());
    }


    public void TimeSortWithHistory()
    {
        SaveHistory();
        TimeSort();
    }

    void TimeSort()
    {
        var list = new List<ItemRes>(items);
        list.Sort((a, b) => a.Time_Res.UnixTime.CompareTo(b.Time_Res.UnixTime));
        items = [.. list];
    }




    [Export]
    private Array<ItemRes> items;
    public Array<ItemRes> Items => items;

    public void AddItem(ItemRes itemRes)
    {
        items.Add(itemRes);
        OnScheduleResChangedEvent?.Invoke(this);
    }

}

public partial class ScheduleResJson
{
    public List<ItemResJson> items { get; set; }

    public static ScheduleResJson Deserialize(string json)
    {
        ScheduleResJson res;
        try
        {
            res = JsonSerializer.Deserialize<ScheduleResJson>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr(e.Message);
            throw;
        }

        return res;
    }

    public string Serialize()
    {
        return JsonSerializer.Serialize(this);
    }
}