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
    /// Interaction logic for WPF_CarDealerManager.xaml
    /// </summary>
    /// <remarks>
    /// This class performs CRUD operations
    /// </remarks>
    public partial class WPF_CarDealerManager : Window
    {
        CarService service = new CarService();
        CarDealerManagementDBEntities context = new CarDealerManagementDBEntities();

        public WPF_CarDealerManager()
        {
            InitializeComponent();

            //For DataGrid to load
            ReloadGrid();

            //Initially disable Update ComboBoxes for edit
            ManageIsFieldEnabled(false, inputPriceUpdate, comboColorsUpdate, comboModelsUpdate, comboConditionUpdate, comboCountryUpdate);


            string arrowPath = "./assets/img/BackArrow.png";
            BitmapImage img = new BitmapImage(new Uri(arrowPath, UriKind.Relative));
            this.arrow.Source = img;

            //Get values from tables
            var getModels = service.GetModels();
            var getColors = service.GetColors();
            var getCondition = service.GetCondition();
            var getCountry = service.GetCountry();


            // FILL MODELS COMBOBOX
            List<string> displayModels = new List<string>();
            foreach (var model in getModels)
                displayModels.Add(model.CAR_MODEL);
            this.comboModels.ItemsSource = displayModels;
            this.comboModelsUpdate.ItemsSource = displayModels;

            // FILL COLORS COMBOBOX
            List<string> displayColors = new List<string>();
            foreach (var color in getColors)
                displayColors.Add(color.COLOR);
            this.comboColors.ItemsSource = displayColors;
            this.comboColorsUpdate.ItemsSource = displayColors;

            // FILL CONDITION COMBOBOX
            List<string> displayCondition = new List<string>();
            foreach (var condition in getCondition)
                displayCondition.Add(condition.CONDITION);
            this.comboCondition.ItemsSource = displayCondition;
            this.comboConditionUpdate.ItemsSource = displayCondition;

            // FILL COUNTRY COMBOBOX
            List<string> displayCountry = new List<string>();
            foreach (var country in getCountry)
                displayCountry.Add(country.COUNTRY);
            this.comboCountry.ItemsSource = displayCountry;
            this.comboCountryUpdate.ItemsSource = displayCountry;
        }

        /// <summary>
        /// Adds new row to DB
        /// </summary>
        private void addCarBtn_Click(object sender, RoutedEventArgs e)
        {
            //Validation for empty fields
            InputDataValidator(inputPrice, comboModels, comboColors, comboCondition, comboCountry);

            //Set up a new object
            TB_CAR carObj = new TB_CAR()
            {
                ID_CAR_MODEL = context.TB_CAR_MODEL.Where(model => model.CAR_MODEL == this.comboModels.Text).Select(model => model.ID_CAR_MODEL).FirstOrDefault(),
                ID_CAR_COLOR = context.TB_CAR_COLOR.Where(color => color.COLOR == this.comboColors.Text).Select(color => color.ID_CAR_COLOR).FirstOrDefault(),
                ID_CAR_CONDITION = context.TB_CAR_CONDITION.Where(condition => condition.CONDITION == this.comboCondition.Text).Select(condition => condition.ID_CAR_CONDITION).FirstOrDefault(),
                ID_CAR_COUNTRY = context.TB_CAR_COUNTRY.Where(country => country.COUNTRY == this.comboCountry.Text).Select(country => country.ID_CAR_COUNTRY).FirstOrDefault(),
                CAR_PRICE = int.Parse(this.inputPrice.Text)
            };

            // Add new object to DB
            context.TB_CAR.Add(carObj);
            context.SaveChanges();

            //Reset values after adding
            ResetComboBoxText(comboModels, comboCountry, comboColors, comboCondition);
            this.inputPrice.Text = "";

            //Reload DataGrid
            ReloadGrid();

            ShowInformationMessageBox("Item successfully added!", "Add");
        }


        /// <summary>
        /// Reloads DataGrid
        /// </summary>
        private void ReloadGrid()
        {
            CarService service = new CarService();
            var getList = service.GetList();
            List<CarDealerClass> display = new List<CarDealerClass>();

            foreach (var item in getList)
            {
                display.Add(new CarDealerClass(item));
            }
            this.gridCars.ItemsSource = display;
        }




        /// <summary>
        /// Load selected row to the form
        /// </summary>
        private int? updatingCarID = null;
        private void gridCars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Check for correct selection
            if (this.gridCars.SelectedIndex >= 0 && this.gridCars.SelectedItems.Count >= 0)
            {
                if (this.gridCars.SelectedItems[0].GetType() == typeof(CarDealerClass))
                {
                    CarDealerClass selected = (CarDealerClass)this.gridCars.SelectedItems[0];
                    this.comboModelsUpdate.SelectedItem = selected.CAR_MODEL;
                    this.comboColorsUpdate.Text = selected.CAR_COLOR;
                    this.comboConditionUpdate.Text = selected.CAR_CONDITION;
                    this.comboCountryUpdate.Text = selected.CAR_COUNTRY;
                    this.inputPriceUpdate.Text = selected.CAR_PRICE.ToString();
                    this.updatingCarID = selected.ID_CAR;

                    ManageIsFieldEnabled(true, inputPriceUpdate, comboColorsUpdate, comboModelsUpdate, comboConditionUpdate, comboCountryUpdate);
                }
            }
        }


        /// <summary>
        /// Update selected row in DB and DataGrid
        /// </summary>
        private void UpdateCars(object sender, RoutedEventArgs e)
        {
            InputDataValidator(inputPriceUpdate, comboModelsUpdate, comboCountryUpdate, comboConditionUpdate, comboColorsUpdate);

            CarDealerManagementDBEntities context = new CarDealerManagementDBEntities();

            if (updatingCarID == null) {
                ShowInformationMessageBox("Please select a row from the DataGrid to update", "Row not selected");
                return;
            }
            var result = from r in context.TB_CAR where r.ID_CAR == this.updatingCarID select r;

            TB_CAR obj = result.SingleOrDefault();

            //Check for null
            if (obj != null)
            {
                var id = obj.ID_CAR;
                obj.ID_CAR_COLOR = context.TB_CAR_COLOR.Where(color => color.COLOR == this.comboColorsUpdate.Text).Select(color => color.ID_CAR_COLOR).FirstOrDefault();
                obj.ID_CAR_CONDITION = context.TB_CAR_CONDITION.Where(cond => cond.CONDITION == this.comboConditionUpdate.Text).Select(cond => cond.ID_CAR_CONDITION).FirstOrDefault();
                obj.ID_CAR_COUNTRY = context.TB_CAR_COUNTRY.Where(country => country.COUNTRY == this.comboCountryUpdate.Text).Select(country => country.ID_CAR_COUNTRY).FirstOrDefault();
                obj.ID_CAR_MODEL = context.TB_CAR_MODEL.Where(model => model.CAR_MODEL == this.comboModelsUpdate.Text).Select(model => model.ID_CAR_MODEL).FirstOrDefault();
                obj.CAR_PRICE = decimal.Parse(this.inputPriceUpdate.Text);
                context.SaveChanges();

                //Reset values
                ResetComboBoxText(comboModelsUpdate, comboCountryUpdate, comboColorsUpdate, comboConditionUpdate);
                this.inputPriceUpdate.Text = "";
                updatingCarID = null;
                ManageIsFieldEnabled(false, inputPriceUpdate, comboColorsUpdate, comboModelsUpdate, comboConditionUpdate, comboCountryUpdate);

                ReloadGrid();

                ShowInformationMessageBox("Item successfully updated!", "Update");
            }
        }

        /// <summary>
        /// Removes selected row from the database
        /// </summary>
        private void DeleteCar(object sender, RoutedEventArgs e)
        {
            if(updatingCarID == null)
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
                var result = from d in context.TB_CAR
                             where d.ID_CAR == this.updatingCarID
                             select d;

                TB_CAR obj = result.SingleOrDefault();

                if (obj != null)
                {
                    context.TB_CAR.Remove(obj);
                    context.SaveChanges();

                    ResetComboBoxText(comboModelsUpdate, comboCountryUpdate, comboColorsUpdate, comboConditionUpdate);
                    this.inputPriceUpdate.Text = "";
                    updatingCarID = null;
                    ReloadGrid();
                }
            }
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

        /// <summary>
        /// Reset ComboBox Text Value to an empty string
        /// </summary>
        private void ResetComboBoxText(params ComboBox[] fields) {
            foreach (ComboBox combo in fields) combo.Text = "";
        }


        /// <summary>
        /// Basic validation for input fields (not empty, is integer, check range, select all fields) 
        /// </summary>
        private bool InputDataValidator(TextBox price, params ComboBox[] fields)
        {
            //Check for empty input price
            if (price.Text.Trim() == "")
            {
                ShowInformationMessageBox("Must select all fiels!", "Empty fields");
                return false;
            }

            //Check for input price - not an int
            int inputPriceParsedToInt;
            bool priceParseResult = int.TryParse(price.Text, out inputPriceParsedToInt);
            if (!priceParseResult) //not parsed
            {
                ShowInformationMessageBox("Must enter integer for price!", "Wrong input");
                return false;
            }
            //Check for input price -  check range
            else if (inputPriceParsedToInt <= 0 || inputPriceParsedToInt > 10000000)
            {
                ShowInformationMessageBox("Price must be between 1 - 10mln", "Price out of range");
                return false;
            }

            foreach (ComboBox combo in fields)
            {
                if (combo.Text.Trim() == "")
                {
                    ShowInformationMessageBox("Must select all fiels!", "Empty fields");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Change field's Enabled Property 
        /// </summary>
        private void ManageIsFieldEnabled(bool state, params Control[] fields)
        {
            foreach (Control field in fields)
                field.IsEnabled = state;
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
    }
}
