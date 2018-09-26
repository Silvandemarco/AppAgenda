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
    public partial class ListViewAgendamentoPage : ContentPage
    {
        public List<string> Items { get; set; }
        public Servico Servico { get; set; }

        public ListViewAgendamentoPage(Servico servico)
        {
            InitializeComponent();
            BindingContext = new ListViewAgendamentoViewModel();
            this.Servico = servico;
            startDatePicker.MinimumDate = DateTime.Now;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //your code here;
            await this.ListaHorasLivres();
        }

        async Task ListaHorasLivres()
        {
            try
            {
                var result = await ApiAgendaHttpClient.Current.BuscarHorasLivres(Servico.id_profissional, Servico.id_prof_serv, startDatePicker.Date);
                Items = result;
                ListView.ItemsSource = Items;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ah não", ex.Message, "Ok");
            }
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            
            var servico = Servico;
            var hora = e.Item as string;
            var agendamento = new ConfirmaAgendamentoPage(servico, startDatePicker.Date, hora);
            if (App.IsUserLoggedIn)
            {
                await Navigation.PushAsync(agendamento);
            }
            else
            {
                await Navigation.PushAsync(new LoginPage(agendamento));
            }

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        async void OnDateSelected(object sender, DateChangedEventArgs args)
        {
            await ListaHorasLivres();
        }
    }
}
