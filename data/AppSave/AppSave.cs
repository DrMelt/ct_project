using Godot;
using System;
using System.Text.Json;

public class AppSave
{
    public static AppSave GetAppSave(string savePath)
    {
        if (!FileAccess.FileExists(savePath))
        {
            GD.Print("saveJson_Path does not exist.");
            return null;
        }

        using FileAccess saveFile = FileAccess.Open(savePath, FileAccess.ModeFlags.Read);
        string json = saveFile.GetAsText();

        try
        {
            var appSave = JsonSerializer.Deserialize<AppSave>(json);
            return appSave;
        }
        catch (Exception e)
        {
            GD.Print(e);
        }

        return null;
    }

    public static void SaveAppSave(AppSave appSave, string savePath)
    {
        using FileAccess saveFile = FileAccess.Open(savePath, FileAccess.ModeFlags.Write);
        saveFile.StoreString(JsonSerializer.Serialize(appSave));
    }

    public string scheduleJson { set; get; }
    public string appSettingsJson { set; get; }
}