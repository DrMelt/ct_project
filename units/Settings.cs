using Godot;
using System;

public partial class Settings : Control
{
    [Export]
    Button bindButton_Close;
    [Export]
    AppSettings_Res appSettings_Res;

    [Export]
    LableLineEdit lableLineEdit_GPT;
    [Export]
    LableLineEdit lableLineEdit_Gaode;

    public override void _Ready()
    {
        bindButton_Close.Pressed += OnClosePressed;

        lableLineEdit_GPT.Init("GPT Key", appSettings_Res.key_GPT);
        lableLineEdit_Gaode.Init("高德 Key", appSettings_Res.key_Gaode);
    }

    private void OnClosePressed()
    {
        appSettings_Res.key_GPT = lableLineEdit_GPT.GetText();
        appSettings_Res.key_Gaode = lableLineEdit_Gaode.GetText();

        QueueFree();
    }

}
