using Godot;
using System;

public partial class FrameAdd : MarginContainer
{
    [ExportGroup("Internal")]
    [Export]
    ScheduleRes schedule_Res;

    [Export]
    Button buttonRef;

    public override void _Ready()
    {
        buttonRef.Pressed += ButtonPressed;
    }

    private void ButtonPressed()
    {
        schedule_Res.AddItem(new ItemRes());
    }

}
