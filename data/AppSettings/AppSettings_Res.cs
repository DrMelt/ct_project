using Godot;
using System;
using System.Text.Json;

[GlobalClass]
public partial class AppSettings_Res : Resource
{
    [Export]
    public string key_GPT = "";
    [Export]
    public string key_Gaode = "";

    public void FromJsonString(string json)
    {
        var appSettingsJson = AppSettingsJson.Deserialize(json);
        FromJson(appSettingsJson);
    }
    public void FromJson(AppSettingsJson appSettingsJson)
    {
        if (appSettingsJson == null)
        {
            return;
        }

        key_GPT = appSettingsJson.key_GPT;
        key_Gaode = appSettingsJson.key_Gaode;
    }

    public AppSettingsJson ToJson()
    {
        return new AppSettingsJson()
        {
            key_GPT = key_GPT,
            key_Gaode = key_Gaode,
        };
    }
    internal string ToJsonString()
    {
        return JsonSerializer.Serialize(ToJson());
    }
}

public class AppSettingsJson
{
    public string key_GPT { get; set; }
    public string key_Gaode { get; set; }

    public static AppSettingsJson Deserialize(string json)
    {
        AppSettingsJson res;
        try
        {
            res = JsonSerializer.Deserialize<AppSettingsJson>(json);
        }
        catch (Exception e)
        {
            GD.PrintErr(e.Message);
            throw;
        }

        return res;
    }
}