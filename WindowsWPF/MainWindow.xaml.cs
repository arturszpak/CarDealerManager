using System.Windows;

namespace ProjektSemestralny
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChangeViewCar(object sender, RoutedEventArgs e)
        {

            WPF_CarDealerManager carWindow = new WPF_CarDealerManager();
            carWindow.Show();

            this.Close();
            
        }

        private void ChangeViewClient(object sender, RoutedEventArgs e)
        {
            WPF_ManageClients clientWindow = new WPF_ManageClients();
            clientWindow.Show();

            this.Close();
        }

        private void ChangeViewAddress(object sender, RoutedEventArgs e)
        {
            WPF_ManageAddress addressWindow = new WPF_ManageAddress();
            addressWindow.Show();

            this.Close();
        }

        private void ChangeViewTransaction(object sender, RoutedEventArgs e)
        {
            WPF_ManageTransactions transactionsWindow = new WPF_ManageTransactions();
            transactionsWindow.Show();

            this.Close();
        }
    }
}
