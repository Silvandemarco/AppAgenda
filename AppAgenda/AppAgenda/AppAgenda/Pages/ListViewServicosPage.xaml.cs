using AppAgenda.Clients;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAgenda.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewServicosPage : ContentPage
    {
        //public List<Servico> Items { get; set; }
        public ObservableCollection<Servicos> Items { get; set; }
        public Pessoa Prof { get; set; }

        public ListViewServicosPage(Pessoa prof)
        {
            InitializeComponent();
            this.Prof = prof;

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //your code here;
            await this.ListaServicos();
        }

        async Task ListaServicos()
        {
            try
            {
                var result = await ApiAgendaHttpClient.Current.BuscarServicos(Prof.id_pessoa);
                Items = new ObservableCollection<Servicos>(result);
                ListView.ItemsSource = Items;
                ListView.IsRefreshing = false;
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ah não", ex.Message, "Ok");
            }
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var servico = e.Item as Servicos;
            var agendamento = new ListViewAgendamentoPage(servico);
            await Navigation.PushAsync(agendamento);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private async void ListView_Refreshing(object sender, EventArgs e)
        {
            await this.ListaServicos();
        }
        /*
        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        */
    }
}
