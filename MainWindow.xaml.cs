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
        }

       

        private void btFelveszPaciens_Click(object sender, RoutedEventArgs e)
        {
            cn.Database.EnsureCreated(); //ha még nincs tábla, ez a metódus csinál
            Patient p = new Patient(); //példányosítjuk a páciens osztályt

            //https://www.youtube.com/watch?v=vFVNRIqnzkM

            if (!string.IsNullOrEmpty(tbName!.Text)  && !string.IsNullOrEmpty(tbTaj.Text) && !string.IsNullOrEmpty(tbCim!.Text) ) //ha a cím, név, taj nem nulla akkor mehet 
            {
                try
                {
                    p.name = tbName.Text; //a pácins osztályban lévő property-knek adunk értéket
                    p.id = int.Parse(tbTaj.Text);
                    p.zip = int.Parse("6000");
                    p.address = tbCim.Text;

                    cn.Patients.Add(p); //hozzáadjuk az értékeket
                    cn.SaveChanges(); //elmentjük az adatbázisba
                    MessageBox.Show("Sikeres betegfelvétel", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    tbName.Text = ""; //mezők kiürítése
                    tbTaj.Text = "";
                    tbCim.Text = "";
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Save error", MessageBoxButton.OK, MessageBoxImage.Error); //kivételkezelés tájékoztat az esetelges hibákról
                }
            }

            else MessageBox.Show("Nincs kitöltve minden mező!", "Save error", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void btHozzaadVakcina_Click(object sender, RoutedEventArgs e)
        {
            cn.Database.EnsureCreated();
            Vaccine v = new Vaccine();
            if (!string.IsNullOrEmpty(tbVaccineName.Text) && !string.IsNullOrEmpty(tbSerial.Text))
            {
                try
                {
                    v.name =tbVaccineName.Text.ToUpper();
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
            else MessageBox.Show("Nincs kitöltve minden mező!", "Save error", MessageBoxButton.OK, MessageBoxImage.Error);

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
        }
    }
}
