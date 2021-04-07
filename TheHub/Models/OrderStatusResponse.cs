using Newtonsoft.Json;

namespace TheHub.Models
{
    public class OrderStatusResponse
    {
        [JsonProperty("code")] public string Code { get; set; }

        [JsonProperty("message")] public string Message { get; set; }
    }
}