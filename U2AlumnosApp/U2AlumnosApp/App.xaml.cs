using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using U2AlumnosApp.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace U2AlumnosApp
{
    public partial class App : Application
    {
   
        public App()
        {
            InitializeComponent();

            if (AvisosPrim.CountAlumnos()>0)
            {
                if (AvisosPrim.AlumnoIniciado==null)
                {
                    
                    AvisosPrim.AlumnoIniciado = AvisosPrim.StartSession();
                }

                 Device.StartTimer(TimeSpan.FromMinutes(20), () =>
                 {
                     GetAvisosNuevos();
                     return true;
                 });
                MainPage = new NavigationPage(new MainPage());
            }
            else
            {

                MainPage = new Views.Login();
            }

        }

        INotificacion noti = DependencyService.Get<INotificacion>();
        private async void GetAvisosNuevos()
        {
            try
            {
                List<Aviso> lista = await AvisosPrim.GetAvisosNuevosNotif();

                if (lista != null)
                {   
                    foreach (Aviso aviso in lista)
                    {
                        string titulo = aviso.Titulo, contenido = aviso.Contenido, maestro = aviso.NombreMaestro;
                        int id = aviso.IdAvisosEnviados;
                        noti.Notificar(titulo, contenido, maestro, id);
                        await AvisosPrim.AvisosMaestroRecibido(aviso);
                        await Task.Delay(500);
                    }
                }
            }
            catch { }
        }

        public static AvisosPrimaria AvisosPrim { get; set; } = new AvisosPrimaria();
     

        protected override void OnStart()
        {
           
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
