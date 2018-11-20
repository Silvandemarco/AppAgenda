using AppAgenda.Clients;
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
	public partial class AlterarSenhaPage : ContentPage
	{
        public User User { get; set; }
		public AlterarSenhaPage ()
		{
			InitializeComponent ();
            User = new User();
            User.email = App.User.email;
		}


        private void etSenhaAntiga_Focused(object sender, FocusEventArgs e)
        {
            lErroSenha.IsVisible = false;
        }

        private async void etSenhaAntiga_Completed(object sender, EventArgs e)
        {
            
        }

        private void etNovaSenha_Focused(object sender, FocusEventArgs e)
        {
            lErroNovaSenha.IsVisible = false;
        }

        private void etConfirmaSenha_Focused(object sender, FocusEventArgs e)
        {
            lErroConfirmaSenha.IsVisible = false;
        }

        private void etNovaSenha_Completed(object sender, EventArgs e)
        {
            etConfirmaSenha.Focus();
        }

        private void etConfirmaSenha_Completed(object sender, EventArgs e)
        {
            
        }

        private async void btSalvar_Clicked(object sender, EventArgs e)
        {
            if (etSenhaAntiga.Text == null || etSenhaAntiga.Text == "" || etNovaSenha.Text == null || etNovaSenha.Text == "" || etConfirmaSenha.Text == null || etConfirmaSenha.Text == "")
            {
                if (etSenhaAntiga.Text == null || etSenhaAntiga.Text == "")
                    lErroSenha.IsVisible = true;
                if (etNovaSenha.Text == null || etNovaSenha.Text == "")
                    lErroNovaSenha.IsVisible = true;
                if (etConfirmaSenha.Text == null || etConfirmaSenha.Text == "")
                    lErroConfirmaSenha.IsVisible = true;
                if (etNovaSenha.Text != etConfirmaSenha.Text)
                {
                    etConfirmaSenha.Text = "";
                    etConfirmaSenha.Focus();
                    DependencyService.Get<IMessage>().LongAlert("Senha incorreta.");
                }


            }
            else
            {
                User.senha = etNovaSenha.Text;
                await AlterarSenha();
            }
        }

        async Task AlterarSenha()
        {
            try
            {
                var result = await ApiAgendaHttpClient.Current.AlteraSenha(User);
                Resposta resposta = result;
                DependencyService.Get<IMessage>().LongAlert(resposta.msg);
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ah não", ex.Message, "Ok");
            }
        }

        private async void etSenhaAntiga_Unfocused(object sender, FocusEventArgs e)
        {
            try
            {
                User.senha = etSenhaAntiga.Text;
                var isValid = await ApiAgendaHttpClient.Current.validaLogin(User);
                if (isValid)
                {
                    etNovaSenha.IsEnabled = true;
                    etConfirmaSenha.IsEnabled = true;
                    etNovaSenha.Focus();
                }
                else
                {
                    etSenhaAntiga.Text = "";
                    etSenhaAntiga.Focus();
                    DependencyService.Get<IMessage>().LongAlert("Senha incorreta.");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ah não", ex.Message, "Ok");
            }
        }

        private void etConfirmaSenha_Unfocused(object sender, FocusEventArgs e)
        {
            if (etNovaSenha.Text != etConfirmaSenha.Text)
            {
                etConfirmaSenha.Text = "";
                etConfirmaSenha.Focus();
                DependencyService.Get<IMessage>().LongAlert("Senha incorreta.");
            }
            else
            {
                btSalvar.Focus();
            }
        }
    }
}