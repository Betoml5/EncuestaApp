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
            _ = Satisfaccion <= 40 ? BgSatisfaccion = Color.Red : (Satisfaccion > 40 && Satisfaccion <= 80) ? BgSatisfaccion = Color.Yellow : BgSatisfaccion = Color.Green;
           
            Lugar = Encuestas.Average(x => x.Lugar) * 10;
            _ = Lugar <= 40 ? BgLugar = Color.Red : (Lugar > 40 && Lugar < 80)? BgLugar = Color.Yellow: BgLugar = Color.Green;
            
            Personal = Encuestas.Average(x => x.Personal) * 10;
            _ = Personal <= 40 ? BgPersonal = Color.Red : (Personal > 40 && Personal < 80) ? BgPersonal = Color.Yellow : BgPersonal = Color.Green;

            OnPropertyChanged(nameof(Satisfaccion));
            OnPropertyChanged(nameof(Lugar));
            OnPropertyChanged(nameof(Personal));

            OnPropertyChanged(nameof(BgSatisfaccion));
            OnPropertyChanged(nameof(BgLugar));
            OnPropertyChanged(nameof(BgPersonal));
            
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
