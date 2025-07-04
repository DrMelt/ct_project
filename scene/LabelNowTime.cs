using Godot;
using System;

public partial class LabelNowTime : Label
{


    double lastUpdateTime;

    public override void _Process(double delta)
    {
        lastUpdateTime += delta;
        if (lastUpdateTime > 1)
        {
            lastUpdateTime = 0;
            TimeRes timeRes = new();
            Text = " 现在时间：" + $"{timeRes.Day}日 {timeRes.TimeString_HM}";
        }
    }



}

