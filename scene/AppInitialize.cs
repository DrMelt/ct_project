using Godot;
using System;
using System.Text.Json;

public partial class AppInitialize : Node
{
    [Export]
    ScheduleRes scheduleRes;
    [Export]
    AppSettings_Res settingsRes;

    [Export]
    RuntimeState_Res runtimeStateRes;

    readonly string saveJson_Path = "user://app_save.save";

    public override void _Ready()
    {
        // GD.Print(scheduleRes.ToJsonString());
        Load();
    }

    public void Load()
    {
        AppSave appSave = AppSave.GetAppSave(saveJson_Path);
        scheduleRes.FromJsonStringWithHistory(appSave.scheduleJson);
        settingsRes.FromJsonString(appSave.appSettingsJson);
    }
    public void Save()
    {
        AppSave appSave = new AppSave()
        {
            scheduleJson = scheduleRes.ToJsonString(),
            appSettingsJson = settingsRes.ToJsonString()
        };
        AppSave.SaveAppSave(appSave, saveJson_Path);
    }

}

