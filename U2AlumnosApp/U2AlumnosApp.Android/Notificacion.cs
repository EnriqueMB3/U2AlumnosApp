using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

[assembly:Xamarin.Forms.Dependency(typeof(U2AlumnosApp.Droid.Notificacion))]
namespace U2AlumnosApp.Droid
{
    public class Notificacion : INotificacion
    {
        public void Notificar(string titulo, string contenido, string nombreMaestro, int id)
        {
            Intent intent = new Intent(Application.Context, typeof(MainActivity));
            intent.PutExtra("titulo", titulo);
            intent.PutExtra("contenido", contenido);
            intent.PutExtra("nombre", nombreMaestro);

            PendingIntent pi = PendingIntent.GetActivity(Application.Context, 1, intent, PendingIntentFlags.OneShot);

            NotificationCompat.Builder noti = new NotificationCompat.Builder(Application.Context, "ALUMNOSAPP2");

            noti.SetContentTitle(titulo)
                .SetContentText(contenido)
                .SetSmallIcon(Resource.Drawable.abc_ic_star_black_36dp)
                .SetPriority((int)NotificationPriority.High)
                .SetDefaults((int)NotificationDefaults.All)
                .SetContentIntent(pi)
                .SetAutoCancel(true);


            NotificationManager manager =
                (NotificationManager)Application.Context.GetSystemService(Application.NotificationService);

            if (Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                manager.CreateNotificationChannel(new NotificationChannel("ALUMNOSAPP2", "APP", NotificationImportance.High));
            }


            var notificacion = noti.Build();

            manager.Notify(id, notificacion);
        }
    }
}