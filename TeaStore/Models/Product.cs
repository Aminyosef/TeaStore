using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaStore.Models
{
    public class Product
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")] 
        public string Name { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
    }
}
