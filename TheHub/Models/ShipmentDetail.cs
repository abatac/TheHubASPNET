using System.Collections.Generic;
using Newtonsoft.Json;

namespace TheHub.Models
{
    public class ShipmentDetail
    {
        [JsonProperty("product_code")] public string ProductCode { get; set; }

        [JsonProperty("product_short_name")] public string ProductShortName { get; set; }

        [JsonProperty("quantity_shipped")] public string QuantityShipped { get; set; }

        [JsonProperty("order_item_number")] public string OrderItemNumber { get; set; }

        [JsonProperty("imei_list")] public List<string> IMEIList { get; set; }

        [JsonProperty("sensor_list", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> SensorList { get; set; }
    }
}