using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using U2AlumnosApp.Models;
using U2AlumnosApp.Views;

using Xamarin.Forms;

namespace U2AlumnosApp.ViewModels
{
   public class MasterDetailViewModel
    {

        public ObservableCollection<Alumno> Alumnos { get; set; }
        public Command<Alumno> AlumnoAvisosCommand { get; set; }
        public Command EliminarAlumnoCommand { get; set; }


        List<Alumno> AlumnosApp;
        public MasterDetailViewModel()
        {
            Alumnos = new ObservableCollection<Alumno>();
            AlumnosApp = App.AvisosPrim.GetAlumnosIniciados();
            AlumnosApp.ForEach(x => Alumnos.Add(x));
            AgregarAlumnoCommand = new Command(AgregarAlumnoIniciado);
            AlumnoAvisosCommand = new Command<Alumno>(AlumnoAvisos);
            EliminarAlumnoCommand = new Command(EliminarAlumnoIniciado);

        }

        private void AlumnoAvisos(Alumno alumno)
        {

            AlumnoIniciado alumnoIniciado = new AlumnoIniciado();
            alumnoIniciado.ClaveAlumnoIniciado = alumno.Clave;
            alumnoIniciado.NombreEscuela = alumno.NombreEscuela;
            App.AvisosPrim.AlumnoIniciado = alumnoIniciado;
            //await App.AvisosPrim.AvisosUpdate();
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        private void AgregarAlumnoIniciado()
        {
            Login Login = new Login();
            App.Current.MainPage.Navigation.PushAsync(Login);
        }
        private void EliminarAlumnoIniciado()
        {
            App.AvisosPrim.EliminarAlumno(App.AvisosPrim.AlumnoIniciado);
            AlumnoIniciado alumno = App.AvisosPrim.StartSession();
            if (alumno == null)
            {
                Login Login = new Login();
                App.Current.MainPage.Navigation.PushAsync(Login);
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
        }

        public Command AgregarAlumnoCommand { get; set; }
    }
}
