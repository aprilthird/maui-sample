namespace AppMAUISample.Pages;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
		await Current.GoToAsync("//login");
	}

	private async void OnLogoutClicked(object sender, EventArgs e)
	{
		await Current.GoToAsync("//login");
	}
}	