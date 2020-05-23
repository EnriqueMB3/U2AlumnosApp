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
        public Command ShowCommand { get; set; }



        private string clave;

        public string Clave
        {
            get
            {
                return clave;
            }
            set
            {
                clave = value; Actualizar();

            }
        }

        private string password;

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value; Actualizar();

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

        private bool show = true;
        public bool Show
        {
            get { return show; }
            set { show = value; Actualizar(); }
        }

        private double opacity;
        public double Opacity
        {
            get { return opacity; }
            set { opacity = value; Actualizar(); }
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
            ShowCommand = new Command(ShowPassword);
            Opacity = 1;
        }
        public event Action<string> Mensaje;

        private void ShowPassword()
        {
            Show = !Show;
        }

        private async void Iniciar()
        {
            try
            {


                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    Error = "No hay conexión a internet.";
                    return;
                }
                Running = true;
                Visible = false;
                Opacity = .2;

                AlumnoIniciado alumnoIniciado = await App.AvisosPrim.IniciarSesionAsync(Clave, Password);

                Running = false;
                Visible = true;
                if (alumnoIniciado != null)
                {
                    App.AvisosPrim.AlumnoIniciado = alumnoIniciado;
                }

                Application.Current.MainPage = new NavigationPage(new MainPage());


            }
            catch (Exception ex)
            {
                Opacity = 1;
                Running = false;
                Visible = true;
                Error = ex.Message;
                Mensaje?.Invoke(ex.Message);

            }
        }
    }
}
