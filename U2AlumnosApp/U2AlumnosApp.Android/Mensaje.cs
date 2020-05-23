using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

[assembly: Xamarin.Forms.Dependency(typeof(U2AlumnosApp.Droid.Mensaje))]

namespace U2AlumnosApp.Droid
{
    public class Mensaje : IMessage
    {
        public void ShowSnackBar(string text)
        {
            throw new NotImplementedException();
        }

        public void ShowSnackBar(string text, Action action)
        {
            throw new NotImplementedException();
        }

        public void ShowToast(string text)
        {
            var t = Toast.MakeText(Application.Context, text, ToastLength.Short);
            t.Show();
        }
    }
}