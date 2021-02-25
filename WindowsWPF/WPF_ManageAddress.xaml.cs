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
    /// Interaction logic for WPF_ManageAddress.xaml
    /// </summary>
    public partial class WPF_ManageAddress : Window
    {
        public WPF_ManageAddress()
        {
            InitializeComponent();
            ReloadGrid();
        }
        /// <summary>
        /// Reload DataGrid
        /// </summary>
        private void ReloadGrid()
        {
            AddressService service = new AddressService();
            var getList = service.GetList();
            List<AddressClass> display = new List<AddressClass>();

            foreach (var item in getList)
            {
                display.Add(new AddressClass(item));
            }
            this.AddressDG.ItemsSource = display;
        }


        private int? updatingAddressID = null;
        private void AddressDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Check for correct selection
            if (this.AddressDG.SelectedIndex >= 0 && this.AddressDG.SelectedItems.Count >= 0)
            {
                if (this.AddressDG.SelectedItems[0].GetType() == typeof(AddressClass))
                {
                    AddressClass selected = (AddressClass)this.AddressDG.SelectedItems[0];
                    this.textboxStreetUpdate.Text = selected.Street;
                    this.textboxCityUpdate.Text = selected.City;
                    this.textboxZIPUpdate.Text = selected.ZIP;
                    this.updatingAddressID = selected.ID;

                    ManageIsFieldEnabled(true, textboxStreetUpdate, textboxCityUpdate, textboxZIPUpdate);
                }
            }
        }

        /// <summary>
        /// Change field enabled
        /// </summary>
        /// <param name="state"></param>
        /// <param name="fields"></param>
        private void ManageIsFieldEnabled(bool state, params Control[] fields)
        {
            foreach (Control field in fields)
                field.IsEnabled = state;
        }

        /// <summary>
        /// Add new record to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAddress(object sender, RoutedEventArgs e)
        {
            bool validation = InputDataValidator(textboxStreet, textboxCity, textboxZIP);

            if (validation)
            {
                CarDealerManagementDBEntities db = new CarDealerManagementDBEntities();

                TB_ADDRESS address = new TB_ADDRESS()
                {
                    STREET_NUMBER = this.textboxStreet.Text,
                    CITY = this.textboxCity.Text,
                    ZIP_CODE = this.textboxZIP.Text
                };

                // Add new object to DB
                db.TB_ADDRESS.Add(address);
                db.SaveChanges();

                ResetFieldValue(this.textboxStreet, this.textboxCity, this.textboxZIP);
                ReloadGrid();
            }
        }

        /// <summary>
        /// Reset Text Content of Grid Element type of Control
        /// </summary>
        /// <param name="fields"></param>
        private void ResetFieldValue(params Control[] fields)
        {
            foreach (Control field in fields)
            {
                if (field is TextBox) (field as TextBox).Text = "";
                if (field is ComboBox) (field as ComboBox).Text = "";
            }
        }

        /// <summary>
        /// Update a record in database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateAddress(object sender, RoutedEventArgs e)
        {
            bool validation = InputDataValidator(textboxStreetUpdate, textboxCityUpdate, textboxZIPUpdate);

            if (validation) {
                CarDealerManagementDBEntities db = new CarDealerManagementDBEntities();

                if (updatingAddressID == null)
                {
                    ShowInformationMessageBox("Please select a row from the DataGrid to update", "Row not selected");
                    return;
                }
                var result = from r in db.TB_ADDRESS where r.ID_ADDRESS == this.updatingAddressID select r;

                TB_ADDRESS obj = result.SingleOrDefault();

                //Check for null
                if (obj != null)
                {
                    //dodać walidacje czy user wprowadza poprawne dane 
                    var id = obj.ID_ADDRESS;
                    obj.STREET_NUMBER = this.textboxStreetUpdate.Text;
                    obj.CITY = this.textboxCityUpdate.Text;
                    obj.ZIP_CODE = this.textboxZIPUpdate.Text;

                    db.SaveChanges();

                    //Reset values
                    ResetFieldValue(this.textboxStreetUpdate, this.textboxCityUpdate, this.textboxZIPUpdate);
                    updatingAddressID = null;
                    ManageIsFieldEnabled(false, textboxStreetUpdate, textboxStreetUpdate, textboxZIPUpdate);
                    ReloadGrid();
                    ShowInformationMessageBox("Item successfully updated!", "Update");
                }
            }

           
        }

        /// <summary>
        /// Delete row from database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteAddress(object sender, RoutedEventArgs e)
        {
            if (updatingAddressID == null)
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
                var result = from d in context.TB_ADDRESS
                             where d.ID_ADDRESS == this.updatingAddressID
                             select d;

                TB_ADDRESS obj = result.SingleOrDefault();

                if (obj != null)
                {
                    context.TB_ADDRESS.Remove(obj);
                    context.SaveChanges();

                    ResetFieldValue(textboxStreetUpdate, textboxCityUpdate, textboxZIPUpdate);
                    updatingAddressID = null;
                    ReloadGrid();
                }
            }
        }

        /// <summary>
        /// Return to main window
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
        /// Display simple MessageBox
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


        /// <summary>
        /// Validate added and updated records
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        private bool InputDataValidator(params Control[] fields)
        {
            foreach (Control field in fields)
            {
                if (field is TextBox)
                {
                    var textBox = (field as TextBox);
                    if (textBox.Text.Trim() == "")
                    {
                        ShowInformationMessageBox("Inputs cannot be empty or blank", "Empty fields");
                        return false;
                    }
                    if (textBox.Text.Trim().Length >= 50) {
                        ShowInformationMessageBox("Length cannot be longer than 50 characters!", "Too many characters");
                        return false;
                    } 
                }

                if (field is ComboBox)
                {
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
