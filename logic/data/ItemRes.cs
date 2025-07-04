using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

[GlobalClass]
public partial class ItemRes : Resource
{
    public ItemRes()
    {
        timeRes = new TimeRes();
        name = "事件标题";
        description = "事件描述";
    }

    public ItemRes(ItemResJson itemResJson)
    {
        timeRes = new TimeRes(itemResJson.timeResJson);
        name = itemResJson.name;
        description = itemResJson.description;
    }
    public ItemResJson ToJson()
    {
        return new ItemResJson()
        {
            timeResJson = timeRes.ToJson(),
            name = name,
            description = description,
        };
    }
    public event Action<ItemRes> ItemChangedEvent;


    [Export]
    TimeRes timeRes;
    public TimeRes Time_Res => timeRes;
    [Export]
    string name;
    public string Name
    {
        get => name; set
        {
            name = value;
            ItemChangedEvent?.Invoke(this);
        }
    }
    [Export]
    string description;
    public string Description
    {
        get => description;
        set
        {
            description = value;
            ItemChangedEvent?.Invoke(this);
        }
    }


}



public class ItemResJson
{
    public TimeResJson timeResJson { get; set; }
    public string name { get; set; }
    public string description { get; set; }

}