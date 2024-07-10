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
		}
		else {
            await DisplayAlert("", "Oops somting went wrong", "Cancel");
        }

    }
}