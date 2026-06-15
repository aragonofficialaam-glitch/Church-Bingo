using System.Collections.Generic;
using System.Windows;

namespace BingoWPF
{
    public partial class PreStartWindow : Window
    {
        private List<string> jugadasSeleccionadas = new List<string>();

        public PreStartWindow()
        {
            InitializeComponent();
            CargarJugadas();
        }

        private void CargarJugadas()
        {
            LstDisponibles.Items.Clear();

            LstDisponibles.Items.Add("Línea Horizontal");
            LstDisponibles.Items.Add("Línea Vertical");
            LstDisponibles.Items.Add("Cuatro Esquinas");
            LstDisponibles.Items.Add("Cartón Lleno");
            LstDisponibles.Items.Add("Letra T");
            LstDisponibles.Items.Add("Letra L");
            LstDisponibles.Items.Add("Línea Diagonal");
        }

        private void BtnAgregarJugada_Click(object sender, RoutedEventArgs e)
        {
            string nuevaJugada = TxtNuevaJugada.Text.Trim();

            if (string.IsNullOrWhiteSpace(nuevaJugada))
            {
                MessageBox.Show("Escriba el nombre de la jugada.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LstDisponibles.Items.Add(nuevaJugada);
            TxtNuevaJugada.Clear();
        }

        private void BtnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            if (LstDisponibles.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una jugada.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string jugada = LstDisponibles.SelectedItem.ToString();

            if (!jugadasSeleccionadas.Contains(jugada))
            {
                jugadasSeleccionadas.Add(jugada);
                LstSeleccionadas.Items.Add(jugada);
            }
        }

        private void BtnQuitar_Click(object sender, RoutedEventArgs e)
        {
            if (LstSeleccionadas.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una jugada para quitar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string jugada = LstSeleccionadas.SelectedItem.ToString();

            jugadasSeleccionadas.Remove(jugada);
            LstSeleccionadas.Items.Remove(jugada);
        }

        private void BtnIniciar_Click(object sender, RoutedEventArgs e)
        {
            string premio = TxtPremio.Text.Trim();

            if (string.IsNullOrWhiteSpace(premio))
            {
                MessageBox.Show("Ingrese el premio del cartón lleno.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (jugadasSeleccionadas.Count == 0)
            {
                MessageBox.Show("Seleccione al menos una jugada.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            BingoWindow ventana = new BingoWindow(jugadasSeleccionadas, premio);
            ventana.Show();

            this.Close();
        }
    }
}