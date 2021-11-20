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
            cn = new cnOltas(); //oltás osztály példáyosítása
            initPatientList(); //az egyes komponensek inicializáló metódusa
            initVaccineList();
            initPhysicianList();
            initHospList();
            initSumList();
            
            
        }

        public void initHospList()
        {
            cbKorhazdata.ItemsSource = cn.Institutes.ToList();
            //meghatározom, hogy a xaml-ban található ComboBox adatai honnan származnak 
           

        }

        public void initPatientList()
        {
            dgPatients.Visibility = Visibility.Visible; //az alapból összecsukott DataGrid megjelenítése
            dgPatients.ItemsSource = cn.Patients.ToList();
            cbPacienszip.ItemsSource = cn.Locs.ToList();

        }

        public void initVaccineList()
        {
            dgVaccines.Visibility = Visibility.Visible;
            //VakcinaDataGrid adatok forrása
            dgVaccines.ItemsSource = cn.Vaccines.ToList();
        }

        public void initPhysicianList()
        {
            dgPhysicians.Visibility = Visibility.Visible;
            dgPhysicians.ItemsSource = cn.Physicians.ToList();
        }

        public void initSumList()
        {
            dgSum.Visibility = Visibility.Visible;
            dgSum.ItemsSource = cn.Oltotts.ToList();
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
            initPatientList(); //frissítjük a listát, h lássuk az eredményt
        }

        private void btHozzaadVakcina_Click(object sender, RoutedEventArgs e)
        {
            cn.Database.EnsureCreated(); //ugyanaz a folyamat, mint feljebb
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
            cn.Database.EnsureCreated(); //folyamatot lásd btFelveszPaciens_Click
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
            //az app kikapcsolása forrás: órai anyag
        }

        private void btTorolPaciens_Click(object sender, RoutedEventArgs e)
        {

                var delpat = dgPatients.SelectedItem as Patient;
                MessageBox.Show("Törölni fogja a/az " + delpat.name + " nevű pácienst!", "Info", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //betöltjük egy változóba a kiválaszott sor referenciáját


                cn.Patients.Remove(delpat); //kitöröljük a listából a kiváalszott sort
                cn.SaveChanges(); //elmentjük a változásokat

                initPatientList(); //frissítés

            //örülünk

         
          
        }

        private void mi_RefreshClick(object sender, RoutedEventArgs e)
        {
            //a menün található frissítés gomb inicializáló metódusokat fog lefuttatni
            initPatientList();
            initVaccineList();
            initPhysicianList();
            initHospList();
            initSumList();
        }

       

        private void cbKorhazdata_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // kiválasztunk egy adatsort a comboboxból
            var khn = cbKorhazdata.SelectedItem as Institute;
            // betöltjük a referenciáját egy változóba
            if (khn == null) return; //ha nem sikerült, kiszállunk
            tbKorhazname.Text = khn.instituteName;//a referencia megfelelő tagját a kijelölt TextBoxba másoljuk
            tbKorhazcim.Text = khn.zip.ToString();// szintén
        }

        private void dgPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //lásd private void cbKorhazdata_SelectionChanged(...)
            var patient = dgPatients.SelectedItem as Patient;
            if (patient == null) return;
            tbName.Text = patient.name;
            tbTaj.Text = patient.id.ToString();
        }

        private void btTorolVakcina_Click(object sender, RoutedEventArgs e)
        {
            //lásd private void btTorolPaciens_Click()
            var delvacc = dgVaccines.SelectedItem as Vaccine;
            MessageBox.Show("Törölni fogja a/az " + delvacc.serial + " sorozatszámú oltóanyagot!", "Info", MessageBoxButton.OK, MessageBoxImage.Exclamation); 

            cn.Vaccines.Remove(delvacc);
            cn.SaveChanges();

            initVaccineList();
        }

        private void dgVaccines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //lásd korábban
            var vaccine = dgVaccines.SelectedItem as Vaccine;
            if (vaccine == null) return;
            tbVaccineName.Text = vaccine.name;
            tbSerial.Text = vaccine.serial;
        }

      

        private void btTorolOrvos_Click(object sender, RoutedEventArgs e)
        {
            //lásd korábban
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

        private void btHozzaadOltottak_Click(object sender, RoutedEventArgs e) //
        {
            //itt is betöltöm az textboxokból begyűjtött változókat és hozzáadom őket az Oltott adatbázishoz
            cn.Database.EnsureCreated();
            Oltott o = new Oltott();

            try // ide kellett volna csinálni egy if/else vizsgálatot, hogy csak nem üres mezőkből lehessen adatot bekérni
            {
                o.patient = tbName.Text; // a különböző mezők értékei
                o.taj = int.Parse(tbTaj.Text);
                o.oltanyag = tbVaccineName.Text;
                o.serialnumber = tbSerial.Text;
                DateTime now = DateTime.Now;
                o.time = now;
                o.orvosnev = tbDrName.Text;
                o.orvospecset = int.Parse(tbPecset.Text);
                o.intezmeny = tbKorhazname.Text;

                cn.Oltotts.Add(o);
                cn.SaveChanges();
                MessageBox.Show(o.patient + " (" + o.taj + ")-t sikeresen beoltották.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            

        }

        private void dgSum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
           var szum = dgSum.SelectedItem as Oltott;
            if (szum == null) return;
           /* var query = physician.dr_name;
            tbPecset.Text = physician.pecsetszam.ToString();
           
            */

        }

        private void btKivalasztOltottak_Click(object sender, RoutedEventArgs e)
        {
           //ide szerettem volna még csinálni egy Query-t amivel rá tudok keresni páciensre, hogy kilistázza, hány oltást kapott, és mikor, de erre már nem volt időm :(
        }
    }
}
