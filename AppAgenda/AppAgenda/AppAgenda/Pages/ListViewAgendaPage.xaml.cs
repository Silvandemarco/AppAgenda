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

        public ListViewAgendaPage()
        {
            InitializeComponent();

            Items = new ObservableCollection<Agenda>();
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
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ah não", ex.Message, "Ok");
            }
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //if (e.Item == null)
            //    return;

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            ////Deselect Item
            //((ListView)sender).SelectedItem = null;
            MyListView.SelectedItem = null;
        }

        private async void startDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            await this.ListaAgenda();
        }
    }
}
