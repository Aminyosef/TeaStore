
using TeaStore.Models;
using TeaStore.Services;

namespace TeaStore.Pages;

public partial class ProductListPage : ContentPage
{
	public ProductListPage(int categoryId)
	{
		InitializeComponent();
		GetProductsAsync(categoryId);
	}

    private async Task GetProductsAsync(int categoryId)
    {
		var products=await ApiService.GetProducts("category", categoryId.ToString());
		CvProducts.ItemsSource = products;
    }

    private async void CvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
		var currentSelction = e.CurrentSelection.FirstOrDefault() as Product;
		if (currentSelction == null) return;
		Navigation.PushAsync(new ProductDetailPage(currentSelction.Id));
		((CollectionView)sender).SelectedItem = null;
    }
}