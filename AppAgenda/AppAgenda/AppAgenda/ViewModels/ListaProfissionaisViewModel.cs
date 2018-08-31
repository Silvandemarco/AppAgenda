using AppAgenda.Clients;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppAgenda.ViewModels
{
    class ListaProfissionaisViewModel : ViewModelBase
    {
        public ListaProfissionaisViewModel() : base()
        {
            // Também é válido...
            //_BuscarCommand = new Command(async () => await BuscarCommandExecute());
        }

        private string _ID;
        public string ID
        {
            get => _ID;
            set
            {
                _ID = value;
                OnPropertyChanged();
            }
        }

        private Command _BuscarCommand;
        //public Command BuscarCommand
        //{
        //    get
        //    {
        //        if (_BuscarCommand == null)
        //            _BuscarCommand = new Command(async () => await BuscarCommandExecute());

        //        return _BuscarCommand;
        //    }
        //}
        public Command BuscarCommand => _BuscarCommand ?? (_BuscarCommand = new Command(async () => await BuscarCommandExecute(), () => IsNotBusy));

        async Task BuscarCommandExecute()
        {
            try
            {
                var result = await ApiAgendaHttpClient.Current.BuscarProfissional(_ID);

                if (!string.IsNullOrWhiteSpace(result.profissionais[0].cep))
                    await App.Current.MainPage.DisplayAlert("Eita", result.profissionais[0].nome, "Ok");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ah não", ex.Message, "Ok");
            }
        }

    }
}
