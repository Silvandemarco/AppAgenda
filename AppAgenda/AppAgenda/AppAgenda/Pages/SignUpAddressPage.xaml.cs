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
	public partial class SignUpAddressPage : ContentPage
	{
        public Pessoa Pessoa { get; set; }
        public Resposta Resposta { get; set; }
        public List<Cidade> Items { get; set; }
        public SignUpAddressPage ()
		{
			InitializeComponent ();
		}
        public SignUpAddressPage(Pessoa _pessoa)
        {
            InitializeComponent();
            this.Pessoa = _pessoa;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //your code here;
            await this.ListaCidades();
        }

        async Task ListaCidades()
        {
            try
            {
                var result = await ApiAgendaHttpClient.Current.BuscarCidades();
                Items = result;
                pKCidade.ItemsSource = Items;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ah não", ex.Message, "Ok");
            }
        }

        async Task CadastrarPessoa()
        {
            try
            {
                var result = await ApiAgendaHttpClient.Current.InserirPessoa(Pessoa);
                Resposta = result;
                Pessoa.id_pessoa = Resposta.id;
                //await App.Current.MainPage.DisplayAlert("", Resposta.msg, "Ok");
                DependencyService.Get<IMessage>().LongAlert("Cadastro efetuado com sucesso.");
                App.IsUserLoggedIn = true;
                List<Pessoa> pessoas = await ApiAgendaHttpClient.Current.BuscarPessoa(Pessoa.id_pessoa);
                App.User = pessoas[0];
                //await Navigation.RemovePage();
                await Navigation.PopToRootAsync(true);
                //await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ah não", ex.Message, "Ok");
            }
        }

        async private void btCadastrar_Clicked(object sender, EventArgs e)
        {
            if (etCep.Text == null || etCep.Text == "" || etEndereco.Text == null || etEndereco.Text == "" || etNumero.Text == null || etNumero.Text == ""
                || etBairro.Text == null || etBairro.Text == "" || pKCidade.SelectedItem == null || pKCidade.SelectedItem.ToString() == "")
            {
                if (etCep.Text == null || etCep.Text == "")
                    lErroCep.IsVisible = true;
                if (etEndereco.Text == null || etEndereco.Text == "")
                    lErroEndereco.IsVisible = true;
                if (etNumero.Text == null || etNumero.Text == "")
                    lErroNumero.IsVisible = true;
                if (etBairro.Text == null || etBairro.Text == "")
                    lErroBairro.IsVisible = true;
                if (pKCidade.SelectedItem == null || pKCidade.SelectedItem.ToString() == "")
                    lErroCidade.IsVisible = true;
            }
            else
            {
                Pessoa.cep = etCep.Text.Trim().Replace("-","");
                Pessoa.endereco = etEndereco.Text.Trim();
                Pessoa.numero = Convert.ToInt32(etNumero.Text.Trim());
                Pessoa.complemento = etComplemento.Text.Trim();
                Pessoa.bairro = etBairro.Text.Trim();
                Pessoa.id_cidade = Items[pKCidade.SelectedIndex].id_cidade;
                //Pessoa.tipo = "C";
                Pessoa.cidade = Items[pKCidade.SelectedIndex];

                await CadastrarPessoa();
            }
        }

        private void etCep_Focused(object sender, FocusEventArgs e)
        {
            lErroCep.IsVisible = false;
        }

        private void etEndereco_Focused(object sender, FocusEventArgs e)
        {
            lErroEndereco.IsVisible = false;
        }

        private void etNumero_Focused(object sender, FocusEventArgs e)
        {
            lErroNumero.IsVisible = false;
        }

        private void etBairro_Focused(object sender, FocusEventArgs e)
        {
            lErroBairro.IsVisible = false;
        }

        private void pKCidade_Focused(object sender, FocusEventArgs e)
        {
            lErroCidade.IsVisible = false;
        }

        private void etCep_Completed(object sender, EventArgs e)
        {
            etEndereco.Focus();
        }

        private void etEndereco_Completed(object sender, EventArgs e)
        {
            etNumero.Focus();
        }

        private void etNumero_Completed(object sender, EventArgs e)
        {
            etComplemento.Focus();
        }

        private void etComplemento_Completed(object sender, EventArgs e)
        {
            etBairro.Focus();
        }

        private void etBairro_Completed(object sender, EventArgs e)
        {
            pKCidade.Focus();
        }

        private void pKCidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            btCadastrar.Focus();
        }
    }
}