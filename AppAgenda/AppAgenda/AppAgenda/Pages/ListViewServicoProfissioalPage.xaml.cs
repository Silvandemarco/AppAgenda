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
    public partial class ListViewServicoProfissioalPage : ContentPage
    {
        public ObservableCollection<Servicos> Items { get; set; }
        public Pessoa Prof { get; set; }

        public ListViewServicoProfissioalPage()
        {
            InitializeComponent();
            this.Prof = App.User;

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
                await App.Current.MainPage.DisplayAlert("Ah não", ex.Message, "Ok");
                ListView.IsRefreshing = false;
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
            }
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var servico = e.Item as Servicos;
            var alterarServico = new ServicoAddPage(servico);
            await Navigation.PushAsync(alterarServico);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private async void ListView_Refreshing(object sender, EventArgs e)
        {
            await this.ListaServicos();
        }

        private async void tbNovo_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ServicoAddPage());
        }
    }
}
