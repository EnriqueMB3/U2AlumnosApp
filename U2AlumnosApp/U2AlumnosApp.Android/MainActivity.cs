using System;
using U2AlumnosApp.Droid;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using U2AlumnosApp.Models;

namespace U2AlumnosApp.Droid
{
    [Activity(Label = "U2AlumnosApp", Icon = "@drawable/buhoSplash", LaunchMode = LaunchMode.SingleInstance, Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnNewIntent(Intent intent)
        {
            if (intent.HasExtra("titulo"))
            {
                string titulo = intent.GetStringExtra("titulo");
                string contenido = intent.GetStringExtra("contenido");
                string nombre = intent.GetStringExtra("nombre");

                AvisoAlumnoPage page = new AvisoAlumnoPage();
                Aviso aviso = new Aviso
                {
                    Titulo = titulo,
                    Contenido = contenido,
                    NombreMaestro = nombre
                };

                page.BindingContext = aviso;
                Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(page);
            }
        }
    }
}