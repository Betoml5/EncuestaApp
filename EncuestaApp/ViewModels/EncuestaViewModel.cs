using EncuestaApp.Models;
using EncuestaApp.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace EncuestaApp.ViewModels
{
    public enum Color {Red,Yellow,Green }
    public class EncuestaViewModel : INotifyPropertyChanged
    {
        #region Propiedades
        private readonly EncuestaService server = new EncuestaService();
        public ObservableCollection<Encuesta> Encuestas = new ObservableCollection<Encuesta>();
        public double Satisfaccion { get; set; }
        public double Lugar { get; set; }
        public double Personal { get; set; }
        public Color BgSatisfaccion { get; set; } = Color.Red;
        public Color BgLugar { get; set; } = Color.Red;
        public Color BgPersonal { get; set; } = Color.Red;
        public double HeigthSatisfaccion { get; set; }
        public double HeigthLugar { get; set; }
        public double HeigthPersonal { get; set; }
        #endregion
        public EncuestaViewModel()
        {
            server.OnEncuestaRecibida += Server_OnEncuestaRecibida;
            server.Iniciar();
            CargarDatos();
            Satisfaccion = Encuestas.Average(x => x.Satisfaccion) * 10;
            //Cambia el color de la barra dependiendo del promedio
            BgSatisfaccion = Satisfaccion <= 40 ? Color.Red : (Satisfaccion > 40 && Satisfaccion <= 80) ? Color.Yellow : Color.Green;

            Lugar = Encuestas.Average(x => x.Lugar) * 10;
            //Cambia el color de la barra dependiendo del promedio
            BgLugar = Lugar <= 40 ? Color.Red : (Lugar > 40 && Lugar < 80) ? Color.Yellow : Color.Green;

            Personal = Encuestas.Average(x => x.Personal) * 10;
            //Cambia el color de la barra dependiendo del promedio
            BgPersonal = Personal <= 40 ? Color.Red : (Personal > 40 && Personal < 80) ? Color.Yellow : Color.Green;

            //Tamaño de las barras
            HeigthLugar = Lugar * 2.25;
            HeigthPersonal = Personal * 2.25;
            HeigthSatisfaccion = Satisfaccion * 2.25;
        }

        private string _nombreJson = "data.json";
        public void GuardarArchivo()
        {
            var json = JsonSerializer.Serialize(Encuestas);
            File.WriteAllText(_nombreJson, json);
        }

        public void CargarDatos()
        {
            if (File.Exists(_nombreJson))
            {
                var json = File.ReadAllText(_nombreJson);
                var datos = JsonSerializer.Deserialize<ObservableCollection<Encuesta>>(json);
                if (datos != null)
                    Encuestas = datos;
                else
                    Encuestas = new ObservableCollection<Encuesta>();
            }
        }
        private void Server_OnEncuestaRecibida(object sender, Encuesta e)
        {
            Encuestas.Add(e);
            GuardarArchivo();
            Satisfaccion = Encuestas.Average(x => x.Satisfaccion) * 10;
            //Cambia el color de la barra dependiendo del promedio
            BgSatisfaccion = Satisfaccion <= 40 ? Color.Red : (Satisfaccion > 40 && Satisfaccion <= 80) ? Color.Yellow : Color.Green;
           
            Lugar = Encuestas.Average(x => x.Lugar) * 10;
            //Cambia el color de la barra dependiendo del promedio
            BgLugar = Lugar <= 40 ? Color.Red : (Lugar > 40 && Lugar < 80)? Color.Yellow: Color.Green;
            
            Personal = Encuestas.Average(x => x.Personal) * 10;
            //Cambia el color de la barra dependiendo del promedio
            BgPersonal = Personal <= 40 ? Color.Red : (Personal > 40 && Personal < 80) ? Color.Yellow : Color.Green;

            //Tamaño de las barras
            HeigthLugar = Lugar * 2.25;
            HeigthPersonal = Personal * 2.25;
            HeigthSatisfaccion = Satisfaccion * 2.25;
            
            OnPropertyChanged();
            
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Satisfaccion"));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Lugar"));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Personal"));
        }

        #region Implementacion de la interface Property Changed
        //Metodo para la Actualizacion de una propiedad, si no se le asigna una propiedad actualizara todas
        void OnPropertyChanged(string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
