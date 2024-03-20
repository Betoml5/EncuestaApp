using EncuestaApp.Models;
using EncuestaApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncuestaApp.ViewModels
{
    public class EncuestaViewModel : INotifyPropertyChanged
    {
        EncuestaService server = new EncuestaService();

        List<Encuesta> Encuestas = new List<Encuesta>();
        public double Satisfaccion { get; set; }
        public double Lugar { get; set; }
        public double Personal { get; set; }



        public EncuestaViewModel()
        {
            server.OnEncuestaRecibida += Server_OnEncuestaRecibida;
            server.Iniciar();
        }

        private void Server_OnEncuestaRecibida(object sender, Encuesta e)
        {
           Encuestas.Add(e);

           Satisfaccion = Encuestas.Average(x => x.Satisfaccion) * 10;
           Lugar = Encuestas.Average(x => x.Lugar) * 10;
           Personal = Encuestas.Average(x => x.Personal) * 10;

           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Satisfaccion"));
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Lugar"));
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Personal"));
          
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
