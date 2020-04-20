using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using U2AlumnosApp.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace U2AlumnosApp.ViewModels
{
    public class InicioSesionViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        void Actualizar([CallerMemberName]string nombre = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }

        public Command IniciarCommand { get; set; }

        InicioSesionAlumno inicioSesionAlumno;

        public InicioSesionAlumno InicioSesionAlumno
        {
            get
            {
                return inicioSesionAlumno;
            }
            set
            {
                inicioSesionAlumno = value; Actualizar();

            }
        }


        private bool running;
        public bool Running
        {
            get { return running; }
            set { running = value; Actualizar(); }
        }

        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set { visible = value; Actualizar(); }
        }
        private string error;
        public string Error
        {
            get { return error; }
            set { error = value; Actualizar(); }
        }


        public InicioSesionViewModel()
        {
            IniciarCommand = new Command(Iniciar);
        }

        private async void Iniciar()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Error = "No hay conexión a internet.";
                return;
            }
            try
            {
                Running = true;
                Visible = true;

             
                await App.AvisosPrim.IniciarSesionAsync("0001", "escolares");

                Running = false;
                visible = false;

                Application.Current.MainPage = new NavigationPage(new MainPage());

            }
            catch (Exception ex)
            {

                Error = ex.Message;
            }
        }
    }
}
