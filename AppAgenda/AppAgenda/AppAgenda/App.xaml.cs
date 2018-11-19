using AppAgenda.Clients;
using AppAgenda.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace AppAgenda
{
	public partial class App : Application
	{
        public static bool IsUserLoggedIn { get; set; }
        public static Pessoa User { get; set; }
        public App ()
		{
			InitializeComponent();
            if (IsUserLoggedIn == true)
            {
                if (User.tipo != "P")
                    MainPage = new NavigationPage(new ClienteTabbedPage());
                else
                    MainPage = new NavigationPage(new ProfissionalTabbedPage());
            }
            else
            {
                MainPage = new NavigationPage(new ClienteTabbedPage());
            }
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
