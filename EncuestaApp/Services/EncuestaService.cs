using EncuestaApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace EncuestaApp.Services
{
    public class EncuestaService
    {
        HttpListener server = new HttpListener();

        public EncuestaService()
        {
            server.Prefixes.Add("http://*:7000/encuesta/");
        }

        public void Iniciar()
        {
            if (!server.IsListening)
            {
                server.Start();
                new Thread(Escuchar)
                {
                    IsBackground = true
                }.Start();
            }
        }



        private void Escuchar(object obj)
        {
            while (true)
            {       
                var context = server.GetContext();
                var pagina = File.ReadAllText("Assets/Index.html");
                var bufferPagina = Encoding.UTF8.GetBytes(pagina);

                if (context.Request.Url != null)
                {
                    if (context.Request.Url.LocalPath == "/encuesta/")
                    {
                        context.Response.ContentLength64 = bufferPagina.Length;
                        context.Response.OutputStream.Write(bufferPagina, 0, bufferPagina.Length);
                        context.Response.StatusCode = 200;
                        context.Response.OutputStream.Close();
                    } else if (context.Request.HttpMethod == "POST" && context.Request.Url.LocalPath == "/encuesta/crear")
                    {
                        var bufferDatos = new byte[context.Request.ContentLength64];
                        context.Request.InputStream.Read(bufferDatos, 0, bufferDatos.Length);
                        var datos = Encoding.UTF8.GetString(bufferDatos);

                        var diccionario = HttpUtility.ParseQueryString(datos);
                        var encuesta = new Encuesta()
                        {
                           Satisfaccion = int.Parse(diccionario["satisfaccion"]),
                           Personal = int.Parse(diccionario["personal"]),
                           Lugar = int.Parse(diccionario["lugar"])
                        };

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            OnEncuestaRecibida?.Invoke(this, encuesta);
                        });

                        context.Response.StatusCode = 200;
                        context.Response.OutputStream.Close();
                    }
                }
                else
                {
                    context.Response.StatusCode = 404;
                    context.Response.OutputStream.Close();
                }
            }
        }

        public void Detener()
        {
            if (server.IsListening)
            {
                server.Stop();
            }
        }   

        public event EventHandler<Encuesta> OnEncuestaRecibida;
    }
}
