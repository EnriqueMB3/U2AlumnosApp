using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using SQLite;
using System.Net.Http;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace U2AlumnosApp.Models
{
    public class AvisosPrimaria : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        void Actualizar([CallerMemberName]string nombre = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }

        SQLiteConnection connection;
        readonly string ruta = $"{System.Environment.GetFolderPath(Environment.SpecialFolder.Personal)}/alumosbase";


        private string claveAlumnoiniciado;
        public string ClaveAlumnoIniciado
        {
            get { return claveAlumnoiniciado; }
            set { claveAlumnoiniciado = value; Actualizar(); }
        }

        public string StartSession()
        {
            return claveAlumnoiniciado = connection.Table<Alumno>().First().Clave;
        }

        public AvisosPrimaria()
        {
            connection = new SQLiteConnection(ruta);
            connection.CreateTable<Alumno>();
            connection.CreateTable<Aviso>();
            connection.CreateTable<Maestro>();

        }

        public List<Alumno> GetAlumnosIniciados()
        {
            return new List<Alumno>(connection.Table<Alumno>());
        }

        public List<Aviso> GetAvisosEnviados(string id)
        {
            return new List<Aviso>(connection.Table<Aviso>().Where(x => x.ClaveAlumno == id));
        }

        public int CountAlumnos()
        {
            return connection.Table<Alumno>().Count();
        }



        public bool Verificar(string clave)
        {
            var alumList = connection.Table<Alumno>().ToList();
            if (alumList.Exists(x => x.Clave == clave))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task IniciarSesionAsync(string clave, string password)
        {


            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {

                if (!Verificar(clave))
                {
                    HttpClient httpClient = new HttpClient();
                    Dictionary<string, string> keyClave = new Dictionary<string, string>() { { "clave", clave } };

                    Dictionary<string, string> data = new Dictionary<string, string>() { { "clave", clave }, { "password", password } };
                    var json = await httpClient.PostAsync("https://avisosprimaria.itesrc.net/api/AlumnosApp/login",
                        new FormUrlEncodedContent(data));
                    json.EnsureSuccessStatusCode();
                    Alumno alumnoReceived = JsonConvert.DeserializeObject<Alumno>(await
                          json.Content.ReadAsStringAsync());

                    connection.Insert(alumnoReceived);

                    var jsonAvisos = await httpClient.PostAsync("https://avisosprimaria.itesrc.net/api/AlumnosApp/AvisosNuevosByClaveAlumno", new FormUrlEncodedContent(keyClave));
                    jsonAvisos.EnsureSuccessStatusCode();
                    List<Aviso> lista = JsonConvert.DeserializeObject<List<Aviso>>(await jsonAvisos.Content.ReadAsStringAsync());
                    if (lista != null)
                    {

                        foreach (var item in lista)
                        {
                            Aviso aviso = new Aviso()
                            {
                                IdAvisosEnviados = item.IdAvisosEnviados,
                                Titulo = item.Titulo,
                                Contenido = item.Contenido,
                                Estatus = item.Estatus,
                                ClaveMaestro = item.ClaveMaestro,
                                NombreMaestro = item.NombreMaestro,
                                FechaEnviar = item.FechaEnviar,
                                FechaLeido = item.FechaLeido,
                                FechaRecibido = item.FechaRecibido,
                                ClaveAlumno = clave,
                            };

                            connection.Insert(aviso);
                        }
                    }

                }
            }
            else
            {
                throw new Exception("No se puede inicias sesión, no hay conexión a internet.");
            }

        }

    }
}
