using AppAgenda.Clients;
using AppAgenda.ViewModels;
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
    public partial class ListViewProfissionaisPage : ContentPage
    {
        public List<Profissionais> Items { get; set; }

        public ListViewProfissionaisPage()
        {
            InitializeComponent();
            BindingContext = new ListViewProfissionaisViewModel();

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //your code here;
            await this.ListaProfissionais();
        }
        
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //your code here;
        }

        async Task ListaProfissionais()
        {
            try
            {
                var result = await ApiAgendaHttpClient.Current.BuscarProfissionais("100");
                Items = result.profissionais;
                ListView.ItemsSource = Items;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ah não", ex.Message, "Ok");
            }
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var prof = e.Item as Profissionais;
            //string i = prof.nome;
            //await DisplayAlert("Item Tapped", prof.nome, "OK");
            //await App.Current.MainPage.Navigation.PushAsync(new ListaProfissionaisPage());
            
            var servicos = new ListViewServicosPage(prof);
            //servicos.BindingContext = prof;
            await Navigation.PushAsync(servicos);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
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
