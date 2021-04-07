using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace TheHub.Models
{
    public class OrderResponse
    {
        [JsonProperty("epicor_order_number")] public string EpicorOrderNumber { get; set; }
       
        [JsonProperty("messages")]
        public List<ErrorMessage> Messages { get; set; }
    }
}