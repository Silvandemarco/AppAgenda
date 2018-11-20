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
        public ObservableCollection<Pessoa> Items { get; set; }

        public ListViewProfissionaisPage()
        {
            InitializeComponent();

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //your code here;
            await this.ListaProfissionais();

            if (!App.IsUserLoggedIn)
                tbLogin.Text = "Entrar";
            else
                tbLogin.Text = "Sair";

            if (App.IsUserLoggedIn) { 
                if (App.User.tipo == "P") 
                    tbProfissionais.Text = "Agenda profissional";
                else
                    tbProfissionais.Text = "Sou profissional";
            }
            else
            {
                tbProfissionais.Text = "Sou profissional";
            }

        }
        
        async void Login_Clicked(object sender, ClickedEventArgs e)
        {
            if (!App.IsUserLoggedIn)
            {
                await Navigation.PushAsync(new LoginPage());
            }
            else
            {
                App.IsUserLoggedIn = false;
                tbLogin.Text = "Entrar";
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //your code here;
        }

        private async void ListView_Refreshing(object sender, EventArgs e)
        {
            await this.ListaProfissionais();
        }

        async Task ListaProfissionais()
        {
            try
            {
                var result = await ApiAgendaHttpClient.Current.BuscarProf("P");
                Items = new ObservableCollection<Pessoa>(result);
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
            var prof = e.Item as Pessoa;
            //string i = prof.nome;
            //await DisplayAlert("Item Tapped", prof.nome, "OK");
            //await App.Current.MainPage.Navigation.PushAsync(new ListaProfissionaisPage());
            
            var servicos = new ListViewServicosPage(prof);
            //servicos.BindingContext = prof;
            await Navigation.PushAsync(servicos);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        async private void tbProfissionais_Clicked(object sender, EventArgs e)
        {
            if (App.IsUserLoggedIn)
            {
                if (App.User.tipo == "P")
                    await Navigation.PushAsync(new ProfissionalTabbedPage());
                else
                    await Navigation.PushAsync(new LoginPage());
            }
            else
            {
                await Navigation.PushAsync(new LoginPage());
            }
        }

        private void sbPesquisar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var texto = sbPesquisar.Text;
            ListView.ItemsSource = Items.Where( x => x.nomeCompleto.ToLower().Contains(texto.ToLower()));
        }

        private void tbSearch_Clicked(object sender, EventArgs e)
        {
            sbPesquisar.IsVisible = true;
            sbPesquisar.Focus();
        }

        private void sbPesquisar_Unfocused(object sender, FocusEventArgs e)
        {
            sbPesquisar.IsVisible = false;
        }

        private async void tbAjustes_Clicked(object sender, EventArgs e)
        {
            if (!App.IsUserLoggedIn)
            {
                await Navigation.PushAsync(new LoginPage(new AjustesPage()));
            }
            else
            {
                await Navigation.PushAsync(new AjustesPage());
            }
        }
    }
}
