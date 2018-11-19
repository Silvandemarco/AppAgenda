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
        public List<Row> Items { get; set; }
        public Servicos Servico { get; set; }
        public string DateFormat { get; set; }

        public ListViewAgendamentoPage(Servicos servico)
        {
            InitializeComponent();
            //BindingContext = new ListViewAgendamentoViewModel();
            this.Servico = servico;
            startDatePicker.MinimumDate = DateTime.Now;
            page.Title = Servico.descricao;
            DateFormat = "dddd, d";
            tbDate.Text = startDatePicker.Date.ToString(DateFormat);
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
                var result = await ApiAgendaHttpClient.Current.BuscarHorasLivres(Servico.id_profissional, Servico.id_servico, startDatePicker.Date);

                List<Row> items = new List<Row>();
                int cont = 0;
                while(cont < result.Count())
                {
                    Row row = new Row();
                    row.Column1 = TimeSpan.Parse(result[cont]);
                    cont++;
                    if(cont >= result.Count()) { items.Add(row); break; }
                    row.Column2 = TimeSpan.Parse(result[cont]);
                    cont++;
                    if (cont >= result.Count()) { items.Add(row); break; }
                    row.Column3 = TimeSpan.Parse(result[cont]);
                    cont++;
                    if (cont >= result.Count()) { items.Add(row); break; }
                    items.Add(row);
                }
                Items = items;
                ListView.ItemsSource = Items;
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
                ListView.IsRefreshing = false;
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ah não", ex.Message, "Ok");
            }
        }

        async void OnDateSelected(object sender, DateChangedEventArgs args)
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
            await ListaHorasLivres();
        }

        private async void ListView_Refreshing(object sender, EventArgs e)
        {
            await this.ListaHorasLivres();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var hora = ((Button)sender).Text;
            if (hora != "00:00")
            {
                var agendamento = new ConfirmaAgendamentoPage(Servico, startDatePicker.Date, hora);
                if (App.IsUserLoggedIn)
                {
                    await Navigation.PushAsync(agendamento);
                }
                else
                {
                    await Navigation.PushAsync(new LoginPage(agendamento));
                }
            }
        }

        private void tbDate_Clicked(object sender, EventArgs e)
        {
            startDatePicker.Focus();
        }
    }
    public class Row
    {
        public TimeSpan Column1 { get; set; }
        public TimeSpan Column2 { get; set; }
        public TimeSpan Column3 { get; set; }
    }
}
