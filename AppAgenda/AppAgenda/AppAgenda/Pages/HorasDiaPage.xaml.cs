using AppAgenda.Clients;
using AppAgenda.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAgenda.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HorasDiaPage : ContentPage
	{
        public ObservableCollection<HorasDia> Items { get; set; }
        public string strDia;
        public HorasDiaPage ()
		{
			InitializeComponent ();
            Items = new ObservableCollection<HorasDia>();
            this.strDia = "";
        }
        public HorasDiaPage(string _dia)
        {
            InitializeComponent();
            Items = new ObservableCollection<HorasDia>();
            this.strDia = _dia;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //your code here;
            await this.ListaHoras();
        }

        private void btAdd_Clicked(object sender, EventArgs e)
        {
            Items.Add(new HorasDia(1, strDia, new TimeSpan(), new TimeSpan()));
        }

        async Task ListaHoras()
        {
            try
            {
                var result = await ApiAgendaHttpClient.Current.BuscarHorasDia(1, strDia);
                Items = new ObservableCollection<HorasDia>(result);
                if(Items.Count == 0)
                {
                    Items.Add(new HorasDia(1, strDia, new TimeSpan(), new TimeSpan()));
                }
                ListView.ItemsSource = Items;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ah não", ex.Message, "Ok");
            }
        }

        async Task GravarHoras()
        {
            try
            {
                var delete = await ApiAgendaHttpClient.Current.DeletarHorasDia(new HorasDia(1, strDia, new TimeSpan(), new TimeSpan()));
                if(Items.Count > 0)
                {
                    var result = await ApiAgendaHttpClient.Current.InserirHorasDia(new List<HorasDia>(Items));
                    DependencyService.Get<IMessage>().LongAlert(result.msg);
                    //await App.Current.MainPage.DisplayAlert("", result.msg, "Ok");
                }
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ah não", ex.Message, "Ok");
            }
        }

        private void MenuItem_Clicked(object sender, EventArgs e)
        {
            

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var button = ((Button)sender);
            for(int i = 0; i<Items.Count; i++)
            {
                if (button.CommandParameter == Items[i])
                    Items.Remove(Items[i]);
            }
        }

        private async void tbSalvar_Clicked(object sender, EventArgs e)
        {
            await GravarHoras();
        }
    }
}