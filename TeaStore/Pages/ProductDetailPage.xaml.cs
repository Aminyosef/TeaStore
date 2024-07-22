
using TeaStore.Services;

namespace TeaStore.Pages;

public partial class ProductDetailPage : ContentPage
{
	public ProductDetailPage(int productId)
	{
		InitializeComponent();
		GetProductdetails(productId);
	}

    private async void GetProductdetails(int productId)
    {
		var product = await ApiService.GetProductDetail(productId);
		LblProductName.Text = product.Name;
		LblProductDescription.Text = product.Detail;
        LblProductPrice.Text=product.Price;
		ImgProduct.Source = product.FullImageUrl;
		LblTotalPrice.Text = product.Price;
    }
}