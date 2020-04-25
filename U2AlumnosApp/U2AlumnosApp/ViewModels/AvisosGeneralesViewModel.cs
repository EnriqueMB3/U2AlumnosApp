﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using U2AlumnosApp.Models;

namespace U2AlumnosApp.ViewModels
{
    public class AvisosGeneralesViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        void Actualizar([CallerMemberName]string nombre = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
        public ObservableCollection<AvisosGenerales> AvisosGenerales { get; set; }
        List<AvisosGenerales> AvisosGeneralesEnviados;

        private bool lleno;
        public bool Lleno
        {
            get { return lleno; }
            set { lleno = value; Actualizar(); }
        }

        private bool vacio;
        public bool Vacio
        {
            get { return vacio; }
            set { vacio = value; Actualizar(); }
        }
        public AvisosGeneralesViewModel()
        {

            AvisosGenerales = new ObservableCollection<AvisosGenerales>();
            AvisosGeneralesEnviados = App.AvisosPrim.GetAvisosGeneralesEnviados(App.AvisosPrim.AlumnoIniciado.NombreEscuela);
            if (App.AvisosPrim.CountGenerales(App.AvisosPrim.AlumnoIniciado.NombreEscuela) == 0)
            {
                Vacio = true;
            }
            else
            {
                foreach (var item in AvisosGeneralesEnviados)
                {
                    AvisosGenerales.Add(item);
                }
            }
        }
    }
}
