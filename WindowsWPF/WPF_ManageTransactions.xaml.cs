using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjektSemestralny
{
    /// <summary>
    /// Interaction logic for WPF_ManageTransactions.xaml
    /// </summary>
    public partial class WPF_ManageTransactions : Window
    {
        TransactionService service = new TransactionService();
        public WPF_ManageTransactions()
        {
            InitializeComponent();
            ReloadGrid();

           
            var getCars = service.GetCars();
            var getClients = service.GetClients();

            // FILL CARS COMBOBOX
            List<string> displayCars = new List<string>();
            foreach (var car in getCars)
            {
                string carBuilder = $"{car.ID_CAR}, " +
                                    $"{car.TB_CAR_MODEL.CAR_MODEL}, " +
                                    $"{car.TB_CAR_COLOR.COLOR}, " +
                                    $"{car.TB_CAR_CONDITION.CONDITION}, " +
                                    $"{car.TB_CAR_COUNTRY.COUNTRY}";
                displayCars.Add(carBuilder);
            }
            this.comboboxCar.ItemsSource = displayCars;

            // FILL CLIENT COMBOBOX
            List<string> displayClient = new List<string>();
            foreach (var client in getClients) 
            {
                string clientBuilder = $"{client.ID_CLIENT}, " +
                                       $"{client.NAME} {client.SURNAME}, " +
                                       $"{client.PESEL}, " +
                                       $"nip: {client.NIP}, " +
                                       $"{client.TB_ADDRESS.STREET_NUMBER}, " +
                                       $"{client.TB_ADDRESS.CITY}, " +
                                       $"{client.TB_ADDRESS.ZIP_CODE}";
                displayClient.Add(clientBuilder);
            }
               
            this.comboboxClient.ItemsSource = displayClient;
        }

        /// <summary>
        /// Reload Grid
        /// </summary>
        private void ReloadGrid()
        {
            TransactionService service = new TransactionService();
            var getList = service.GetList();
            List<TransactionManager> display = new List<TransactionManager>();

            foreach (var item in getList)
            {
                display.Add(new TransactionManager(item));
            }
            this.TransactionsDG.ItemsSource = display;
        }

        private int? updatingTransactionID = null;
        /// <summary>
        /// Populate Update form on row selection in datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TransactionsDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Check for correct selection
            if (this.TransactionsDG.SelectedIndex >= 0 && this.TransactionsDG.SelectedItems.Count >= 0)
            {
                if (this.TransactionsDG.SelectedItems[0].GetType() == typeof(TransactionManager))
                {
                    TransactionManager selected = (TransactionManager)this.TransactionsDG.SelectedItems[0];
                    Console.WriteLine(selected.Client.ToString());
                    this.displayClientBox.Text = selected.Client.ToString();
                    this.displayCarBox.Text = selected.Car.ToString();
                    this.datepickerDisplayTransactionDate.SelectedDate = selected.DateOfTransaction;
                    this.updatingTransactionID = selected.ID;
                }
            }
        }

        /// <summary>
        /// Add record to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTransaction(object sender, RoutedEventArgs e)
        {
            CarDealerManagementDBEntities db = new CarDealerManagementDBEntities();

            //ComboBox SelectedIndex property counts from 0, so we add 1 to match ID
            TB_TRANSACTIONS transaction = new TB_TRANSACTIONS()
            {
                ID_CAR = db.TB_CAR.Where(car => car.ID_CAR == this.comboboxCar.SelectedIndex+1).Select(car => car.ID_CAR).FirstOrDefault(),
                ID_CLIENT = db.TB_CLIENT.Where(client => client.ID_CLIENT == this.comboboxClient.SelectedIndex+1).Select(client => client.ID_CLIENT).FirstOrDefault(),
                TRANSACTION_DATE = (DateTime)(this.datepickerTransaction.SelectedDate)
            };

            // Add new object to DB
            db.TB_TRANSACTIONS.Add(transaction);
            db.SaveChanges();

            ResetFieldValue(this.comboboxCar, this.comboboxClient, this.datepickerTransaction);
            ReloadGrid();
        }

        /// <summary>
        /// Delete record from the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteTransaction(object sender, RoutedEventArgs e)
        {
            if (updatingTransactionID == null)
            {
                ShowInformationMessageBox("Select a row to delete", "No row selected");
                return;
            }

            //Ask user to confirm action
            MessageBoxResult removeResult = MessageBox.Show("Remove selected object from database?",
                "Remove",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No);

            if (removeResult == MessageBoxResult.Yes)
            {
                CarDealerManagementDBEntities context = new CarDealerManagementDBEntities();
                var result = from d in context.TB_TRANSACTIONS
                             where d.ID_TRANSACTION == this.updatingTransactionID
                             select d;

                TB_TRANSACTIONS obj = result.SingleOrDefault();

                if (obj != null)
                {
                    context.TB_TRANSACTIONS.Remove(obj);
                    context.SaveChanges();

                    ResetFieldValue(displayCarBox, displayClientBox, datepickerDisplayTransactionDate);
                    updatingTransactionID = null;
                    ReloadGrid();
                }
            }
        }

        /// <summary>
        /// Reset field text property
        /// </summary>
        /// <param name="fields"></param>
        private void ResetFieldValue(params Control[] fields)
        {
            foreach (Control field in fields)
            {
                if (field is TextBox) (field as TextBox).Text = "";
                if (field is ComboBox) (field as ComboBox).Text = "";
                if (field is DatePicker) (field as DatePicker).SelectedDate = null;
            }
        }

        /// <summary>
        /// Close current window and open main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReturnMainWindow(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Show simple MessageBox
        /// </summary>
        /// <param name="content"></param>
        /// <param name="header"></param>
        private void ShowInformationMessageBox(string content, string header)
        {
            MessageBox.Show(content,
                header,
                MessageBoxButton.OK,
                MessageBoxImage.Information,
                MessageBoxResult.OK);
        }
    }
}
