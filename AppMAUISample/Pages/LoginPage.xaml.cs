using AppMAUISample.ViewModels;

namespace AppMAUISample.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        txtEmail.Text = string.Empty;
        txtPassword.Text = string.Empty;
    }
}