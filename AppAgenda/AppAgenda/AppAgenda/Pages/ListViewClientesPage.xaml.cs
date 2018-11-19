using AppAgenda.Clients;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAgenda.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewClientesPage : ContentPage
    {
        public ObservableCollection<Cliente> Items { get; set; }

        public ListViewClientesPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //your code here;
            await this.ListaClientes();
        }

        async Task ListaClientes()
        {
            try
            {
                var result = await ApiAgendaHttpClient.Current.BuscarClientes(App.User.id_pessoa);
                Items = new ObservableCollection<Cliente>(result);
                ListView.ItemsSource = Items;
                ListView.IsRefreshing = false;
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
                if (Items.Count == 0)
                {
                    slNotFound.IsVisible = true;
                    ListView.IsVisible = false;
                }
                else
                {
                    slNotFound.IsVisible = false;
                    ListView.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ops", ex.Message, "Ok");
            }
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var cliente = e.Item as Cliente;
            var agendamentos = new ListViewAgendCliProfPage(cliente);
            await Navigation.PushAsync(agendamentos);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private async void ListView_Refreshing(object sender, EventArgs e)
        {
            await this.ListaClientes();
        }
    }
}
