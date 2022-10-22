using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OcphAuthBlazorView.Extentions
{
    public static class GenerateContentToJsonExtention
    {
        public static HttpContent GenerateContentToJson(this HttpClient client, object model)
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;

        }
    }



}
