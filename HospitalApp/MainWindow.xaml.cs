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

using Newtonsoft.Json;
using System.IO;
using System.Xml.Linq;

namespace HospitalApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isLoaded = false;
        public ObservableCollection<HospitalApp.Ward> Wards { get; set; } = new ObservableCollection<Ward>();
        public ObservableCollection<HospitalApp.Patient> Patients = new ObservableCollection<Patient>();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            isLoaded = true;
        }

        //startup code
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

      
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

            double numb = sliderWard.Value;
            tbxSliderValue.Text = string.Format("{0:F0}", sliderWard.Value);

        }

        //checking which radio buttom selected 
        private BloodType GetSelectedBloodType()
        {
            if (rbA.IsChecked == true) return BloodType.A;
            if (rbB.IsChecked == true) return BloodType.B;
            if (rbAB.IsChecked == true) return BloodType.AB;
            if (rbO.IsChecked == true) return BloodType.O;

            MessageBox.Show("Please select a blood type!", "Selection Error", MessageBoxButton.OK,
            MessageBoxImage.Warning);


            return BloodType.NotSelected;
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
            int capacity = Convert.ToInt32(sliderWard.Value);
           
            Wards.Add(new HospitalApp.Ward(wardName, capacity));
            MessageBox.Show($"The ward was added successfully! " +
                $"Ward's name: {wardName}  Capacity: {capacity}", "Success", MessageBoxButton.OK);
            
            //refresh listbox
            lbxWardList.ItemsSource = null;
            lbxWardList.ItemsSource = Wards;
            
            //WardsCount
            tblWardList.Text = $"Wards: {Wards.Count}";

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

            BloodType blood = GetSelectedBloodType();
            string name = tbxNamePatient.Text;
            DateTime birthday = Convert.ToDateTime(dpDOB.Text);
                
            Patient pat = new Patient(name, blood, birthday);
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
                if (selectedPatient.Blood == BloodType.A)
                {
                    imgDetails.Source = new BitmapImage(new Uri("images/a.png", UriKind.Relative));
                }
                if (selectedPatient.Blood == BloodType.B)
                {
                    imgDetails.Source = new BitmapImage(new Uri("images/b.png", UriKind.Relative));
                }
                if (selectedPatient.Blood == BloodType.AB)
                {
                    imgDetails.Source = new BitmapImage(new Uri("images/ab.png", UriKind.Relative));
                }
                if (selectedPatient.Blood == BloodType.O)
                {
                    imgDetails.Source = new BitmapImage(new Uri("images/o.png", UriKind.Relative));
                }
            }
            else
            {
                tbkDetailName.Text = null; // Clear list if no ward is selected
            }
        }

        private void tbxNameWard_GotFocus(object sender, RoutedEventArgs e)
        {
            tbxNameWard.Clear();
        }

        private void tbxNamePatient_GotFocus(object sender, RoutedEventArgs e)
        {
            tbxNamePatient.Clear();
        }

        //save Ward and Pation information, json
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //get string of objects - JSON formatted 
            string jsonWards = JsonConvert.SerializeObject(Wards, Formatting.Indented);
            string jsonPatients = JsonConvert.SerializeObject(Patients, Formatting.Indented);

            //write that to file
            using (StreamWriter sw = new StreamWriter(@"C:\Users\user\Downloads\Wards.json"))
            {
                sw.Write(jsonWards);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Users\user\Downloads\Patients.json"))
            {
                sw.Write(jsonPatients);
            }
            MessageBox.Show("Wards and Patients saved in Downloads", "Success", MessageBoxButton.OK);
        }


        //loads json file
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            //connect to a file
            using (StreamReader sr = new StreamReader(@"C:\Users\user\Downloads\Wards.json"))
            {
                //read text
                string jsonWards = sr.ReadToEnd();

                //convert from json to obj
                Wards = JsonConvert.DeserializeObject<ObservableCollection<HospitalApp.Ward>>(jsonWards);

                //refresh the display
                lbxWardList.ItemsSource = null;
                lbxWardList.ItemsSource = Wards;

            }
            using (StreamReader sr = new StreamReader(@"C:\Users\user\Downloads\Wards.json"))
            {
                //read text
                string jsonPatients = sr.ReadToEnd();

                //convert from json to obj
                Patients = JsonConvert.DeserializeObject<ObservableCollection<HospitalApp.Patient>> (jsonPatients);

                //refresh the display
                lbxPatientList.ItemsSource = null;
                lbxPatientList.ItemsSource = Patients;

                MessageBox.Show("Wards and Patients were loaded", "Success", MessageBoxButton.OK);

            }

        }
    }
}
