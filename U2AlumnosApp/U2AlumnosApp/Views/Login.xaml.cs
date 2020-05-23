using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U2AlumnosApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace U2AlumnosApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            InicioSesionViewModel inicioSesionViewModel = (InicioSesionViewModel)BindingContext;
            inicioSesionViewModel.Mensaje += InicioSesionViewModel_Mensaje;
        }

        private void InicioSesionViewModel_Mensaje(string obj)
        {
            var msj = DependencyService.Get<IMessage>();
            msj.ShowToast(obj);
        }
    }
}