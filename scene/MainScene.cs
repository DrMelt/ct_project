using Godot;
using System;

public partial class MainScene : Control
{
    [Export]
    RuntimeState_Res runtimeState_Res;


    public override void _Ready()
    {
        runtimeState_Res.SceneRoot = this;
    }

}
