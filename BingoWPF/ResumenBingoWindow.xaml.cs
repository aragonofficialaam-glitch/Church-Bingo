using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace BingoWPF
{
    public partial class ResumenBingoWindow : Window
    {
        private readonly List<string> ganadores;

        public ResumenBingoWindow(List<string> ganadores)
        {
            InitializeComponent();

            this.ganadores = ganadores;
            CargarResumen();
        }

        private void CargarResumen()
        {
            TxtResumen.Inlines.Clear();

            // TÍTULO
            TxtResumen.Inlines.Add(new Run("GANADORES REGISTRADOS\n\n")
            {
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                FontSize = 60  // 👈 aquí
            });

            if (ganadores.Count == 0)
            {
                TxtResumen.Inlines.Add(new Run("No se registraron ganadores.\n\n"));
            }
            else
            {
                for (int i = 0; i < ganadores.Count; i++)
                {
                    TxtResumen.Inlines.Add(new Run($"{i + 1}.\n")
                    {
                        Foreground = Brushes.Gold,
                        FontSize = 30
                    });

                    TxtResumen.Inlines.Add(new Run(ganadores[i] + "\n"));

                    TxtResumen.Inlines.Add(new Run("------------------------------\n")
                    {
                        Foreground = Brushes.White
                    });
                }
            }

            // MENSAJE FINAL
            TxtResumen.Inlines.Add(new Run("\nFELICIDADES A TODOS LOS GANADORES.\n")
            {
                Foreground = Brushes.Black,
                FontWeight = FontWeights.Bold
            });

            TxtResumen.Inlines.Add(new Run("Muchas GRACIAS por colaborar y participar en el bingo.")
            {
                Foreground = Brushes.Black
            });
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow();
            win.Show();
            Close();
        }
    }
}