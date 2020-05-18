using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using U2AlumnosApp.Models;
using U2AlumnosApp.Views;
using Xamarin.Forms;

namespace U2AlumnosApp.ViewModels
{
    public class AvisosViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        void Actualizar([CallerMemberName]string nombre = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }


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
        private bool cargando;

        public bool Cargando
        {
            get { return cargando; }
            set { cargando = value; Actualizar(); }
        }

        private int avisosGeneralesCount;
        public int AvisosGeneralesCount
        {
            get { return avisosGeneralesCount; }
            set { avisosGeneralesCount = value; Actualizar(); }
        }


        //private bool visible;
        //public bool Visible
        //{
        //    get { return visible; }
        //    set { visible = value; Actualizar(); }
        //}
        public Command<Aviso> AvisoAlumnoCommand { get;  set; }
        public Command AvisosGeneralesCommand { get; private set; }

        public ObservableCollection<Aviso> Avisos { get; set; }
        List<Aviso> AvisosEnviados;

        public AvisosViewModel(AlumnoIniciado alumno)
        {
            Avisos = new ObservableCollection<Aviso>();
            AvisosEnviados = App.AvisosPrim.GetAvisosEnviados(alumno.ClaveAlumnoIniciado);
            AvisosGeneralesCount = App.AvisosPrim.CountGenerales(alumno.NombreEscuela);

            if (AvisosEnviados.Count == 0)
            {
                Vacio = true;
            }
            else
            {
                foreach (var item in AvisosEnviados)
                {
                    Avisos.Add(item);
                }
                Lleno = true;
            }

            AvisoAlumnoCommand = new Command<Aviso>(AvisoAlumnoVer);
            AvisosGeneralesCommand = new Command(AvisosGenerales);
        }

        AvisoAlumnoPage avisoAlumnoPage; 
        private async void AvisoAlumnoVer(Aviso obj)
        {
            avisoAlumnoPage = new AvisoAlumnoPage();
            avisoAlumnoPage.BindingContext = obj;
            await App.Current.MainPage.Navigation.PushAsync(avisoAlumnoPage);
        }

      

        private async void AvisosGenerales()
        {
            Cargando = true;
            await Task.Run(() => App.AvisosPrim.AvisosGeneralesUpdate());
            Cargando = false;
            await App.Current.MainPage.Navigation.PushAsync(new AvisosGeneralesPage());
        }
       
    }
}
