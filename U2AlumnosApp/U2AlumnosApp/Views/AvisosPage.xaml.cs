using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U2AlumnosApp.Models;
using U2AlumnosApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace U2AlumnosApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AvisosPage : ContentPage
    {
        AvisosViewModel Avisos = new AvisosViewModel(App.AvisosPrim.AlumnoIniciado);
        public AvisosPage()
        {
            InitializeComponent();
            BindingContext = Avisos;
            Avisos.Mensaje += Avisos_Mensaje;
            
        }

        private void Avisos_Mensaje(string obj)
        {
            var msj = DependencyService.Get<IMessage>();
            msj.ShowToast(obj);
        }
    }
}