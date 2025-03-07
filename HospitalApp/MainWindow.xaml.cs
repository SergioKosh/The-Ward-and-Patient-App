using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace HospitalApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Ward> Wards { get; set; } = new ObservableCollection<Ward>();
        ObservableCollection<HospitalApp.Patient> patients = new ObservableCollection<Patient> ();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        //startup code
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Ward w1 = new HospitalApp.Ward("w1",10);
            Ward w2 = new HospitalApp.Ward("w2", 7);
            Ward w3 = new HospitalApp.Ward("w3", 5);

            Wards.Add(w1);
            Wards.Add(w2);
            Wards.Add(w3);

            lbxWardList.ItemsSource = Wards;
        }

        private void btnAddWard_Click(object sender, RoutedEventArgs e)
        {
            //check if the fields are empty
            if (string.IsNullOrWhiteSpace(tbxNameWard.Text) || sliderWard.Value == 0)
            {
                
                MessageBox.Show("Error: The input cannot be empty!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
                string wardName = tbxNameWard.Text;
                int capacity = Convert.ToInt16(sliderWard.Value);
                Wards.Add(new HospitalApp.Ward(wardName, capacity));
                MessageBox.Show("The ward was added successfully!", "Success", MessageBoxButton.OK);
            
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tbxSliderValue.Text = string.Format("{0:F0}", sliderWard.Value);
        }
    }
}
