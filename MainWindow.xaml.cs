﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    }
}
