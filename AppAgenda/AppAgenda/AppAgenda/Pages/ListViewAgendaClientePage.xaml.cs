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
    public partial class ListViewAgendaClientePage : ContentPage
    {
        public ObservableCollection<Agenda> Items { get; set; }
        public ObservableCollection<Meses> Meses { get; set; }

        public ListViewAgendaClientePage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //your code here;
            if (!App.IsUserLoggedIn)
            {
                await Navigation.PushAsync(new LoginPage());
            }

            await ListaMeses();
            //await this.ListaAgenda(Meses[pkMes.SelectedIndex].mes, Meses[pkMes.SelectedIndex].ano);
        }

        async Task ListaMeses()
        {
            var result = await ApiAgendaHttpClient.Current.agendaMeses(App.User.id_pessoa);
            Meses = new ObservableCollection<Meses>(result);
            pkMes.ItemsSource = Meses;
            if (Meses.Count - 1 >= 0)
                pkMes.SelectedIndex = Meses.Count - 1;
        }

        async Task ListaAgenda(int _mes, int _ano)
        {
            try
            {
                var result = await ApiAgendaHttpClient.Current.BuscarAgendaCliente(App.User.id_pessoa, _mes, _ano);
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

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private async void ListView_Refreshing(object sender, EventArgs e)
        {
            await this.ListaAgenda(Meses[pkMes.SelectedIndex].mes, Meses[pkMes.SelectedIndex].ano);
        }

        private async void btCancelar_Clicked(object sender, EventArgs e)
        {
            var cancel = await DisplayAlert("Cancelar", "Cancelar o agendamento?", "Sim", "Não");
            if (cancel)
            {
                var button = ((Button)sender);
                var result = await ApiAgendaHttpClient.Current.CancelarAgendamento(button.ClassId);
                await this.ListaAgenda(Meses[pkMes.SelectedIndex].mes, Meses[pkMes.SelectedIndex].ano);
            }
        }

        private async void pkMes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(pkMes.SelectedIndex >= 0)
                await this.ListaAgenda(Meses[pkMes.SelectedIndex].mes, Meses[pkMes.SelectedIndex].ano);
        }
    }
}
