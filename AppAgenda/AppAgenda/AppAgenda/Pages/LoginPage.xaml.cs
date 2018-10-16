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
	public partial class LoginPage : ContentPage
	{
        public Object PageDestino { get; set; }

        public LoginPage (Object pageDestino)
		{
			InitializeComponent ();
            this.PageDestino = pageDestino;
		}

        public LoginPage()
        {
            InitializeComponent();
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            if (emailEntry.Text != null && passwordEntry.Text != null)
            {
                var user = new User
                {
                    email = emailEntry.Text.Trim(),
                    senha = passwordEntry.Text.Trim()
                };
                try
                {
                    var isValid = await ApiAgendaHttpClient.Current.validaLogin(user);
                    if (isValid)
                    {
                        App.IsUserLoggedIn = true;
                        List<Pessoa> pessoas = await ApiAgendaHttpClient.Current.BuscarPessoa(user.email);
                        App.User = pessoas[0];
                        if (PageDestino != null)
                        {
                            Navigation.InsertPageBefore(PageDestino as Page, this);
                        }
                        DependencyService.Get<IMessage>().LongAlert("Login efetuado com sucesso.");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        messageLabel.Text = "Login failed";
                        passwordEntry.Text = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Ah não", ex.Message, "Ok");
                }
            }
            else
            {
                if (emailEntry.Text == null || emailEntry.Text == "")
                    lErroEmail.IsVisible = true;
                if (passwordEntry.Text == null || passwordEntry.Text == "")
                    lErroPassword.IsVisible = true;
            }
        }

        private void emailEntry_Focused(object sender, FocusEventArgs e)
        {
            lErroEmail.IsVisible = false;
            lEmail.IsVisible = true;
            emailEntry.PlaceholderColor = Color.Transparent;

        }

        private void emailEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (emailEntry.Text == null || emailEntry.Text == "")
            {
                lEmail.IsVisible = false;
            }
            emailEntry.PlaceholderColor = Color.Default;
        }

        private void passwordEntry_Focused(object sender, FocusEventArgs e)
        {
            lErroPassword.IsVisible = false;
            lSenha.IsVisible = true;
            passwordEntry.PlaceholderColor = Color.Transparent;
        }

        private void passwordEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (passwordEntry.Text == null || passwordEntry.Text == "")
            {
                lSenha.IsVisible = false;  
            }
            passwordEntry.PlaceholderColor = Color.Default;
        }

        private void emailEntry_Completed(object sender, EventArgs e)
        {
            passwordEntry.Focus();
        }

        private void passwordEntry_Completed(object sender, EventArgs e)
        {

        }
    }
}