using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using SQLite;
using System.Net.Http;
using Newtonsoft.Json;

namespace U2AlumnosApp.Models
{
    public class AvisosPrimaria
    {
        SQLiteConnection connection;
        readonly string ruta = $"{System.Environment.GetFolderPath(Environment.SpecialFolder.Personal)}/data";


        public AvisosPrimaria()
        {
            connection = new SQLiteConnection(ruta);
            connection.CreateTable<Alumno>();
            connection.CreateTable<Aviso>();
            connection.CreateTable<Maestro>();
        }

        public void Insert(Alumno alumno)
        {
            connection.Insert(alumno);
        }

        public async Task IniciarSesionAsync(string clave, string password)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                HttpClient httpClient = new HttpClient();
                Dictionary<string, string> data = new Dictionary<string, string>() { { "clave", clave }, { "password", password } };
                
                var json = await httpClient.PostAsync("https://avisosprimaria.itesrc.net/api/AlumnosApp/login",
                    new FormUrlEncodedContent(data));

                json.EnsureSuccessStatusCode();
                Alumno alumnoReceived = JsonConvert.DeserializeObject<Alumno>(await 
                        json.Content.ReadAsStringAsync());

                    Insert(alumnoReceived);
            }
            else
            {
                throw new Exception("No se puede inicias sesión, no hay conexión a internet.");
            }
        }
        }
    }
