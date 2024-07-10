using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeaStore.Models
{
    public  class ShoppingCartItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("price")]
        public int Price { get; set; }
        [JsonProperty("qty")]
        public int Qty { get; set; }
        [JsonProperty("totalAmount")]
        public int TotalAmount { get; set; }
        [JsonProperty("productId")]
        public int ProductId { get; set; }
        [JsonProperty("productName")]
        public string ProductName { get; set; }
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
    }
}
