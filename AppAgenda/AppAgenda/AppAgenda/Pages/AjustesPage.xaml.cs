using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAgenda.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AjustesPage : ContentPage
	{
		public AjustesPage ()
		{
			InitializeComponent ();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //your code here;
            if (App.User.tipo != "P")
                tbProfissional.IsVisible = false;
            else
                tbCliente.IsVisible = false;
        }

            private async void tcDados_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage(App.User));
        }

        private async void tcEndereco_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpAddressPage(App.User));
        }

        private async void tcSenha_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AlterarSenhaPage());
        }

        private async void tcSemana_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HorasDiaSemanaPage());
        }
    }
}