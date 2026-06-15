using System.Windows;

namespace BingoWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnPrincipal_Click(object sender, RoutedEventArgs e)
        {
            PreStartWindow win = new PreStartWindow();
            win.Show();
            Close();
        }

        
    }
}