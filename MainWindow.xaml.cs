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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Oltas;

namespace VP_Beadando
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        cnOltas cn;

        public MainWindow()
        {
            InitializeComponent();
            cn = new cnOltas();
            initPatientList();
            initVaccineList();
            initPhysicianList();
            initHospList();
            
        }

        public void initHospList()
        {
            cbKorhazdata.ItemsSource = cn.Institutes.ToList();
           

        }

        public void initPatientList()
        {
            dgPatients.Visibility = Visibility.Visible;
            dgPatients.ItemsSource = cn.Patients.ToList();
            cbPacienszip.ItemsSource = cn.Locs.ToList();

        }

        public void initVaccineList()
        {
            dgVaccines.Visibility = Visibility.Visible;
            dgVaccines.ItemsSource = cn.Vaccines.ToList();
        }

        public void initPhysicianList()
        {
            dgPhysicians.Visibility = Visibility.Visible;
            dgPhysicians.ItemsSource = cn.Physicians.ToList();
        }

       

        private void btFelveszPaciens_Click(object sender, RoutedEventArgs e)
        {
            cn.Database.EnsureCreated(); //ha még nincs tábla, ez a metódus csinál
            Patient p = new Patient(); //példányosítjuk a páciens osztályt

            //https://www.youtube.com/watch?v=vFVNRIqnzkM

            if (!string.IsNullOrEmpty(tbName!.Text) && !string.IsNullOrEmpty(tbTaj.Text) && !string.IsNullOrEmpty(tbCim!.Text)) //ha a cím, név, taj nem nulla akkor mehet 
            {
                try
                {
                    p.name = tbName.Text; //a pácins osztályban lévő property-knek adunk értéket
                    p.id = int.Parse(tbTaj.Text) ;

                    var c = cbPacienszip.SelectedItem as Loc; //adatok kinyerése ComboBoxból
                        if (c == null) return;
                    p.zip = int.Parse(c.zip.ToString());
                    p.address = tbCim.Text;

                    cn.Patients.Add(p); //hozzáadjuk az értékeket
                    cn.SaveChanges(); //elmentjük az adatbázisba
                    MessageBox.Show("Sikeres betegfelvétel", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    tbName.Text = ""; //mezők kiürítése
                    tbTaj.Text = "";
                    tbCim.Text = "";
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error); //kivételkezelés tájékoztat az esetelges hibákról
                }
            }

            else { MessageBox.Show("Nincs kitöltve minden mező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error); }
            initPatientList();
        }

        private void btHozzaadVakcina_Click(object sender, RoutedEventArgs e)
        {
            cn.Database.EnsureCreated();
            Vaccine v = new Vaccine();
            if (!string.IsNullOrEmpty(tbVaccineName.Text) && !string.IsNullOrEmpty(tbSerial.Text))
            {
                try
                {
                    v.name = tbVaccineName.Text.ToUpper();
                    v.serial = tbSerial.Text;
                    cn.Vaccines.Add(v);
                    cn.SaveChanges();
                    MessageBox.Show("Sikeres oltóanyag regisztráció", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    tbVaccineName.Text = "";
                    tbSerial.Text = "";

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else { MessageBox.Show("Nincs kitöltve minden mező!", "Save error", MessageBoxButton.OK, MessageBoxImage.Error); }
            initVaccineList();
        }

        private void btHozzaadOrvos_Click(object sender, RoutedEventArgs e)
        {
            cn.Database.EnsureCreated();
            Physician p = new Physician();

            if (!string.IsNullOrEmpty(tbDrName.Text) && !string.IsNullOrEmpty(tbPecset.Text))
            {
                try
                {
                    p.dr_name = tbDrName.Text;
                    p.pecsetszam = int.Parse(tbPecset.Text);
                    cn.Physicians.Add(p);
                    cn.SaveChanges();
                    MessageBox.Show("Sikeres orvos regisztráció", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    tbDrName.Text = "";
                    tbPecset.Text = "";
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else MessageBox.Show("Nincs kitöltve minden mező!", "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
            initPhysicianList();
        }

        private void mi_CloseClick(object sender, RoutedEventArgs e) //https://youtu.be/fksgm-CR1Z4?t=1097
        {
            Application.Current.Shutdown(); 
        }

        private void btTorolPaciens_Click(object sender, RoutedEventArgs e)
        {

                var delpat = dgPatients.SelectedItem as Patient;
                MessageBox.Show("Törölni fogja a/az " + delpat.name + " nevű pácienst!", "Info", MessageBoxButton.OK, MessageBoxImage.Exclamation);



                cn.Patients.Remove(delpat);
                cn.SaveChanges();

                initPatientList();

         
          
        }

        private void mi_RefreshClick(object sender, RoutedEventArgs e)
        {
            initPatientList();
            initVaccineList();
            initPhysicianList();
            initHospList();
        }

       

        private void cbKorhazdata_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var khn = cbKorhazdata.SelectedItem as Institute;
            if (khn == null) return;
            tbKorhazname.Text = khn.instituteName;
            tbKorhazcim.Text = khn.zip.ToString();
        }

        private void dgPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var patient = dgPatients.SelectedItem as Patient;
            if (patient == null) return;
            tbName.Text = patient.name;
            tbTaj.Text = patient.id.ToString();
        }

        private void btTorolVakcina_Click(object sender, RoutedEventArgs e)
        {
            var delvacc = dgVaccines.SelectedItem as Vaccine;
            MessageBox.Show("Törölni fogja a/az " + delvacc.serial + " sorozatszámú oltóanyagot!", "Info", MessageBoxButton.OK, MessageBoxImage.Exclamation); 

            cn.Vaccines.Remove(delvacc);
            cn.SaveChanges();

            initVaccineList();
        }

        private void dgVaccines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vaccine = dgVaccines.SelectedItem as Vaccine;
            if (vaccine == null) return;
            tbVaccineName.Text = vaccine.name;
            tbSerial.Text = vaccine.serial;
        }

      

        private void btTorolOrvos_Click(object sender, RoutedEventArgs e)
        {
            var deldoc = dgPhysicians.SelectedItem as Physician;
            MessageBox.Show("Törölni fogja a/az " + deldoc.pecsetszam + " pecsétszámú orvost!", "Info", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            cn.Physicians.Remove(deldoc);
            cn.SaveChanges();

            initPhysicianList();
        }

        private void dgPhysicians_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var physician = dgPhysicians.SelectedItem as Physician;
            if (physician == null) return;
            tbDrName.Text = physician.dr_name;
            tbPecset.Text = physician.pecsetszam.ToString();
        }
    }
}
