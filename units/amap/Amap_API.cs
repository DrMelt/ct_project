using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Godot;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;


public class AmapAPI
{
    private static readonly System.Net.Http.HttpClient httpClient = new();
    private static readonly string apiUrl_map = "https://restapi.amap.com/v3/staticmap";
    private static readonly string apiUrl_navi = "https://m.amap.com/navi/";


    public static string GetNaviUrl(string apiKey, string location, string zoom, string size)
    {
        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "key", apiKey },
            { "location", "116.481485,39.990464" },
            { "zoom", "10" },
            { "size", "750*300" },
        };


        return apiUrl_navi;
    }

    public static string GetMapUrl(string apiKey)
    {

        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "location", "116.481485,39.990464" },
            { "zoom", "10" },
            { "size", "750*300" },
            { "key", apiKey }
        };

        return BuildUrlWithParams(apiUrl_map, parameters);
    }



    #region Geo
    private static readonly string apiUrl_geo = "https://restapi.amap.com/v3/geocode/geo";
    public class GeoResponseJson
    {
        public string status { get; set; }       // status
        public string count { get; set; }        // count
        public string info { get; set; }         // info

        public List<Geocode> geocodes { get; set; }  // geocodes array

        public class Geocode
        {
            public string country { get; set; }     // country
            public string province { get; set; }    // province
            public string city { get; set; }        // city
            public string citycode { get; set; }    // citycode
            public string district { get; set; }    // district
            public string street { get; set; }      // street
            public string number { get; set; }      // number
            public string adcode { get; set; }      // adcode
            public string location { get; set; }    // location (longitude,latitude)
            public string level { get; set; }       // level
        }
    }
    public static string GetGeoUrl(string apiKey, string address, string city = "")
    {
        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            {"key", apiKey},
            {"address", address},
            {"city", city}
        };

        return BuildUrlWithParams(apiUrl_geo, parameters);
    }

    public async static Task<GeoResponseJson> SendGeoRequest(string apiKey, string address, string city = "")
    {
        string url = GetGeoUrl(apiKey, address, city);
        return await SendRequest<GeoResponseJson>(url);
    }
    #endregion


    public class SearchResponseJson
    {
        public string status { get; set; }      // 结果状态值：0（失败）或1（成功）
        public string info { get; set; }         // 返回状态说明，如“OK”或错误原因
        public string count { get; set; }        // 搜索方案数目
        public string infocode { get; set; }
        public Suggestion suggestion { get; set; }  // 城市建议列表

        public List<City> cities { get; set; }      // 城市列表
        public List<Poi> pois { get; set; }         // POI 信息列表

        public class Suggestion
        {
            public List<string> keywords { get; set; }   // 关键字
            public List<CitySuggestion> cities { get; set; }  // 城市建议列表

            public class CitySuggestion
            {
                public string name { get; set; }     // 城市名称
                public string num { get; set; }         // 该城市包含此关键字的个数
                public string citycode { get; set; } // 城市编码
                public string adcode { get; set; }   // 区域编码
            }

        }
        public class City
        {
            public string name { get; set; }     // 城市名称
            public string citycode { get; set; } // 城市编码
            public string adcode { get; set; }   // 区域编码
        }
        public class Poi
        {
            public string id { get; set; }               // POI 的唯一 ID
            public string parent { get; set; }           // 父 POI 的 ID，可能为空
            public string name { get; set; }             // 名称
            public string type { get; set; }             // 兴趣点类型（大类;中类;小类）
            public string typecode { get; set; }         // 兴趣点类型编码（例如：050118）
            public string biz_type { get; set; }         // 行业类型
            public string address { get; set; }          // 地址
            public string location { get; set; }         // 经纬度，格式：X,Y
            public string distance { get; set; }         // 离中心点距离（单位：米；仅在周边搜索时有值）
            public string tel { get; set; }              // 电话
            public string postcode { get; set; }         // 邮编（extensions=all 时返回）
            public string website { get; set; }          // 网址（extensions=all 时返回）
            public string email { get; set; }            // 电子邮箱（extensions=all 时返回）
            public string pcode { get; set; }            // 所在省份编码（extensions=all 时返回）
            public string pname { get; set; }            // 所在省份名称（若是直辖市则为市名，如北京市）
            public string citycode { get; set; }         // 城市编码（extensions=all 时返回）
            public string cityname { get; set; }         // 城市名（若是直辖市则为市名，如北京市）
            public string adcode { get; set; }           // 区域编码（extensions=all 时返回）
            public string adname { get; set; }           // 区域名称（区县级别的返回，如朝阳区）
            public string entr_location { get; set; }    // 入口经纬度（extensions=all 时返回）
            public string exit_location { get; set; }    // 出口经纬度（目前不会返回）
            public string navi_poiid { get; set; }       // 导航 ID（extensions=all 时返回）
            public string gridcode { get; set; }         // 地理格ID（extensions=all 时返回）
            public string alias { get; set; }            // 别名（extensions=all 时返回）
            public string parking_type { get; set; }     // 停车场类型（地下、地面、路边）（extensions=all 时返回）
            public string tag { get; set; }              // 特色内容（如美食类POI的特色菜）（extensions=all 时返回）
            public string indoor_map { get; set; }       // 是否有室内地图标志（1=有，0=无）（extensions=all 时返回）
            public IndoorData indoor_data { get; set; }  // 室内地图相关数据（indoor_map=0时不返回）（extensions=all 时返回）

            public string groupbuy_num { get; set; }     // 团购数据（逐渐废弃）
            public string business_area { get; set; }    // 所属商圈（extensions=all 时返回）
            public string discount_num { get; set; }     // 优惠信息数目（逐渐废弃）
            public Biz_ext biz_ext { get; set; }          // 深度信息（extensions=all 时返回）


            public List<Photo> photos { get; set; }      // 照片相关信息（extensions=all 时返回）

            public class Photo
            {
                public string title { get; set; }   // 图片介绍
                public string url { get; set; }     // 图片链接
            }

            public class IndoorData
            {
                public string cpid { get; set; }     // 当前 POI 的父级 POI
                public string floor { get; set; }    // 楼层索引（数字表示，如8）
                public string truefloor { get; set; }// 所在楼层（带字母，如F8）
            }

            public class Biz_ext
            {
                public string rating { get; set; }           // 评分（餐饮、酒店、景点、影院类POI存在）
                public string cost { get; set; }             // 人均消费（餐饮、酒店、景点、影院类POI存在）
                public string meal_ordering { get; set; }    // 是否可订餐（餐饮相关POI存在，逐渐废弃）
                public string seat_ordering { get; set; }    // 是否可选座（影院相关POI存在，逐渐废弃）
                public string ticket_ordering { get; set; }  // 是否可订票（景点相关POI存在，逐渐废弃）
                public string hotel_ordering { get; set; }   // 是否可订房（酒店相关POI存在，逐渐废弃）
            }
        }


    }



    #region Text
    private static readonly string apiUrl_text = "https://restapi.amap.com/v3/place/text";
    public static string GetTextUrl(string apiKey, string keywords = "", string types = "", string city = "")
    {
        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            {"key", apiKey},
            {"keywords", keywords},
            {"types", types},
            {"city", city},
            {"offset", "5"},
            {"extensions", "all"},
        };

        return BuildUrlWithParams(apiUrl_text, parameters);
    }

    public async static Task<SearchResponseJson> SendTextRequest(string apiKey, string keywords = "", string types = "", string city = "")
    {
        return await SendRequest<SearchResponseJson>(GetTextUrl(apiKey, keywords, types, city));
    }


    #endregion

    #region Around

    private static readonly string apiUrl_around = "https://restapi.amap.com/v3/place/around";
    public static string GetAroundUrl(string apiKey, string location, string keywords = "", string types = "", string city = "", string radius = "")
    {
        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            {"key", apiKey},
            {"location", location},
            {"keywords", keywords},
            {"types", types},
            {"city", city},
            {"radius", radius},
            {"offset", "5"},
            {"extensions", "all"},
        };

        return BuildUrlWithParams(apiUrl_around, parameters);
    }

    public async static Task<SearchResponseJson> SendAroundRequest(string apiKey, string location, string keywords = "", string types = "", string city = "", string radius = "")
    {
        return await SendRequest<SearchResponseJson>(GetAroundUrl(apiKey, location, keywords, types, city, radius));
    }

    #endregion

    public async static Task<T> SendRequest<T>(string url)
    {
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode(); // 如果失败状态码，抛出异常
            string content = await response.Content.ReadAsStringAsync();

            // GD.Print(content);
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new NullArrayConverter());


            return JsonSerializer.Deserialize<T>(content, options); // 使用自定义转换器
        }
        catch (Exception e)
        {
            GD.Print(e.Message);
            return default;
        }

    }

    private static string BuildUrlWithParams(string baseUrl, Dictionary<string, string> parameters)
    {
        string query = string.Join("&", parameters.Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));
        return $"{baseUrl}?{query}";
    }

    public class NullArrayConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                using (JsonDocument jsonDoc = JsonDocument.ParseValue(ref reader))
                {
                    JsonElement array = jsonDoc.RootElement;
                    if (array.GetArrayLength() == 0)
                    {
                        return null;
                    }
                }
            }

            return JsonSerializer.Deserialize(ref reader, typeToConvert) as string;
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }


}


