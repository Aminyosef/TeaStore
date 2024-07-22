
using TeaStore.Models;
using TeaStore.Services;

namespace TeaStore.Pages;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
        LblUserName.Text="HI " + Preferences.Get("userName", string.Empty);
		GetCategories();
		GetTrendingProducts();
		GetBestSellingProduct();
	}

    private async void GetBestSellingProduct()
    {
        var Products = await ApiService.GetProducts("bestselling", string.Empty);
		CvBestSelling.ItemsSource = Products;
    }

    private async void GetTrendingProducts()
    {
		var Products=await ApiService.GetProducts("trending", string.Empty);
		CvTrending.ItemsSource = Products;
			}

    private async void GetTrendingProperties()
	{
		var products = await ApiService.GetProducts("trending", string.Empty);
		CvTrending.ItemsSource = products;
	}
	private async void GetCategories()
	{
		var Categories=await ApiService.GetCategories();
        CvCategories.ItemsSource = Categories;
	}

    private void CvCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
	var currentSelction=	e.CurrentSelection.FirstOrDefault() as Category;
		if (currentSelction == null) return;
		Navigation.PushAsync(new ProductListPage(currentSelction.Id));
		((CollectionView)sender).SelectedItem= null;
    }
}