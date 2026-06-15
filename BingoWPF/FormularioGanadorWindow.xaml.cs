using System.Windows;

namespace BingoWPF
{
    public partial class FormularioGanadorWindow : Window
    {
        public string NombreGanador { get; private set; } = "";
        public string PremioGanado { get; private set; } = "";

        public FormularioGanadorWindow(string jugada, string premioBase, bool esCartonLleno)
        {
            InitializeComponent();

            LblJugada.Text = "Jugada: " + jugada;
            TxtPremio.Text = premioBase;

            if (esCartonLleno)
            {
                TxtPremio.IsEnabled = false;
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = TxtNombre.Text.Trim();
            string premio = TxtPremio.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Ingrese el nombre de la persona.");
                return;
            }

            if (string.IsNullOrWhiteSpace(premio))
            {
                MessageBox.Show("Ingrese qué ganó o el premio.");
                return;
            }

            NombreGanador = nombre;
            PremioGanado = premio;

            DialogResult = true;
            Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}