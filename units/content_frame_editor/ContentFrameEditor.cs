using Godot;
using System;

public partial class ContentFrameEditor : Control
{
    [ExportGroup("Internal")]
    [Export]
    LableLineEdit nameEdit;
    [Export]
    LableTextEdit descriptionEdit;


    [Export]
    Button bindButton_Close;

    ItemRes bindItem_Res;

    public override void _Ready()
    {
        bindButton_Close.Pressed += OnButtonClose_Pressed;
    }

    private void OnButtonClose_Pressed()
    {
        bindItem_Res.Name = nameEdit.GetText();
        bindItem_Res.Description = descriptionEdit.GetText();

        QueueFree();
    }


    public void Init(ItemRes itemRes)
    {
        bindItem_Res = itemRes;
        nameEdit.Init("事件名称", itemRes.Name);
        descriptionEdit.Init("事件描述", itemRes.Description);
    }
}
