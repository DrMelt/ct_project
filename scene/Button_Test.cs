using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;

public partial class Button_Test : Button
{

    [ExportGroup("texts")]
    [Export]
    string text_Normal = "发送";
    [Export]
    string text_Waiting = "等待中";

    [ExportGroup("references")]
    [Export]
    ScheduleRes scheduleRes;
    [Export]
    AppSettings_Res appSettingsRes;


    [Export]
    Label labelRef;
    [Export]
    TextEdit textEditRef;


    bool _isWaiting = false;
    bool IsWaiting
    {
        get => _isWaiting;
        set
        {
            if (_isWaiting != value)
            {
                _isWaiting = value;
                CallDeferred(nameof(UpdateButton));
            }
        }
    }
    void UpdateButton()
    {
        Text = IsWaiting ? text_Waiting : text_Normal;
    }

    public override void _Ready()
    {
        UpdateButton();
    }

    public override void _Pressed()
    {
        labelRef.Text = "";
        if (IsWaiting)
        {
            return;
        }
        if (appSettingsRes.key_GPT == "")
        {
            labelRef.Text = "请在设置中填写阿里云GPT Key。";
            return;
        }
        if (appSettingsRes.key_Gaode == "")
        {
            labelRef.Text = "请在设置中填写高德 Key。";
        }


        IsWaiting = true;
        Task.Run(Send);
    }

    void CleanTextEdit()
    {
        textEditRef.Text = "";
    }

    async Task Send()
    {
        GPT_API.ApiResponse apiResponse = await GPT_API.Send(scheduleRes, new TimeRes(), textEditRef.Text, appSettingsRes.key_GPT, appSettingsRes.key_Gaode);
        CallDeferred(nameof(CleanTextEdit));


        ScheduleResJson scheduleResJson = null;
        try
        {
            string json = apiResponse.choices.Last().message.content;
            scheduleResJson = ScheduleResJson.Deserialize(json);
        }
        catch (Exception e)
        {
            labelRef.CallDeferred(nameof(Text), "创建失败，返回信息有误。");
            IsWaiting = false;
            GD.Print(e.Message);
        }

        scheduleRes.CallDeferred(nameof(scheduleRes.FromJsonStringWithHistory), scheduleResJson.Serialize());
        IsWaiting = false;
    }
}

