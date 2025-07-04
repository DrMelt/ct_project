using Godot;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using static GPT_API;

public partial class RunTest : Node
{
    [Export]
    AppSettings_Res appSettings_Res;

    public override void _Ready()
    {
    }


    void SendAroundRequestTest()
    {
        Task.Run(async () =>
        {
            AmapAPI.SearchResponseJson res = await AmapAPI.SendAroundRequest("ffae8436f60719e48bf0759be6b97c27", "上海");

            foreach (var item in res.pois)
            {
                GD.Print(item.name);
            }
        });
    }

    void SendTextRequestTest()
    {
        Task.Run(async () =>
        {
            AmapAPI.SearchResponseJson res = await AmapAPI.SendTextRequest("ffae8436f60719e48bf0759be6b97c27", "上海景点");

            foreach (var item in res.pois)
            {
                GD.Print(item.name);
                GD.Print(item.address);
                GD.Print(item.tag);
            }
        });

    }



}
