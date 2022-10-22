using System.Text.Json;

namespace OcphAuthBlazorView
{
    public class Helper
    {
        public static JsonSerializerOptions? JsonOption => new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public static string Token { get; set; } = string.Empty;
    }
}
