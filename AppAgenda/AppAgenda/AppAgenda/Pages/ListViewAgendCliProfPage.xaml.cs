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
    public partial class ListViewAgendCliProfPage : ContentPage
    {
        public ObservableCollection<Agenda> Items { get; set; }
        public ObservableCollection<Meses> Meses { get; set; }
        public Cliente Cliente { get; set; }
        public bool Primeiro  { get; set; }

        public ListViewAgendCliProfPage(Cliente cliente)
        {
            InitializeComponent();
            Cliente = cliente;
            tbDate.Text = pkMes.SelectedItem as string;
            Primeiro = false;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //your code here;
            page.Title = Cliente.nomeCompleto;
            if (!App.IsUserLoggedIn)
            {
                await Navigation.PushAsync(new LoginPage());
            }
            else
            {
                if (!Primeiro)
                {
                    await ListaMeses();
                    Primeiro = true;
                }
                    
            }

            //await this.ListaAgenda(Meses[pkMes.SelectedIndex].mes, Meses[pkMes.SelectedIndex].ano);
        }

        async Task ListaMeses()
        {
            var result = await ApiAgendaHttpClient.Current.agendaMeses(Cliente.id_pessoa);
            Meses = new ObservableCollection<Meses>(result);
            pkMes.ItemsSource = Meses;
            if (Meses.Count - 1 >= 0)
                pkMes.SelectedIndex = Meses.Count - 1;
        }

        async Task ListaAgenda(int _mes, int _ano)
        {
            try
            {
                var result = await ApiAgendaHttpClient.Current.BuscarAgendaCliente(Cliente.id_pessoa, _mes, _ano, App.User.id_pessoa);
                Items = new ObservableCollection<Agenda>(result);
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
            if (e.Item == null)
                return;

            var agenda = e.Item as Agenda;
            var agendaPage = new AgendaPage(agenda);
            await Navigation.PushAsync(agendaPage);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private async void ListView_Refreshing(object sender, EventArgs e)
        {
            await this.ListaAgenda(Meses[pkMes.SelectedIndex].mes, Meses[pkMes.SelectedIndex].ano);
        }

        private async void pkMes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pkMes.SelectedIndex >= 0)
            {
                var meses = pkMes.SelectedItem as Meses;
                tbDate.Text = meses.nomeMes;
                await this.ListaAgenda(Meses[pkMes.SelectedIndex].mes, Meses[pkMes.SelectedIndex].ano);
            }
        }

        private void tbDate_Clicked(object sender, EventArgs e)
        {
            pkMes.Focus();
        }
    }
}
