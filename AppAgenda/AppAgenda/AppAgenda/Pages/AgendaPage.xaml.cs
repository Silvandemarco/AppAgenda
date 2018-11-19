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
	public partial class AgendaPage : ContentPage
	{
		public AgendaPage ()
		{
			InitializeComponent ();
		}
        private Agenda Agenda { get; set; }
        public AgendaPage(Agenda agenda)
        {
            InitializeComponent();
            Agenda = agenda;
            lServico.Text = Agenda.servicos.descricao;
            lCliente.Text = Agenda.cliente.nomeCompleto;
            lData.Text = Agenda.datetime.ToString("dd/MM/yyyy");
            lHora.Text = Agenda.datetime.ToString("hh:mm");
            lDuracao.Text = String.Format("{0} minutos", Agenda.servicos.duracao);
            lValor.Text = String.Format("{0:C}", Agenda.servicos.valor);
            btCancelar.ClassId = Agenda.status;
            btFinalizar.ClassId = Agenda.status;
            if (Agenda.status == "C")
                lStatus.Text = "Cancelado";
            if (Agenda.status == "F")
                lStatus.Text = "Finalizado";
        }

        private async void btFinalizar_Clicked(object sender, EventArgs e)
        {
            var delete = await DisplayAlert("Finalizar", "Deseja finalizar esse agendamento?", "Sim", "Não");
            if (delete)
            {
                try
                {
                    var result = await ApiAgendaHttpClient.Current.CancelarAgendamento(Convert.ToString(Agenda.id_agenda), "F");
                    await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Ops", ex.Message, "Ok");
                }
            }
        }

        private async void btCancelar_Clicked(object sender, EventArgs e)
        {
            var delete = await DisplayAlert("Cancelar", "Deseja cancelar esse agendamento?", "Sim", "Não");
            if (delete)
            {
                try
                {
                    var result = await ApiAgendaHttpClient.Current.CancelarAgendamento(Convert.ToString(Agenda.id_agenda), "C");
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