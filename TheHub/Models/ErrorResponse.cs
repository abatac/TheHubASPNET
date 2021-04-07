using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheHub.Models
{
    public class ErrorResponse
    {
        [JsonProperty("messages")]
        public List<ErrorMessage> Messages { get; set; }
    }
}