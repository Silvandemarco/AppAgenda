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
    public partial class ListViewAgendaPage : ContentPage
    {
        public ObservableCollection<Agenda> Items { get; set; }
        public string DateFormat { get; set; }
        public ListViewAgendaPage()
        {
            InitializeComponent();
            Items = new ObservableCollection<Agenda>();
            DateFormat = "dddd, d";
            tbDate.Text = startDatePicker.Date.ToString(DateFormat);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //your code here;
            await this.ListaAgenda();
        }

        async Task ListaAgenda()
        {
            try
            {
                var result = await ApiAgendaHttpClient.Current.BuscarAgenda(App.User.id_pessoa,this.startDatePicker.Date);
                Items = new ObservableCollection<Agenda>(result);
                MyListView.ItemsSource = Items;
                MyListView.IsRefreshing = false;
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
                if (Items.Count == 0)
                {
                    slNotFound.IsVisible = true;
                    MyListView.IsVisible = false;
                }
                else
                {
                    slNotFound.IsVisible = false;
                    MyListView.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ah não", ex.Message, "Ok");
                MyListView.IsRefreshing = false;
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
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
            MyListView.SelectedItem = null;
        }

        private async void startDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (startDatePicker.Date.Year != DateTime.Now.Year)
            {
                DateFormat = "ddd, d MMM, yyyy";
            }
            else
            {
                if (startDatePicker.Date.Month != DateTime.Now.Month)
                {
                    DateFormat = "ddd, d MMMM";
                }
                else
                {
                    DateFormat = "dddd, d";
                }
            }
            tbDate.Text = startDatePicker.Date.ToString(DateFormat);
            await this.ListaAgenda();
        }

        private void tbDate_Clicked(object sender, EventArgs e)
        {
            startDatePicker.Focus();
        }

        private async void tbAgendar_Clicked(object sender, EventArgs e)
        {
            var servicos = new ListViewServicosPage(App.User);
            //servicos.BindingContext = prof;
            await Navigation.PushAsync(servicos);
        }

        private async void MyListView_Refreshing(object sender, EventArgs e)
        {
            await this.ListaAgenda();
        }
    }
}
