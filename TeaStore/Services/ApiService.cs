using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TeaStore.Models;

namespace TeaStore.Services
{
    public static class ApiService
    {
        public async static Task<bool> Registration(string name, string email, string phone, string password)
        {
            var reqister = new Register()
            {
                Name = name,
                Email = email,
                Phone = phone,
                Password = password
            };
            var httpclient = new HttpClient();
            var json = JsonConvert.SerializeObject(reqister);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpclient.PostAsync(AppSetting.ApiUrl + "api/Users/Register", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }
        public async static Task<bool> Login(string email, string password)
        {
            var login = new Login()
            {
                Email = email,
                Password = password
            };
            var httpclient = new HttpClient();
            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpclient.PostAsync(AppSetting.ApiUrl + "api/Users/login", content);
            if (!response.IsSuccessStatusCode) return false;
            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Toked>(jsonResult);
            Preferences.Set("accessToken", result.AccessToken);
            Preferences.Set("userId", result.UserId);
            Preferences.Set("userName", result.UserName);
            return true;
        }
        public static async Task<ProfileImage> GetUserProfileImage()
        {
            var httpclient = new HttpClient();
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpclient.GetStringAsync(AppSetting.ApiUrl + "api/users/profileimage");
            return JsonConvert.DeserializeObject<ProfileImage>(response);
        }
        public async static Task<bool> UploadUserImage(byte[] imageArray)
        {
           
            var httpclient = new HttpClient();
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(imageArray),"Image","image.jpg");
            var response = await httpclient.PostAsync(AppSetting.ApiUrl + "api/Users/uploadphoto", content);
            if (!response.IsSuccessStatusCode) return false;
           
            return true;
        }
        public static async Task<List<Category>> GetCategories()
        {
            var httpclient = new HttpClient();
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpclient.GetStringAsync(AppSetting.ApiUrl + "api/categories");
            return JsonConvert.DeserializeObject<List<Category>>(response);
        }
        public static async Task<List<Product>> GetProducts(string productType,string categoryId)
        {
            var httpclient = new HttpClient();
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpclient.GetStringAsync(AppSetting.ApiUrl + "api/products?producttype="+productType+"&categoryId="+categoryId);
            return JsonConvert.DeserializeObject<List<Product>>(response);
        }
        public static async Task<List<ProductDetail>> GetProductDetail(int productId)
        {
            var httpclient = new HttpClient();
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpclient.GetStringAsync(AppSetting.ApiUrl + "api/products/" + productId);
            return JsonConvert.DeserializeObject<List<ProductDetail>>(response);
        }
        public async static Task<bool> AddItemInCart(ShoppingCart shoppingCart)
        {
          
            var httpclient = new HttpClient();
            var json = JsonConvert.SerializeObject(shoppingCart);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpclient.PostAsync(AppSetting.ApiUrl + "api/shoppingcartitems", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }
        public static async Task<List<ShoppingCartItem>> GetShoppingCartItems(int userId)
        {
            var httpclient = new HttpClient();
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpclient.GetStringAsync(AppSetting.ApiUrl + "api/shoppingcartitems"+userId);
            return JsonConvert.DeserializeObject<List<ShoppingCartItem>>(response);
        }
        public async static Task<bool> UpdateCartQuantity(int productId,string action)
        {

            var httpclient = new HttpClient();
            var content = new StringContent(string.Empty);
            var response = await httpclient.PutAsync(AppSetting.ApiUrl + "api/shoppingcartitems?productId="+productId+"&action="+action, content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }
        public async static Task<bool> PlaceOrder(Order order)
        {

            var httpclient = new HttpClient();
            var json = JsonConvert.SerializeObject(order);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpclient.PostAsync(AppSetting.ApiUrl + "api/orders", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }
        public static async Task<List<OrderByUser>> GetOrderByUser(int userId)
        {
            var httpclient = new HttpClient();
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpclient.GetStringAsync(AppSetting.ApiUrl + "api/orders/orderbyuser/" + userId);
            return JsonConvert.DeserializeObject<List<OrderByUser>>(response);
        }
        public static async Task<List<OrderDetail>> GetOrderDetails(int orderId)
        {
            var httpclient = new HttpClient();
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpclient.GetStringAsync(AppSetting.ApiUrl + "api/orders/orderdetails/" + orderId);
            return JsonConvert.DeserializeObject<List<OrderDetail>>(response);
        }
    }
}
