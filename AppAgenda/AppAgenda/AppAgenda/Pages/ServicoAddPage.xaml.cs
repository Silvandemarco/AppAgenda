using AppAgenda.Clients;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAgenda.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ServicoAddPage : ContentPage
	{
		public ServicoAddPage ()
		{
			InitializeComponent ();
            etValor.Text = "0";
            Edit = false;
            tbDelete.Icon = "";
        }
        private Servicos Servico { get; set; }
        private bool Edit { get; set; }
        public ServicoAddPage(Servicos servico)
        {
            InitializeComponent();
            Servico = servico;
            Edit = true;
            etDescricao.Text = Servico.descricao;
            etValor.Text = string.Format("{0:n2}", Servico.valor).Replace(",","");
            etDuracao.Text = Convert.ToString(Servico.duracao);
            page.Title = Servico.descricao;
        }

        private void etDescricao_Focused(object sender, FocusEventArgs e)
        {
            lErroDescricao.IsVisible = false;
        }

        private void etDescricao_Completed(object sender, EventArgs e)
        {
            etValor.Focus();
        }

        private void etValor_Focused(object sender, FocusEventArgs e)
        {
            lErroValor.IsVisible = false;
        }

        private void etValor_Completed(object sender, EventArgs e)
        {
            etDuracao.Focus();
        }

        private void etDuracao_Focused(object sender, FocusEventArgs e)
        {
            lErroDuracao.IsVisible = false;
        }

        private void etDuracao_Completed(object sender, EventArgs e)
        {

        }

        private async void btSalvar_Clicked(object sender, EventArgs e)
        {
            if (etDescricao.Text == null || etDescricao.Text == "" || etValor.Text == null || etValor.Text == "" || etDuracao.Text == null || etDuracao.Text == "")
            {
                if (etDescricao.Text == null || etDescricao.Text == "")
                    lErroDescricao.IsVisible = true;
                if (etValor.Text == null || etValor.Text == "")
                    lErroValor.IsVisible = true;
                if (etDuracao.Text == null || etDuracao.Text == "")
                    lErroDuracao.IsVisible = true;
            }
            else
            {
                if(!Edit)
                    await CadastrarServico();
                else
                    await AlterarServico();

            }
        }

        async Task CadastrarServico()
        {
            try
            {
                Servicos servicos = new Servicos();
                servicos.id_profissional = App.User.id_pessoa;
                servicos.descricao = etDescricao.Text;
                servicos.valor = ConverterReaisParaDecimal(etValor.Text);
                servicos.duracao = Convert.ToInt32(etDuracao.Text);

                var result = await ApiAgendaHttpClient.Current.InserirServico(servicos);
                Resposta resposta = result;
                DependencyService.Get<IMessage>().LongAlert("Cadastro efetuado com sucesso.");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ah não", ex.Message, "Ok");
            }
        }

        async Task AlterarServico()
        {
            try
            {
                Servicos servicos = new Servicos();
                servicos.id_profissional = Servico.id_profissional;
                servicos.descricao = etDescricao.Text;
                servicos.valor = ConverterReaisParaDecimal(etValor.Text);
                servicos.duracao = Convert.ToInt32(etDuracao.Text);
                servicos.id_servico = Servico.id_servico;

                var result = await ApiAgendaHttpClient.Current.AlterarServico(servicos);
                Resposta resposta = result;
                DependencyService.Get<IMessage>().LongAlert("Alteração efetuado com sucesso.");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ah não", ex.Message, "Ok");
            }
        }

        private static double ConverterReaisParaDecimal(string valor)
        {
            try
            {
                var valorConvertido = Decimal.Parse(valor.Replace("R$ ", "").Replace(".", ""),
                CultureInfo.GetCultureInfo("pt-BR"));

                return (double)valorConvertido;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private async void tbDelete_Clicked(object sender, EventArgs e)
        {
            if (tbDelete.Icon != "") {
                var delete = await DisplayAlert("Excluir", "Deseja excluir esse serviço?", "Sim", "Não");
                if (delete)
                {
                    try
                    {
                        var result = await ApiAgendaHttpClient.Current.DeletarServico(Servico.id_servico);
                        Resposta resposta = result;
                        DependencyService.Get<IMessage>().LongAlert(resposta.msg);
                        await Navigation.PopAsync();
                    }
                    catch (Exception ex)
                    {
                        await App.Current.MainPage.DisplayAlert("Ops", ex.Message, "Ok");
                    }
                }
            }
        }
    }
}