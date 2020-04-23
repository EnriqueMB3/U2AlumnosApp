﻿using System;
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
        List<Alumno> AlumnosApp;
        public MasterDetailViewModel()
        {
            Alumnos = new ObservableCollection<Alumno>();
            AlumnosApp = App.AvisosPrim.GetAlumnosIniciados();
            AlumnosApp.ForEach(x => Alumnos.Add(x));
            AgregarAlumnoCommand = new Command(AgregarAlumnoIniciado);
        }

        private void AgregarAlumnoIniciado()
        {
            Login Login = new Login();
            App.Current.MainPage.Navigation.PushAsync(Login);
        }

        public Command AgregarAlumnoCommand { get; set; }
    }
}
