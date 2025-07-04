using Godot;
using System;

public partial class ButtonSetting : Button
{
    [Export]
    RuntimeState_Res runtimeState_Res;

    [Export]
    PackedScene settingPKS;
    Settings NewSetting => settingPKS.Instantiate<Settings>();

    public override void _Pressed()
    {
        Settings setting = NewSetting;
        runtimeState_Res.SceneRoot.AddChild(setting);
    }

}
