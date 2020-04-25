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


        private string nombreEscuela;
        public string NombreEscuela
        {
            get { return nombreEscuela; }
            set { nombreEscuela = value; Actualizar(); }
        }



        private AlumnoIniciado alumnoIniciado;
        public AlumnoIniciado AlumnoIniciado
        {
            get { return alumnoIniciado; }
            set { alumnoIniciado = value; Actualizar(); }
        }



        public AlumnoIniciado StartSession()
        {
            Alumno Alumno = connection.Table<Alumno>().First();
            AlumnoIniciado alumnoIniciado = new AlumnoIniciado
            {
                ClaveAlumnoIniciado = Alumno.Clave,
                NombreEscuela = Alumno.NombreEscuela
            };
            return alumnoIniciado;
        }

        public AvisosPrimaria()
        {
            connection = new SQLiteConnection(ruta);
            connection.CreateTable<Alumno>();
            connection.CreateTable<Aviso>();
            connection.CreateTable<Maestro>();
            connection.CreateTable<AvisosGenerales>();
         
        }

     
        public List<Alumno> GetAlumnosIniciados()
        {
            return new List<Alumno>(connection.Table<Alumno>());
        }

        public List<AvisosGenerales> GetAvisosGeneralesEnviados(string id)
        {
            return new List<AvisosGenerales>(connection.Table<AvisosGenerales>().Where(x => x.NombreEscuela == id));
        }
        public List<Aviso> GetAvisosEnviados(string id)
        {
            return new List<Aviso>(connection.Table<Aviso>().Where(x => x.ClaveAlumno == id));
        }

        public int CountAlumnos()
        {
            return connection.Table<Alumno>().Count();
        }

        public int CountGenerales(string nameEscuela)
        {
            return connection.Table<AvisosGenerales>().Where(x => x.NombreEscuela == nameEscuela).Count();
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

        public bool VerificarAvisoGeneral(int id)
        {
            var alumList = connection.Table<AvisosGenerales>().ToList();
            if (alumList.Exists(x => x.IdAvisosGenerales == id))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool VerificarAviso(int id)
        {
            var alumList = connection.Table<Aviso>().ToList();
            if (alumList.Exists(x => x.IdAvisosEnviados == id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<AlumnoIniciado> IniciarSesionAsync(string clave, string password)
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


                    var jsonAvisosGenerales = await httpClient.GetAsync($"https://avisosprimaria.itesrc.net/api/AvisosGenerales/NombreEscuela/{alumnoReceived.NombreEscuela}");
                    jsonAvisosGenerales.EnsureSuccessStatusCode();
                    List<AvisosGenerales> listaGeneral = JsonConvert.DeserializeObject<List<AvisosGenerales>>(await jsonAvisosGenerales.Content.ReadAsStringAsync());
                    if (listaGeneral != null)
                    {

                        foreach (var item in listaGeneral)
                        {
                            if (!VerificarAvisoGeneral(item.IdAvisosGenerales))
                            {
                                AvisosGenerales avisosGenerales = new AvisosGenerales
                                {
                                    Contenido = item.Contenido,
                                    IdAvisosGenerales = item.IdAvisosGenerales,
                                    FechaCaducidad = item.FechaCaducidad,
                                    Titulo = item.Titulo,
                                    FechaEnviado = item.FechaEnviado,
                                    NombreEscuela = alumnoReceived.NombreEscuela
                                };
                                connection.Insert(avisosGenerales);
                            }
                        }
                    }
                    AlumnoIniciado alumnoIniciado = new AlumnoIniciado()
                    {
                        ClaveAlumnoIniciado = alumnoReceived.Clave,
                        NombreEscuela = alumnoReceived.NombreEscuela
                    };
                    return alumnoIniciado;
                }
                return null;
            }
            else
            {
                return null;
                throw new Exception("No se puede inicias sesión, no hay conexión a internet.");

            }

        }
        public async Task AvisosUpdate()
        {
            HttpClient httpClient = new HttpClient();
            Dictionary<string, string> keyClave = new Dictionary<string, string>() { { "clave", AlumnoIniciado.ClaveAlumnoIniciado } };

            var jsonAvisos = await httpClient.PostAsync("https://avisosprimaria.itesrc.net/api/AlumnosApp/AvisosNuevosByClaveAlumno", new FormUrlEncodedContent(keyClave));
            jsonAvisos.EnsureSuccessStatusCode();
            List<Aviso> lista = JsonConvert.DeserializeObject<List<Aviso>>(await jsonAvisos.Content.ReadAsStringAsync());
            if (lista != null)
            {

                foreach (var item in lista)
                {
                    if (!VerificarAviso(item.IdAvisosEnviados))
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
                            ClaveAlumno = alumnoIniciado.ClaveAlumnoIniciado,
                        };

                        connection.Insert(aviso);
                    }
                }
            }
        }


        public async Task AvisosGeneralesUpdate()
        {
            HttpClient httpClient = new HttpClient();

            var jsonAvisosGenerales = await httpClient.GetAsync($"https://avisosprimaria.itesrc.net/api/AvisosGenerales/NombreEscuela/{alumnoIniciado.NombreEscuela}");
            jsonAvisosGenerales.EnsureSuccessStatusCode();
            List<AvisosGenerales> listaGeneral = JsonConvert.DeserializeObject<List<AvisosGenerales>>(await jsonAvisosGenerales.Content.ReadAsStringAsync());
            if (listaGeneral != null)
            {

                foreach (var item in listaGeneral)
                {
                    if (!VerificarAvisoGeneral(item.IdAvisosGenerales))
                    {
                        AvisosGenerales avisosGenerales = new AvisosGenerales
                        {
                            Contenido = item.Contenido,
                            IdAvisosGenerales = item.IdAvisosGenerales,
                            FechaCaducidad = item.FechaCaducidad,
                            Titulo = item.Titulo,
                            FechaEnviado = item.FechaEnviado,
                            NombreEscuela = alumnoIniciado.NombreEscuela
                        };
                        connection.Insert(avisosGenerales);
                    }
                }
            }
          
            
        }

    }
    }
