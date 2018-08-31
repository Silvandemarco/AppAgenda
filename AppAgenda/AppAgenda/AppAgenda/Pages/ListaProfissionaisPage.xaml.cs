using AppAgenda.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAgenda.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListaProfissionaisPage : ContentPage
	{
		public ListaProfissionaisPage ()
		{
			InitializeComponent ();

            BindingContext = new ListaProfissionaisViewModel();

        }
        

    }
}