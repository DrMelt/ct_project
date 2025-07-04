using Godot;
using System;

public partial class ScheduleData : Node
{
    [Export]
    ScheduleRes scheduleRes;
    public ScheduleRes Schedule_Res => scheduleRes;
}
