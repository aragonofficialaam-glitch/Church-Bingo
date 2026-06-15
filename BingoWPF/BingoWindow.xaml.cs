using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BingoWPF
{
    public partial class BingoWindow : Window
    {
        private readonly List<string> jugadas;
        private readonly string premio;

        private readonly List<int> numerosDisponibles = Enumerable.Range(1, 75).ToList();
        private readonly List<int> historial = new List<int>();
        private readonly Dictionary<int, Border> celdas = new Dictionary<int, Border>();
        private readonly List<string> resumenGanadores = new List<string>();

        private bool animando = false;
        private readonly DateTime fechaInicioBingo = DateTime.Now;
        private readonly string nombreSorteo = "Bingo PPJ y PJ";

        public BingoWindow(List<string> jugadas, string premio)
        {
            InitializeComponent();

            this.jugadas = jugadas;
            this.premio = premio;

            LblPremio.Text = "Premio Final: " + premio;
            LblUltimos.Text = "Últimos: -";

            foreach (string j in jugadas)
            {
                LstJugadas.Items.Add(j);
            }

            CrearTablero();
        }

        private void CrearTablero()
        {
            GridTablero.Children.Clear();
            GridTablero.RowDefinitions.Clear();
            GridTablero.ColumnDefinitions.Clear();

            for (int fila = 0; fila < 5; fila++)
                GridTablero.RowDefinitions.Add(new RowDefinition());
            for (int col = 0; col < 15; col++)
                GridTablero.ColumnDefinitions.Add(new ColumnDefinition());

            // Ajusta cada fila independientemente: (izq, arriba, der, abajo)
            Thickness[] margenesPorFila = new Thickness[]
            {
                new Thickness(-3, 6, -3, 13),    // fila 1 R
                new Thickness(-3, 3, -3, 13),    // fila 2
                new Thickness(-3, 1, -3, 13),    // fila 3
                new Thickness(-3, -3, -3, 13),    // fila 4
                new Thickness(-3, -9, -3, 13),   // fila 5
            };

            // Ancho y alto independiente por fila
            double[] anchosPorFila = new double[] { 90, 90, 90, 90, 90 };
            double[] altosPorFila = new double[] { 127, 127, 127, 127, 127 };

            int numero = 1;

            for (int fila = 0; fila < 5; fila++)
            {
                for (int col = 0; col < 15; col++)
                {
                    TextBlock txt = new TextBlock
                    {
                        Text = numero.ToString(),
                        FontSize = 50,
                        FontWeight = FontWeights.Bold,
                        Foreground = Brushes.Black,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        TextAlignment = TextAlignment.Center
                    };

                    Border celda = new Border
                    {
                        Width = anchosPorFila[fila],
                        Height = altosPorFila[fila],
                        Background = Brushes.Transparent,
                        BorderBrush = Brushes.Transparent,
                        BorderThickness = new Thickness(0),
                        Margin = margenesPorFila[fila],
                        CornerRadius = new CornerRadius(4),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Child = txt
                    };

                    Grid.SetRow(celda, fila);
                    Grid.SetColumn(celda, col);

                    GridTablero.Children.Add(celda);
                    celdas[numero] = celda;

                    numero++;
                }
            }
        }

        private void ActualizarUltimos()
        {
            List<int> ultimos = historial
                .Skip(System.Math.Max(0, historial.Count - 7))
                .ToList();
            ultimos.Reverse();

            LblUltimos.Text = ultimos.Count == 0
                ? "Últimos: -"
                : "Últimos: " + string.Join(" - ", ultimos);
        }

        private void MarcarNumero(int n)
        {
            if (!numerosDisponibles.Contains(n))
            {
                MessageBox.Show("Ese número ya salió.");
                return;
            }

            numerosDisponibles.Remove(n);
            historial.Add(n);

            TxtNumeroGrande.Text = n.ToString();

            if (celdas.ContainsKey(n))
            {
                celdas[n].Background = new SolidColorBrush(Color.FromRgb(255, 200, 0));
            }

            ActualizarUltimos();
        }

        private void BtnRevision_Click(object sender, RoutedEventArgs e)
        {
            if (animando) return;

            Window revision = new Window
            {
                Title = "Revisión",
                Width = 1500,
                Height = 750,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Background = new SolidColorBrush(Color.FromRgb(18, 18, 18))
            };

            Grid grid = new Grid();

            Grid contenido = new Grid();

            TextBlock titulo = new TextBlock
            {
                Text = "BINGO EN REVISIÓN",
                Foreground = Brushes.White,
                FontSize = 70,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 80, 0, 0)
            };

            TextBlock subtitulo = new TextBlock
            {
                Text = "SE ESTAN VERIFICANDO  LOS CARTONES",
                Foreground = Brushes.Gold,
                FontSize = 45,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            Button cerrar = new Button
            {
                Content = "CONTINUAR",
                Width = 280,
                Height = 75,
                FontSize = 26,
                FontWeight = FontWeights.Bold,
                Background = new SolidColorBrush(Color.FromRgb(39, 174, 96)),
                Foreground = Brushes.Black,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, 70)
            };

            cerrar.Click += (s, a) => revision.Close();

            contenido.Children.Add(titulo);
            contenido.Children.Add(subtitulo);
            contenido.Children.Add(cerrar);

            grid.Children.Add(contenido);

            revision.Content = grid;
            revision.ShowDialog();
        }

        private void BtnHistorial_Click(object sender, RoutedEventArgs e)
        {
            if (animando) return;

            Window historialWindow = new Window
            {
                Title = "Historial",
                Width = 1500,
                Height = 750,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Background = new SolidColorBrush(Color.FromRgb(18, 18, 18))
            };

            Grid grid = new Grid();

            TextBlock titulo = new TextBlock
            {
                Text = "NUMEROS SALIDOS",
                Foreground = Brushes.White,
                FontSize = 60,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 40, 0, 0)
            };

            string numeros = historial.Count == 0
                ? "Ningún número ha salido aún."
                : string.Join("  -  ", historial);

            TextBlock txtNumeros = new TextBlock
            {
                Text = numeros,
                Foreground = Brushes.Gold,
                FontSize = 50,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(60, 0, 60, 0)
            };

            TextBlock txtConteo = new TextBlock
            {
                Text = $"Total: {historial.Count} números",
                Foreground = Brushes.White,
                FontSize = 38,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 130, 0, 0)
            };

            Button cerrar = new Button
            {
                Content = "CERRAR",
                Width = 280,
                Height = 75,
                FontSize = 26,
                FontWeight = FontWeights.Bold,
                Background = new SolidColorBrush(Color.FromRgb(39, 174, 96)),
                Foreground = Brushes.Black,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, 40)
            };

            cerrar.Click += (s, a) => historialWindow.Close();

            grid.Children.Add(titulo);
            grid.Children.Add(txtConteo);
            grid.Children.Add(txtNumeros);
            grid.Children.Add(cerrar);

            historialWindow.Content = grid;
            historialWindow.ShowDialog();
        }

        private void BtnDeshacer_Click(object sender, RoutedEventArgs e)
        {
            if (animando) return;
            if (!historial.Any()) return;

            int ultimo = historial.Last();

            historial.RemoveAt(historial.Count - 1);

            if (!numerosDisponibles.Contains(ultimo))
            {
                numerosDisponibles.Add(ultimo);
                numerosDisponibles.Sort();
            }

            if (celdas.ContainsKey(ultimo))
            {
                celdas[ultimo].Background = Brushes.Transparent;
            }

            TxtNumeroGrande.Clear();
            ActualizarUltimos();
        }

        private void BtnBingo_Click(object sender, RoutedEventArgs e)
        {
            if (animando) return;

            if (historial.Count < 4)
            {
                MessageBox.Show("No se puede cantar BINGO antes de 4 números marcados.");
                return;
            }

            if (LstJugadas.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una jugada.");
                return;
            }

            int indice = LstJugadas.SelectedIndex;
            string jugada = LstJugadas.SelectedItem.ToString() ?? "";

            if (jugada.StartsWith("✘ GANADA | "))
            {
                MessageBox.Show("Esa jugada ya está marcada como ganada.");
                return;
            }

            string jugadaLimpia = LimpiarJugada(jugada);
            bool esCartonLleno = EsCartonLleno(jugadaLimpia);

            FormularioGanadorWindow formulario =
                new FormularioGanadorWindow(jugadaLimpia, premio, esCartonLleno);

            bool? resultado = formulario.ShowDialog();

            if (resultado != true) return;

            LstJugadas.Items[indice] =
                "✘ GANADA | " + jugadaLimpia +
                " | " + formulario.NombreGanador +
                " | Premio: " + formulario.PremioGanado;

            LstJugadas.SelectedIndex = indice;

            LstJugadas.Dispatcher.InvokeAsync(() =>
            {
                ListBoxItem item = LstJugadas.ItemContainerGenerator.ContainerFromIndex(indice) as ListBoxItem;

                if (item != null)
                {
                    item.Foreground = Brushes.Yellow;
                }
            });

            resumenGanadores.Add(
                "Jugada: " + jugadaLimpia + "\n" +
                "Ganador: " + formulario.NombreGanador + "\n" +
                "Premio: " + formulario.PremioGanado + "\n"
            );

            if (TodasLasJugadasGanadas())
            {
                ReporteBingoPDF.GenerarReporte(
                    nombreSorteo,
                    fechaInicioBingo,
                    DateTime.Now,
                    premio,
                    jugadas,
                    resumenGanadores,
                    historial
                );

                ResumenBingoWindow win = new ResumenBingoWindow(resumenGanadores);
                win.Show();
                Close();
            }
        }

        private string LimpiarJugada(string jugada)
        {
            return jugada.Replace("✘ GANADA | ", "").Trim();
        }

        private bool EsCartonLleno(string jugada)
        {
            string texto = jugada.ToLower();

            return texto.Contains("carton lleno") ||
                   texto.Contains("cartón lleno") ||
                   texto.Contains("lleno") ||
                   texto.Contains("premio mayor");
        }

        private bool TodasLasJugadasGanadas()
        {
            foreach (object item in LstJugadas.Items)
            {
                string texto = item.ToString() ?? "";

                if (!texto.StartsWith("✘ GANADA | "))
                {
                    return false;
                }
            }

            return true;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (animando) return;

            MessageBoxResult confirmacion = MessageBox.Show(
                "¿Está seguro que desea cancelar el bingo?",
                "Cancelar Bingo",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (confirmacion == MessageBoxResult.Yes)
            {
                MainWindow principal = new MainWindow();
                principal.Show();
                this.Close();
            }
        }

        private void TxtNumeroGrande_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (int.TryParse(TxtNumeroGrande.Text.Trim(), out int n))
                {
                    if (n >= 1 && n <= 75)
                    {
                        MarcarNumero(n);
                    }
                    else
                    {
                        MessageBox.Show("Ingrese un número entre 1 y 75.");
                    }
                }

                TxtNumeroGrande.Clear();
                TxtNumeroGrande.Focus();

                e.Handled = true;
            }
        }

        private void LstJugadas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TxtNumeroGrande_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}