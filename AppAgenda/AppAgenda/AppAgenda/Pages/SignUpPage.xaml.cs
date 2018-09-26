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
	public partial class SignUpPage : ContentPage
	{
        public Pessoa Pessoa { get; set; }
        public SignUpPage ()
		{
			InitializeComponent ();
            Pessoa = new Pessoa();
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //your code here;
            etNascimento.Date = new DateTime(2000,01,01);
            etNascimento.MinimumDate = new DateTime(1900, 01, 01);
            etNascimento.MaximumDate = DateTime.Now;
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            var result = await ApiAgendaHttpClient.Current.EmailUnico(etEmail.Text);
            if (etEmail.Text == null || etEmail.Text == "" || etName.Text == null || etName.Text == "" || etSobrenome.Text == null || etSobrenome.Text == ""
                 || etSenha.Text == null || etSenha.Text == "" || etConfirmaSenha.Text == null || etConfirmaSenha.Text == "" || !result)
            {
                if (etEmail.Text == null || etEmail.Text == "")
                    lErroEmail.IsVisible = true;
                else
                {
                    if (!result)
                    {
                        lErroEmail.Text = "Email incorreta ou já cadastrado.";
                        lErroEmail.IsVisible = true;
                        etEmail.Focus();
                        DependencyService.Get<IMessage>().LongAlert("Email incorreta ou já cadastrado.");
                    }
                }
                if (etName.Text == null || etName.Text == "")
                    lErroNome.IsVisible = true;
                if (etSobrenome.Text == null || etSobrenome.Text == "")
                    lErroSobrenome.IsVisible = true;
                if (etSenha.Text == null || etSenha.Text == "")
                    lErroSenha.IsVisible = true;
                if (etConfirmaSenha.Text == null || etConfirmaSenha.Text == "")
                    lErroConfirmaSenha.IsVisible = true;
                if (etSenha.Text != etConfirmaSenha.Text)
                {
                    etConfirmaSenha.Text = "";
                    etConfirmaSenha.Focus();
                    DependencyService.Get<IMessage>().LongAlert("Senha incorreta.");
                }
                

            }
            else
            {
                Pessoa.nome = etName.Text.Trim();
                Pessoa.sobrenome = etSobrenome.Text.Trim();
                Pessoa.nascimento = etNascimento.Date.ToString("yyyy-MM-dd");
                Pessoa.telefone = etTelefone.Text.Trim();
                Pessoa.email = etEmail.Text.Trim();
                Pessoa.senha = etSenha.Text;

                await Navigation.PushAsync(new SignUpAddressPage(Pessoa));
            }
                
        }

        private void etName_Focused(object sender, FocusEventArgs e)
        {
            lErroNome.IsVisible = false;
        }

        private void etSobrenome_Focused(object sender, FocusEventArgs e)
        {
            lErroSobrenome.IsVisible = false;
        }

        private void etNascimento_Focused(object sender, FocusEventArgs e)
        {
            lErroNascimento.IsVisible = false;
        }

        private void etEmail_Focused(object sender, FocusEventArgs e)
        {
            lErroEmail.IsVisible = false;
        }

        private void etSenha_Focused(object sender, FocusEventArgs e)
        {
            lErroSenha.IsVisible = false;
        }

        private void etConfirmaSenha_Focused(object sender, FocusEventArgs e)
        {
            lErroConfirmaSenha.IsVisible = false;
        }

        private void etName_Completed(object sender, EventArgs e)
        {
            etSobrenome.Focus();
        }

        private void etSobrenome_Completed(object sender, EventArgs e)
        {
            etNascimento.Focus();
        }

        private void OnDateSelected(object sender, EventArgs e)
        {
            etTelefone.Focus();
        }

        private void etTelefone_Completed(object sender, EventArgs e)
        {
            etEmail.Focus();
        }

        async private void etEmail_Completed(object sender, EventArgs e)
        {
            var result = await ApiAgendaHttpClient.Current.EmailUnico(etEmail.Text);
            if (result)
            {
                etSenha.Focus();
            }
            else
            {
                etEmail.Focus();
                DependencyService.Get<IMessage>().LongAlert("Email incorreta ou já cadastrado.");
            }
        }

        private void etSenha_Completed(object sender, EventArgs e)
        {
            etConfirmaSenha.Focus();
        }

        private void etConfirmaSenha_Completed(object sender, EventArgs e)
        {
            if (etSenha.Text != etConfirmaSenha.Text)
            {
                etConfirmaSenha.Text = "";
                etConfirmaSenha.Focus();
                DependencyService.Get<IMessage>().LongAlert("Senha incorreta.");
            }
            else
            {
                btContinuar.Focus();
            }
        }

    }
}