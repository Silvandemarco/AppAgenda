using AppAgenda.Clients;
using AppAgenda.ViewModels;
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
	public partial class ConfirmaAgendamentoPage : ContentPage
	{
        public string Hora { get; set; }
        public string Data { get; set; }
        public int Profissional { get; set; }
        public int IdProf_serv { get; set; }
        public Resposta Resposta { get; set; }
        public DateTime DateTime { get; set; }
        public Servicos Servicos { get; set; }

        public ConfirmaAgendamentoPage (Servicos servico, DateTime dateTime, string hora)
        {
            
            InitializeComponent ();
            BindingContext = new ConfirmaAgendamentoViewModel();
            this.DateTime = dateTime;
            this.Hora = hora;
            this.Profissional = Convert.ToInt32(servico.id_profissional);
            this.IdProf_serv = servico.id_servico;
            this.Data = dateTime.ToString("dd/MM/yyyy");
            this.Servicos = servico;
            lServico.Text = Servicos.descricao;
            lDescricao.Text = Servicos.descricao;
            lData.Text = String.Format("Data {0} hora {1}", Data, this.Hora.Substring(0,5));
            lDuracao.Text = String.Format("{0} minutos", servico.duracao);
            lValor.Text = String.Format("{0:C}", servico.valor);
            

        }
        async void OnButtonClicked(object sender, EventArgs args)
        {
            Agenda agenda = new Agenda();
            agenda.id_cliente = App.User.id_pessoa;
            agenda.id_profissional = this.Profissional;
            agenda.servico = new List<Servicos>();
            Servicos prof_serv = new Servicos();
            prof_serv.id_servico = this.IdProf_serv;
            agenda.servico.Add(prof_serv);
            agenda.datetime = new DateTime(this.DateTime.Year, this.DateTime.Month, this.DateTime.Day, Convert.ToInt32(Hora.Substring(0, 2)), Convert.ToInt32(Hora.Substring(3, 2)), Convert.ToInt32(Hora.Substring(6, 2)));
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