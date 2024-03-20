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
    public enum Color {Red,Yellow,Green }
    public class EncuestaViewModel : INotifyPropertyChanged
    {
        private readonly EncuestaService server = new EncuestaService();
        private readonly List<Encuesta> Encuestas = new List<Encuesta>();
        public double Satisfaccion { get; set; }
        public double Lugar { get; set; }
        public double Personal { get; set; }
        public Color BgSatisfaccion { get; set; } = Color.Red;
        public Color BgLugar { get; set; } = Color.Red;
        public Color BgPersonal { get; set; } = Color.Red;


        public EncuestaViewModel()
        {
            server.OnEncuestaRecibida += Server_OnEncuestaRecibida;
            server.Iniciar();
        }

        private void Server_OnEncuestaRecibida(object sender, Encuesta e)
        {
            Encuestas.Add(e);

            Satisfaccion = Encuestas.Average(x => x.Satisfaccion) * 10;
            BgSatisfaccion = Satisfaccion <= 40 ? Color.Red : (Satisfaccion > 40 && Satisfaccion <= 80) ? Color.Yellow : Color.Green;
           
            Lugar = Encuestas.Average(x => x.Lugar) * 10;
            BgLugar = Lugar <= 40 ? Color.Red : (Lugar > 40 && Lugar < 80)? Color.Yellow: Color.Green;
            
            Personal = Encuestas.Average(x => x.Personal) * 10;
            BgPersonal = Personal <= 40 ? Color.Red : (Personal > 40 && Personal < 80) ? Color.Yellow : Color.Green;

            OnPropertyChanged();
            
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Satisfaccion"));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Lugar"));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Personal"));
        }


        void OnPropertyChanged(string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
