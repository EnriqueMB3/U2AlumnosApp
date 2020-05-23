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

        private bool cargandoRefresh;

        public bool CargandoRefresh
        {
            get { return cargandoRefresh; }
            set { cargandoRefresh = value; Actualizar(); }
        }

        private int avisosGeneralesCount;
        public int AvisosGeneralesCount
        {
            get { return avisosGeneralesCount; }
            set { avisosGeneralesCount = value; Actualizar(); }
        }
        public Command<Aviso> AvisoAlumnoCommand { get; set; }
        public Command AvisosGeneralesCommand { get; private set; }
        public Command AvisosNuevosCommand { get; set; }
        public ObservableCollection<Aviso> Avisos { get; set; }
        List<Aviso> AvisosEnviados;
        public AlumnoIniciado AlumnoIniciado { get; set; }
        public AvisosViewModel(AlumnoIniciado alumno)
        {
            try
            {
                AlumnoIniciado = alumno;
                Avisos = new ObservableCollection<Aviso>();
                Task.Run(() => App.AvisosPrim.AvisosUpdate());
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
                AvisosNuevosCommand = new Command(RevisarNuevoasAvisos);
            }
            catch (Exception ex)
            {
                Mensaje?.Invoke(ex.Message);
            }

        }

        private async void RevisarNuevoasAvisos()
        {
            try
            {
                CargandoRefresh = true;
                await Task.Run(() => App.AvisosPrim.AvisosUpdate());
                AvisosEnviados = App.AvisosPrim.GetAvisosEnviados(AlumnoIniciado.ClaveAlumnoIniciado);
                AvisosGeneralesCount = App.AvisosPrim.CountGenerales(AlumnoIniciado.NombreEscuela);
                Avisos.Clear();
                foreach (var item in AvisosEnviados)
                {
                    Avisos.Add(item);
                }
                CargandoRefresh = false;
            }
            catch (Exception ex)
            {
                Mensaje?.Invoke(ex.Message);

            }

        }

        AvisoAlumnoPage avisoAlumnoPage;

        public event Action<string> Mensaje;
        private async void AvisoAlumnoVer(Aviso obj)
        {
            try
            {
                avisoAlumnoPage = new AvisoAlumnoPage();
                avisoAlumnoPage.BindingContext = obj;
                Cargando = true;
                await App.AvisosPrim.AvisosMaestroVisto(obj);
                Cargando = false;
                await App.Current.MainPage.Navigation.PushAsync(avisoAlumnoPage);
            }
            catch (Exception ex)
            {
                Cargando = false;
                Mensaje?.Invoke(ex.Message);
            }

        }



        private async void AvisosGenerales()
        {
            try
            {
                Cargando = true;
                await Task.Run(() => App.AvisosPrim.AvisosGeneralesUpdate());
                Cargando = false;
                await App.Current.MainPage.Navigation.PushAsync(new AvisosGeneralesPage());
            }
            catch (Exception ex)
            {
                Mensaje?.Invoke(ex.Message);
            }
        }




    }
}
