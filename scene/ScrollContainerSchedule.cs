using Godot;
using System;
using System.Text.Json;
public partial class ScrollContainerSchedule : ScrollContainer
{
    [Export]
    ScheduleRes scheduleRes;

    [ExportGroup("Internal")]
    [Export]
    PackedScene contentFrame_PKS;
    ContentFrame NewContentFrame => contentFrame_PKS.Instantiate<ContentFrame>();
    [Export]
    PackedScene frameAdd_PKS;
    FrameAdd NewFrameAdd => frameAdd_PKS.Instantiate<FrameAdd>();

    [Export]
    VBoxContainer vBoxContainerRef;
    public override void _Ready()
    {
        if (scheduleRes != null)
        {
            LoadScheduleRes(scheduleRes);

            scheduleRes.OnScheduleResChangedEvent += LoadScheduleRes;

            // GD.Print(scheduleRes.ToJson());
        }
    }


    public void LoadScheduleRes(ScheduleRes scheduleRes)
    {
        Godot.Collections.Array<Node> children = vBoxContainerRef.GetChildren();
        foreach (Node child in children)
        {
            child.QueueFree();
        }

        if (scheduleRes != null && scheduleRes.Items != null)
        {
            foreach (ItemRes item in scheduleRes.Items)
            {
                ContentFrame contentFrame = NewContentFrame;
                vBoxContainerRef.AddChild(contentFrame);
                contentFrame.BindingItem_Res = item;
            }
        }

        FrameAdd frameAdd = NewFrameAdd;
        vBoxContainerRef.AddChild(frameAdd);
    }
}
