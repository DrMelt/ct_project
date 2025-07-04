using Godot;
using System;

public partial class ContentFrame : MarginContainer
{
    [ExportGroup("Internal")]
    [Export]
    RuntimeState_Res runtimeState_Res;

    [Export]
    Label labelTitle;
    [Export]
    Label labelDescription;
    [Export]
    HBoxContainerTimeShow timeShowRes;

    [Export]
    Button bindClickedButton;
    [Export]
    PackedScene contentFrameEditor_PKS;
    ContentFrameEditor NewContentFrameEditor => contentFrameEditor_PKS.Instantiate<ContentFrameEditor>();


    ItemRes _bindingItem_Res;
    public ItemRes BindingItem_Res
    {
        get => _bindingItem_Res;
        set
        {
            if (_bindingItem_Res != null)
            {
                _bindingItem_Res.ItemChangedEvent -= UpdateItem;
            }
            if (value != null)
            {
                value.ItemChangedEvent += UpdateItem;
            }
            _bindingItem_Res = value;
            UpdateItem(value);
        }
    }


    public override void _Ready()
    {
        bindClickedButton.Pressed += OnClickedButtonPressed;
    }
    public override void _ExitTree()
    {
        if (BindingItem_Res != null)
        {
            BindingItem_Res.ItemChangedEvent -= UpdateItem;
        }
    }


    private void OnClickedButtonPressed()
    {
        Control sceneRoot = runtimeState_Res.SceneRoot;
        ContentFrameEditor contentFrameEditor = NewContentFrameEditor;

        sceneRoot.AddChild(contentFrameEditor);
        contentFrameEditor.Init(BindingItem_Res);
    }


    void UpdateItem(ItemRes itemRes)
    {
        labelTitle.Text = itemRes.Name;
        labelDescription.Text = itemRes.Description;
        timeShowRes.UpdateWithTime(itemRes.Time_Res);
    }


}
