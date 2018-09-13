using AppAgenda.Clients;
using AppAgenda.ViewModels;
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
	public partial class ConfirmaAgendamentoPage : ContentPage
	{
        public string Hora { get; set; }
        public string Data { get; set; }
        public string Servico { get; set; }
        public string Valor { get; set; }
        public string Duracao { get; set; }
        public int Profissional { get; set; }
        public string IdServico { get; set; }
        public Resposta Resposta { get; set; }

        public ConfirmaAgendamentoPage (Servico servico, DateTime dateTime, string hora)
        {
            
            InitializeComponent ();
            BindingContext = new ConfirmaAgendamentoViewModel();
            this.Servico = servico.descricao;
            this.Valor = servico.valor;
            this.Duracao = servico.duracao;
            this.Data = dateTime.ToString("yyyy-MM-dd");
            this.Hora = hora;
            this.Profissional = Convert.ToInt32(servico.id_profissional);
            this.IdServico = servico.id_servico;
            lServico.Text = Servico;
            lValor.Text = Valor;
            lDuracao.Text = Duracao;
            lData.Text = Data;
            lHora.Text = Hora;

        }
        async void OnButtonClicked(object sender, EventArgs args)
        {
            Agenda agenda = new Agenda();
            agenda.id_cliente = 1;
            agenda.id_profissional = this.Profissional;
            agenda.servicos = new List<Servico>();
            Servico sServico = new Servico();
            sServico.id_servico = this.IdServico;
            agenda.servicos.Add(sServico);
            agenda.datetime = Data + Hora;

            try
            {
                var result = await ApiAgendaHttpClient.Current.Agendamento(agenda);
                Resposta = result;
                await App.Current.MainPage.DisplayAlert("", Resposta.msg, "Ok");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ah não", ex.Message, "Ok");
            }
        }
    }
}