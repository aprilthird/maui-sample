namespace AppMAUISample;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new Pages.AppShell();
	}
}
