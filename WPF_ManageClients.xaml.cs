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
    /// Interaction logic for WPF_ManageClients.xaml
    /// </summary>
    public partial class WPF_ManageClients : Window
    {
        public WPF_ManageClients()
        {
            InitializeComponent();
            ClientService service = new ClientService();

            ReloadGrid();

            string arrowPath = "./assets/img/BackArrow.png";
            BitmapImage img = new BitmapImage(new Uri(arrowPath, UriKind.Relative));
            this.arrow.Source = img;


            //Fill address combobox
            var getAddresses = service.GetAddresses();
            List<string> displayAddress = new List<string>();
            foreach (var address in getAddresses)
            {
                string result = $"{address.STREET_NUMBER}, {address.CITY}, {address.ZIP_CODE}";
                displayAddress.Add(result);
            }
                
            this.comboAddress.ItemsSource = displayAddress;
        }

        private void AddClient(object sender, RoutedEventArgs e)
        {
            bool validation = InputDataValidator(textboxName, textboxSurname, textboxPESEL, textboxNIP, comboAddress);

            if (validation)
            {
                CarDealerManagementDBEntities db = new CarDealerManagementDBEntities();
                int parsed;
                bool parseNIP = int.TryParse(this.textboxNIP.Text, out parsed);

                string[] addressSplitted = (this.comboAddress.Text).Split(',');

                //Must assign value to variable , because LINQ Entities does not support 'ArrayIndex'
                string splitted0 = addressSplitted[0];


                foreach (string address in addressSplitted) address.Trim();

                var add = db.TB_ADDRESS
                    .Where(address => (address.STREET_NUMBER == splitted0))
                    .Select(adres => adres.ID_ADDRESS)
                    .FirstOrDefault();
                Console.WriteLine(add);

                TB_CLIENT client = new TB_CLIENT()
                {
                    NAME = this.textboxName.Text.Trim(),
                    SURNAME = this.textboxSurname.Text.Trim(),
                    PESEL = this.textboxPESEL.Text.Trim(),
                    NIP = parseNIP ? parsed : 0,
                    ID_CLIENT_ADDRESS = db.TB_ADDRESS.Where(address => address.STREET_NUMBER == splitted0)
                                                     .Select(adres => adres.ID_ADDRESS)
                                                     .FirstOrDefault()
                };


                // Add new object to DB
                db.TB_CLIENT.Add(client);
                db.SaveChanges();

                ResetFieldValue(this.textboxName, this.textboxSurname, this.textboxPESEL, this.textboxNIP);
                ReloadGrid();
            }
           

        }

        private void ResetFieldValue(params Control[] fields)
        {
            foreach (Control field in fields)
            {
                if (field is TextBox) (field as TextBox).Text = "";
                if (field is ComboBox) (field as ComboBox).Text = "";
            }
        }

        private void ReloadGrid()
        {
            ClientService service = new ClientService();
            var getList = service.GetList();
            List<ClientManager> display = new List<ClientManager>();

            foreach (var item in getList)
            {
                display.Add(new ClientManager(item));
            }
            this.ClientDG.ItemsSource = display;
        }

        private int? updatingClientID = null;
        private void ClientDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //Check for correct selection
            if (this.ClientDG.SelectedIndex >= 0 && this.ClientDG.SelectedItems.Count >= 0)
            {
                if (this.ClientDG.SelectedItems[0].GetType() == typeof(ClientManager))
                {
                    ClientManager selected = (ClientManager)this.ClientDG.SelectedItems[0];
                    this.textboxNameUpdate.Text = selected.Name;
                    this.textboxSurnameUpdate.Text = selected.Surname;
                    this.textboxPESELUpdate.Text = selected.PESEL;
                    this.textboxNIPUpdate.Text = selected.NIP;
                    string builder = $"{selected.StreetNumber}, {selected.City}, {selected.ZIPCODE}";
                    this.comboAddressUpdate.Text = builder;
                    this.updatingClientID = selected.ID;

                    ManageIsFieldEnabled(true, textboxName, textboxSurname, textboxPESEL, textboxNIP);
                }
            }
        }


        /// <summary>
        /// Update selected row in DB and DataGrid
        /// </summary>
        private void UpdateClient(object sender, RoutedEventArgs e)
        {
            bool validation = InputDataValidator(textboxNameUpdate, textboxSurnameUpdate, textboxPESELUpdate, textboxNIPUpdate, comboAddressUpdate);

            if (validation)
            {
                CarDealerManagementDBEntities db = new CarDealerManagementDBEntities();

                if (updatingClientID == null)
                {
                    ShowInformationMessageBox("Please select a row from the DataGrid to update", "Row not selected");
                    return;
                }
                var result = from r in db.TB_CLIENT where r.ID_CLIENT == this.updatingClientID select r;

                TB_CLIENT obj = result.SingleOrDefault();

                //Check for null
                if (obj != null)
                {
                    //dodać walidacje czy user wprowadza poprawne dane 
                    var id = obj.ID_CLIENT;
                    obj.NAME = this.textboxNameUpdate.Text.Trim();
                    obj.SURNAME = this.textboxSurnameUpdate.Text.Trim();
                    obj.PESEL = this.textboxPESELUpdate.Text.Trim();
                    // dodać walidacje czy user wprowadza int
                    obj.NIP = int.Parse(this.textboxNIPUpdate.Text);

                    string[] addressSplitted = (this.comboAddressUpdate.Text.Trim()).Split(',');
                    //Must assign value to variable , because LINQ Entities does not support 'ArrayIndex'
                    string splitted0 = addressSplitted[0];
                    obj.ID_CLIENT_ADDRESS = db.TB_ADDRESS.Where(address => address.STREET_NUMBER == splitted0)
                                                     .Select(adres => adres.ID_ADDRESS)
                                                     .FirstOrDefault();

                    db.SaveChanges();

                    //Reset values
                    ResetFieldValue(this.textboxNameUpdate, this.textboxNameUpdate, this.textboxNameUpdate, this.textboxNameUpdate);
                    this.comboAddressUpdate.Text = "";
                    updatingClientID = null;
                    ManageIsFieldEnabled(false, textboxNameUpdate, textboxNameUpdate, textboxNameUpdate, textboxNameUpdate, comboAddressUpdate);

                    ReloadGrid();

                    ShowInformationMessageBox("Item successfully updated!", "Update");
                }
            }
            else
            {
                ShowInformationMessageBox("Some fields contain errors, please check them", "Error");
                return;
            } 
        }

        private void ManageIsFieldEnabled(bool state, params Control[] fields)
        {
            foreach (Control field in fields)
                field.IsEnabled = state;
        }

        /// <summary>
        /// Display simple MessageBox
        /// </summary>
        private void ShowInformationMessageBox(string content, string header)
        {
            MessageBox.Show(content,
                header,
                MessageBoxButton.OK,
                MessageBoxImage.Information,
                MessageBoxResult.OK);
        }

        private void DeleteClient(object sender, RoutedEventArgs e)
        {
            if (updatingClientID == null)
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
                var result = from d in context.TB_CLIENT
                             where d.ID_CLIENT == this.updatingClientID
                             select d;

                TB_CLIENT obj = result.SingleOrDefault();

                if (obj != null)
                {
                    context.TB_CLIENT.Remove(obj);
                    context.SaveChanges();

                    ResetFieldValue(textboxNameUpdate, textboxSurnameUpdate, textboxPESELUpdate, textboxNIPUpdate);
                    this.comboAddressUpdate.Text = "";
                    updatingClientID = null;
                    ReloadGrid();
                }
            }
        }

        /// <summary>
        /// Go back to Main Window after click 
        /// </summary>
        private void returnMainWindow(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private bool InputDataValidator(params Control[] fields)
        {
            foreach (Control field in fields)
            {
                if (field is TextBox)
                {
                    var textBox = (field as TextBox);
                    if (textBox.Text.Trim() == "") {
                        ShowInformationMessageBox("Inputs cannot be empty or blank", "Empty fields");
                        return false;
                    } 
                    if (textBox.Text.Trim().Length >= 50) return false;
                }

                if (field is ComboBox) {
                    if ((field as ComboBox).Text == "")
                    {
                        ShowInformationMessageBox("Must select all fiels!", "Empty fields");
                        return false;
                    }
                } 
            }
            return true;
        }
    }
}
