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
    public partial class WPF_CarDealerManager : Window
    {

        CarService service = new CarService();
        public WPF_CarDealerManager()
        {
            InitializeComponent();
            var getList = service.GetList();
            List<CarDealerClass> display = new List<CarDealerClass>();

            foreach (var item in getList) {
                display.Add(new CarDealerClass(item));
            }
            this.gridCars.ItemsSource = display;

        }
    }
}
