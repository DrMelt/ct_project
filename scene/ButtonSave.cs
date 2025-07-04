using Godot;
using System;

public partial class ButtonSave : Button
{
    [Export]
    AppInitialize appInitializeRef;

    public override void _Pressed()
    {
        appInitializeRef.Save();
    }


}
