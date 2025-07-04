using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Godot;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
public class GPT_API
{
    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    #region Send Message
    public class RequestModel
    {
        public string model { get; set; }
        public List<Message> messages { get; set; }
    }
    #endregion

    #region Receive Message
    public class PromptTokensDetails
    {
        public int cached_tokens { get; set; }
    }

    public class ApiResponse
    {
        public List<Choice> choices { get; set; }
        public string @object { get; set; }
        public Usage usage { get; set; }
        public long created { get; set; }
        public object system_fingerprint { get; set; } // 可能为 null
        public string model { get; set; }
        public string id { get; set; }
        public class Usage
        {
            public int prompt_tokens { get; set; }
            public int completion_tokens { get; set; }
            public int total_tokens { get; set; }
            public PromptTokensDetails prompt_tokens_details { get; set; }
        }
        public class Choice
        {
            public Message message { get; set; }
            public string finish_reason { get; set; }
            public int index { get; set; }
            public object logprobs { get; set; } // 可以是 null 或更复杂的类型
        }
    }
    #endregion

    public class SearchRequest
    {
        public bool needSearch { set; get; }
        public List<string> searchText { set; get; }
    }

    private static readonly System.Net.Http.HttpClient httpClient = new();
    private static readonly string apiUrl = "https://dashscope.aliyuncs.com/compatible-mode/v1/chat/completions";

    async public static Task<ApiResponse> Send(ScheduleRes scheduleRes, TimeRes currentTime, string userText, string apiKey, string gaodeKey)
    {
        // 此处以qwen-plus为例，可按需更换模型名称。模型列表：https://help.aliyun.com/zh/model-studio/getting-started/models
        string extraneousInformation = "";

        bool IS_DEBUG = true;
        Action<string> PrintInfo = (string info) =>
        {
            if (IS_DEBUG)
            {
                GD.Print(info);
                GD.Print();
                GD.Print();
            }
        };


        string searchRequestJsonTemp = """
        {"needSearch":false,"searchText":["上海景点","上海美食"]}
        """;
        RequestModel isNeedSearchRequest = new RequestModel
        {
            model = "qwen-plus",
            messages = new List<Message>
            {
                new Message { role = "system", content = $"你是一个行程安排小助手。请根据用户要求判断是否需要使用地点搜索功能并依据json样板生成返回值，返回字段开头为{{，结尾为}}，只需要返回json部分。这是高德地图搜索，只能搜索地点相关信息，比如“上海景区”。现在时间是{currentTime.TimeString_YMD_HMS}。现在的行程安排为{scheduleRes.ToJsonString()}。json样板为{searchRequestJsonTemp}。" },
                new Message { role = "user", content = userText},
            }
        };
        ApiResponse isNeedSearchResponse = await GetResponse(isNeedSearchRequest, apiKey);
        string isNeedSearchJson = isNeedSearchResponse.choices.Last().message.content;
        PrintInfo.Invoke($"{nameof(isNeedSearchJson)}: {isNeedSearchJson}");
        SearchRequest searchRequest = JsonSerializer.Deserialize<SearchRequest>(isNeedSearchJson);


        if (searchRequest != null)
        {
            if (searchRequest.needSearch)
            {
                List<AmapAPI.SearchResponseJson.Poi> searchRes = [];
                foreach (var searchText in searchRequest.searchText)
                {
                    AmapAPI.SearchResponseJson res = await AmapAPI.SendTextRequest(gaodeKey, searchText);
                    searchRes.AddRange(res.pois);
                }



                RequestModel fetchInformationRequest = new RequestModel
                {
                    model = "qwen-turbo",
                    messages = new List<Message>
                    {
                        new Message { role = "system", content = $"你是一个行程安排小助手。请根据用户要求提取额外信息中可能的有效信息。现在时间是{currentTime.TimeString_YMD_HMS}。现在的行程安排为{scheduleRes.ToJsonString()}。提供的额外信息为{JsonSerializer.Serialize(searchRes)}" },
                        new Message { role = "user", content = userText},
                    }
                };
                ApiResponse fetchInformationResponse = await GetResponse(fetchInformationRequest, apiKey);
                string fetchInformationJson = fetchInformationResponse.choices.Last().message.content;
                PrintInfo.Invoke($"{nameof(fetchInformationJson)}: {fetchInformationJson}");

                extraneousInformation += fetchInformationJson;
            }
        }


        string jsonTemp = @"{""items"":[{""timeResJson"":{""year"":2025,""month"":6,""day"":29,""hour"":12,""minute"":0,""second"":0},""name"":""Name"",""description"":""Description""}]}";

        RequestModel modificationRequest = new RequestModel
        {
            model = "qwen-plus",
            messages = new List<Message>
            {
                new Message { role = "system", content = $"你是一个行程安排小助手。请根据用户要求修改或生成对应的行程计划。现在时间是{currentTime.TimeString_YMD_HMS}。现在的行程安排为{scheduleRes.ToJsonString()}。额外信息为{extraneousInformation}。json样板为{jsonTemp}。" },
                new Message { role = "user", content = userText},
            }
        };
        ApiResponse apiResponse = await GetResponse(modificationRequest, apiKey);
        string modificationJson = apiResponse.choices.Last().message.content;
        PrintInfo.Invoke($"{nameof(modificationJson)}: {modificationJson}");



        RequestModel finalRequest = new RequestModel
        {
            model = "qwen-turbo",
            messages = new List<Message>
            {
                new Message { role = "system", content = $"你是一个行程安排小助手。请结合现在的行程安排和修改的行程安排，按照修改合成json格式的行程计划，返回字段开头为{{，结尾为}}，只需要返回json内容。现在的行程安排为{scheduleRes.ToJsonString()}。修改行程安排为{modificationJson}。json样板为{jsonTemp}。" },
                new Message { role = "user", content = userText},
            }
        };
        ApiResponse result = await GetResponse(finalRequest, apiKey);

        return result;
    }

    private static async Task<ApiResponse> GetResponse(RequestModel request, string apiKey)
    {
        string jsonContent = JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true });
        ApiResponse apiResponse = await SendPostRequestAsync(apiUrl, jsonContent, apiKey);
        return apiResponse;
    }

    private static async Task<ApiResponse> SendPostRequestAsync(string url, string jsonContent, string apiKey)
    {
        using (StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
        {
            // 设置请求头
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // 发送请求并获取响应
            HttpResponseMessage response = await httpClient.PostAsync(url, content);

            // 处理响应
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                ApiResponse apiResponse = ParseResult(responseJson);
                return apiResponse;
            }
            else
            {
                GD.Print($"SendPostRequestAsync failed: {response.StatusCode}");
                return null;
            }
        }
    }


    private static ApiResponse ParseResult(string jsonResponse)
    {
        try
        {
            ApiResponse apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonResponse);

            // foreach (var choice in apiResponse.choices)
            // {
            //     GD.Print($"Role: {choice.message.role}");
            //     GD.Print($"Content: {choice.message.content}");
            //     GD.Print($"Finish Reason: {choice.finish_reason}");
            // }

            // GD.Print($"Model: {apiResponse.model}");
            // GD.Print($"Total Tokens: {apiResponse.usage.total_tokens}");

            return apiResponse;
        }
        catch (Exception ex)
        {
            GD.Print($"解析失败: {ex.Message}");
            return null;
        }
    }

}