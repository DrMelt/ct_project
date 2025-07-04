using Godot;
using System;

public partial class HBoxContainerTimeShow : HBoxContainer
{
    [ExportGroup("Internal")]
    [Export]
    Color futureTimeColor;
    [Export]
    Color pastTimeColor;

    [Export]
    Label labelTimeRef;


    public void UpdateWithTime(TimeRes timeRes)
    {
        labelTimeRef.LabelSettings.FontColor = timeRes.UnixTime > new TimeRes().UnixTime ? futureTimeColor : pastTimeColor;

        string timeText = $"{timeRes.Day}日 {timeRes.TimeString_HM}";
        labelTimeRef.Text = timeText;
    }


}
