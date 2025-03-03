﻿using AppAgenda.Clients;
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
    public partial class Profissionais : ContentPage
    {
        //public ObservableCollection<string> Items { get; set; }
        public List<Profissionaiss> Items { get; set; }

        public Profissionais()
        {
            InitializeComponent();
            Lista();
            //ListView.ItemsSource = Items;

   //         Items = new ObservableCollection<string>
   //         {
   //             "Item 1",
   //             "Item 2",
   //             "Item 3",
   //             "Item 4",
   //             "Item 5"
   //         };

            //MyListView.ItemsSource = Items;
        }
        async Task Lista()
        {
            try
            {
                var result = await ApiAgendaHttpClient.Current.BuscarProfissionais("10");
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
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
