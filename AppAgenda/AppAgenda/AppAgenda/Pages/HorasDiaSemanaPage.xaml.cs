using AppAgenda.Clients;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAgenda.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HorasDiaSemanaPage : ContentPage
	{
        public ObservableCollection<DiaSemana> Items { get; set; }
        public HorasDiaSemanaPage ()
		{
			InitializeComponent ();
            Items = new ObservableCollection<DiaSemana>();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //your code here;
            await this.ListaHoras();
        }

        async Task ListaHoras()
        {
            try
            {
                var result = await ApiAgendaHttpClient.Current.BuscarDiasSemana(App.User.id_pessoa);
                Items = new ObservableCollection<DiaSemana>(result);
                scDomingo.On = Items[0].status;
                scSegundao.On = Items[1].status;
                scTerca.On = Items[2].status;
                scQuarta.On = Items[3].status;
                scQuinta.On = Items[4].status;
                scSexta.On = Items[5].status;
                scSabado.On = Items[6].status;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ah não", ex.Message, "Ok");
            }
        }

        async private void SwitchCell_Tapped(object sender, EventArgs e)
        {
            var switchCell = ((SwitchCell)sender);
            await Navigation.PushAsync(new HorasDiaPage(switchCell.ClassId, switchCell.Text));
        }

        private async void switchCell_OnChanged(object sender, ToggledEventArgs e)
        {
            var switchCell = ((SwitchCell)sender);
            if (!switchCell.On)
            {
                var delete = await ApiAgendaHttpClient.Current.DeletarHorasDia(new HorasDia(App.User.id_pessoa, switchCell.ClassId, new TimeSpan(), new TimeSpan()));
                await this.ListaHoras();
            }
            else
            {
                var result = await ApiAgendaHttpClient.Current.BuscarHorasDia(App.User.id_pessoa, switchCell.ClassId);
                if (result.Count == 0)
                    await Navigation.PushAsync(new HorasDiaPage(switchCell.ClassId, switchCell.Text));
            }
            
        }
    }
}