using TeaStore.Services;

namespace TeaStore.Pages;

public partial class SignupPage : ContentPage
{
	public SignupPage()
	{
		InitializeComponent();
	}

    private async void BtnSignup_Clicked(object sender, EventArgs e)
    {
		var response=await ApiService.Registration(EntName.Text,EntEmail.Text,EntPhone.Text,EntPassword.Text);
		if (response)
		{
			await DisplayAlert("", "Your Account has been Created", "Alright");
			await Navigation.PushAsync(new LoginPage());
		}
		else {
            await DisplayAlert("", "Oops somting went wrong", "Cancel");
        }

    }

    private async void TapLogin_Tapped(object sender, TappedEventArgs e)
    {
		await Navigation.PushAsync(new LoginPage());
    }
}