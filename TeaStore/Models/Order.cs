using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaStore.Models
{
    public class Order
    {
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("orderTotal")]
        public int OrderTotal { get; set; }
        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
}
