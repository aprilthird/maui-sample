using AppMAUISample.ViewModels;

namespace AppMAUISample.Pages;

public partial class ListUsersPage : ContentPage
{
	public ListUsersPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        lstUsers.RefreshCommand.Execute(this);
    }
}