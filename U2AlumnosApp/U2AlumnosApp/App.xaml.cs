﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using U2AlumnosApp.Models;


namespace U2AlumnosApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Views.Login();
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
