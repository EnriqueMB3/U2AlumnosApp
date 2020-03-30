using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace U2AlumnosApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : ContentPage
    {
        public MasterPage()
        {
            InitializeComponent();
            lista.Add(Alumnos);
            lista.Add(Alumnodos);

        }

        public List<Alumno> lista = new List<Alumno>();
        Alumno Alumnos = new Alumno
        {
            Clave = "161G0252",
            Nombre = "Jesus Emmanuel",

        };
        Alumno Alumnodos = new Alumno
        {
            Clave = "161G0252",
            Nombre = "Magdalena",

        };

        public class Alumno
        {
            public string Nombre { get; set; }
            public string Clave { get; set; }
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {

            listview.ItemsSource = lista;
        }
    }

}