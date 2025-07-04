using Godot;
using System;

public partial class ButtonLoad : Button
{
    [Export]
    AppInitialize appInitializeRef;

    public override void _Pressed()
    {
        appInitializeRef.Load();
    }

}
