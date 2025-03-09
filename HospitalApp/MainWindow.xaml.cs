using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
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
        private bool isLoaded = false;
        public ObservableCollection<HospitalApp.Ward> Wards { get; set; } = new ObservableCollection<Ward>();
        public ObservableCollection<HospitalApp.Patient> patients = new ObservableCollection<Patient>();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        //startup code
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            
            isLoaded = true;
      
        }

        private void lbxWardList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxWardList.SelectedItem is Ward selectedWard)
            {
                lbxPatientList.ItemsSource = selectedWard.Patients;
            }
            else
            {
                lbxPatientList.ItemsSource = null; // Clear list if no ward is selected
            }
        }

        //slider, but stil not working
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!isLoaded) return;


            tbxSliderValue.Text = string.Format("{0:F0}", sliderWard.Value);

        }

        //checking which radio buttom selected 
        private string GetSelectedBloodType()
        {
            if (rbA.IsChecked == true) return "A";
            if (rbB.IsChecked == true) return "B";
            if (rbAB.IsChecked == true) return "AB";
            if (rbO.IsChecked == true) return "O";

            MessageBox.Show("Please select a blood type!", "Selection Error", MessageBoxButton.OK, 
            MessageBoxImage.Warning);
            
            return string.Empty;
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
            
            //refresh listbox
            lbxWardList.ItemsSource = null;
            lbxWardList.ItemsSource = Wards;

        }

        

        private void btnAddPatient_Click(object sender, RoutedEventArgs e)
        {
            Ward selectedWard = lbxWardList.SelectedItem as Ward;

            if (selectedWard == null)
            {
                MessageBox.Show("Ward is not selected");
                return;
            }
            else if (selectedWard.Patients.Count == selectedWard.Capacity)
            {
                MessageBox.Show("The ward is full, select another one");
                return;
            }

            string bloodType = GetSelectedBloodType();
            string name = tbxNamePatient.Text;
            DateTime birthday = Convert.ToDateTime(dpDOB.Text);
                
            Patient pat = new Patient(name, bloodType, birthday);
            selectedWard.Patients.Add(pat);
            MessageBox.Show($"Patient {name} has been added to {selectedWard.WardName}.", "Success", MessageBoxButton.OK);
            
            //refresh Patient listbox
            lbxPatientList.ItemsSource = null;
            lbxPatientList.ItemsSource = selectedWard.Patients;

        }


        //display details of a patient
        private void lbxPatientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxPatientList.SelectedItem is Patient selectedPatient)
            {
                tbkDetailName.Text = selectedPatient.Name;

                //display bloddType
                if (selectedPatient.BloodType == "A")
                {
                    imgDetails.Source = new BitmapImage(new Uri("images/a.png", UriKind.Relative));
                }
                if (selectedPatient.BloodType == "B")
                {
                    imgDetails.Source = new BitmapImage(new Uri("images/b.png", UriKind.Relative));
                }
                if (selectedPatient.BloodType == "AB")
                {
                    imgDetails.Source = new BitmapImage(new Uri("images/ab.png", UriKind.Relative));
                }
                if (selectedPatient.BloodType == "O")
                {
                    imgDetails.Source = new BitmapImage(new Uri("images/o.png", UriKind.Relative));
                }
            }
            else
            {
                tbkDetailName.Text = null; // Clear list if no ward is selected
            }
        }
    }
}
