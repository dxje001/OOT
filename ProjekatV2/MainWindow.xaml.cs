using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace ProjekatV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

   
    public partial class MainWindow : Window
    {
        private ObservableCollection<Apoteka> apoteke = new ObservableCollection<Apoteka>();

        public MainWindow()
        {
            InitializeComponent();

            Import("C:/Users/duxaje/source/repos/ProjekatV2/ProjekatV2/bin/Debug/Apoteke.txt");
            //Import("C: /Users/duxaje/Desktop/Apoteke.txt");
            
            apotekaDataGrid.ItemsSource = apoteke;

            Export("C:/Users/duxaje/source/repos/ProjekatV2/ProjekatV2/bin/Debug/Apoteke.txt", String.Join("*", apoteke));
        }
      
        public void Import(string file)
        {
            apoteke.Clear();

            string readText = File.ReadAllText(file);
            

            string[] apotekePodeljene = readText.Split("*");

            for (int i = 0; i < apotekePodeljene.Length; i++)
            {

                string[] linije;
                string[] deloviApoteke;
                string[] deloviLeka;
                string[] deloviRadnika;
                string adresa = "";
                
                string apotekaLinija;
                string radniciLinija;
                string lekoviLinija;
                string[] lekovi;
                string[] radnici;

                linije = apotekePodeljene[i].Split("\n");

                apotekaLinija = linije[0];
                lekoviLinija = linije[1];
                radniciLinija = linije[2];

                deloviApoteke = apotekaLinija.Split(",");

                
                adresa += deloviApoteke[3];
                

          
                string substring = adresa.Substring(0, adresa.Length-1);

                Apoteka a = new Apoteka(Int32.Parse(deloviApoteke[1]), deloviApoteke[0], deloviApoteke[2], deloviApoteke[3]) ;

                lekovi = lekoviLinija.Split("|");
                for (int j = 0; j < lekovi.Length; j++)
                {
                    deloviLeka = lekovi[j].Split(",");
                    Lek l = new Lek(deloviLeka[0], deloviLeka[1], deloviLeka[2], Int32.Parse(deloviLeka[3]));
                    a.Lekovi.Add(l);
                }
                radnici = radniciLinija.Split("|");
                for (int k = 0; k < radnici.Length; k++)
                {
                    string radnikAdresa = "";
                    string substring1 = "";
                    deloviRadnika = radnici[k].Split(",");
                    radnikAdresa += deloviRadnika[4];
                    if (k == (radnici.Length - 1))
                    {
                         substring1 = radnikAdresa.Substring(0, radnikAdresa.Length - 1);
                    }
                    else { substring1 = radnikAdresa;
                    }
                    
                    
                    Radnik r = new Radnik(deloviRadnika[0], deloviRadnika[1], deloviRadnika[2], Int64.Parse(deloviRadnika[3]), deloviRadnika[4]);
                    a.Radnici.Add(r);
                }
                adresa = null;
                
                apoteke.Add(a);

            }
        }
            public void Export(string file, string txt)
            {
                StreamWriter sw = null;
                try
                {
                    sw = new StreamWriter(file);
                    sw.Write(txt);

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace);
                    MessageBox.Show("ERROR");
                }
                finally
                {
                    try { sw.Close(); } catch (Exception e) { }

                }
            }

        private void apotekaDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (apotekaDataGrid.SelectedIndex != -1)
            {


                lekoviDataGrid.ItemsSource = apoteke[apotekaDataGrid.SelectedIndex].Lekovi;



                radniciDataGrid.ItemsSource = apoteke[apotekaDataGrid.SelectedIndex].Radnici;
                dodaj1.IsEnabled = true;
                dodaj2.IsEnabled = true;

                pretragaPoNazivu.Text = "";
            }
        }

        private void lekoviDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DataContext = (lekoviDataGrid.SelectedItem as Lek);
            izmeni1.IsEnabled = true;

            
        }

        private void pretragaPoNazivu_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (apotekaDataGrid.SelectedIndex != -1)
            {
                List<Lek> pretrazenaLista = new List<Lek>();

                if (pretragaPoNazivu.Text.Equals(""))
                {
                    pretrazenaLista.AddRange(apoteke[apotekaDataGrid.SelectedIndex].Lekovi);
                }
                else
                {
                    foreach (Lek st in apoteke[apotekaDataGrid.SelectedIndex].Lekovi)
                    {

                        if (st.Naziv.ToLower().StartsWith(pretragaPoNazivu.Text.ToLower()))
                        {
                            pretrazenaLista.Add(st);

                        }
                    }
                }

                lekoviDataGrid.ItemsSource = pretrazenaLista.ToList();
            }

        }

        private void radniciDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DataContext = (radniciDataGrid.SelectedItem as Radnik);
            izmeni2.IsEnabled = true;

        }

        private void izmeni1_Click(object sender, RoutedEventArgs e)
        {
            nazivLeka.IsReadOnly = false;
            datumProizvodnjeLeka.IsReadOnly = false;
            datumVazenjaLeka.IsReadOnly=false;
            kolicinaLeka.IsReadOnly = false;

            potvrdiIzmenu1.IsEnabled = true;
        }

        private Lek validacijaLk(string naziv, string datumProizvodnje, string datumVazenja, string kolicina)
        {

            if (naziv == "" || datumProizvodnje == "" || datumVazenja == "" || kolicina=="")
                return null;

            if (!int.TryParse(kolicina, out int kolicinaLk))
                return null;

            if (kolicinaLk <= 0)
                return null;


            return new Lek(naziv, datumProizvodnje, datumVazenja, kolicinaLk);
        }

        private Radnik validacijaRa(string ime, string prezime, string datum, string jmbg, string pathSlika)
        {

            if (ime == "" || prezime == "" || datum == "" || jmbg == "" || pathSlika == "")
                return null;

            if (!Int64.TryParse(jmbg, out Int64 jmbgRa))
                return null;

            if (jmbgRa <= 0)
                return null;


            return new Radnik(ime, prezime, datum, jmbgRa, pathSlika);
        }

        private void dodaj1_Click(object sender, RoutedEventArgs e)
        {
            nazivLeka.Text = datumVazenjaLeka.Text = datumProizvodnjeLeka.Text = kolicinaLeka.Text = "";
            lekoviDataGrid.SelectAllCells();
            lekoviDataGrid.SelectedItem = null;

            nazivLeka.IsReadOnly = false;
            datumProizvodnjeLeka.IsReadOnly = false;
            datumVazenjaLeka.IsReadOnly = false;
            kolicinaLeka.IsReadOnly = false;
            potvrdiDodavanje.IsEnabled = true;
            
        }

        private bool Dodaj(Lek l)
        {

                if (l == null)
                {
                    return false;
                }
                else
                {
                    foreach (Lek lk in apoteke[apotekaDataGrid.SelectedIndex].Lekovi)
                    {
                        if (lk.Naziv == l.Naziv)
                        {
                            return false;
                        }
                    }
                    apoteke[apotekaDataGrid.SelectedIndex].Lekovi.Add(l);
                    return true;
                }

            
        }

        private bool DodajRa(Radnik r)
        {
           
            
                if (r == null)
                {
                    return false;
                }
                else
                {
                    foreach (Radnik ra in apoteke[apotekaDataGrid.SelectedIndex].Radnici)
                    {
                        if (ra.Ime == r.Ime)
                        {
                            return false;
                        }
                    }
                    apoteke[apotekaDataGrid.SelectedIndex].Radnici.Add(r);
                    return true;
                }
            
        }


        private void potvrdiIzmenu1_Click(object sender, RoutedEventArgs e)
        {
            if ((lekoviDataGrid.SelectedItem as Lek) != null)
            {
                Lek novaStavka = validacijaLk(nazivLeka.Text, datumProizvodnjeLeka.Text, datumVazenjaLeka.Text, kolicinaLeka.Text.ToString());
                if (novaStavka == null)
                {
                    MessageBox.Show("Loša validacija!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    int index = lekoviDataGrid.SelectedIndex;
                    apoteke[apotekaDataGrid.SelectedIndex].Lekovi.RemoveAt(index);
                    apoteke[apotekaDataGrid.SelectedIndex].Lekovi.Insert(index, novaStavka);
                    MessageBox.Show("Uspešno izmenjen lek!", "Uspešna operacija", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Lek nije selektovan!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            Export("../Apoteke.txt", String.Join("*", apoteke));

            Import("../Apoteke.txt");
        }

        private void potvrdiDodavanje_Click(object sender, RoutedEventArgs e)
        {
            bool uspesno = true;



            Lek l = validacijaLk(nazivLeka.Text, datumVazenjaLeka.Text, datumProizvodnjeLeka.Text, kolicinaLeka.Text);

            if (Dodaj(l))
            {
                uspesno = true;
            }
            else
            {
                uspesno = false;
            }





            if (uspesno)
                MessageBox.Show("Uspešno dodavanje leka!", "Uspešna operacija", MessageBoxButton.OK, MessageBoxImage.Information);

            else
                MessageBox.Show("Neuspešno dodavanje", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);

            nazivLeka.Text = datumVazenjaLeka.Text = datumProizvodnjeLeka.Text = kolicinaLeka.Text = "";

            Export("../Apoteke.txt", String.Join("*", apoteke));

            Import("../Apoteke.txt");
        }



        private void pretragaPoImenuIPrezimenu_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(apotekaDataGrid.SelectedIndex!=-1)
            {
                List<Radnik> pretrazenaLista = new List<Radnik>();

                if (pretragaPoImenuIPrezimenu.Text.Equals(""))
                {
                    pretrazenaLista.AddRange(apoteke[apotekaDataGrid.SelectedIndex].Radnici);
                }
                else
                {
                    foreach (Radnik st in apoteke[apotekaDataGrid.SelectedIndex].Radnici)
                    {

                        if (st.Ime.ToLower().StartsWith(pretragaPoImenuIPrezimenu.Text.ToLower()) || st.Prezime.ToLower().StartsWith(pretragaPoImenuIPrezimenu.Text.ToLower()))
                        {
                            pretrazenaLista.Add(st);

                        }
                    }
                }

                radniciDataGrid.ItemsSource = pretrazenaLista.ToList();
            }
        }

        private void izmeni2_Click(object sender, RoutedEventArgs e)
        {
            ime.IsReadOnly = false;
            prezime.IsReadOnly = false;
            datumPocetkaRada.IsReadOnly = false;
            jmbg.IsReadOnly = false;
            radnikSlika.IsEnabled = true;

            potvrdiIzmenu2.IsEnabled = true;
        }

        private void potvrdiIzmenu2_Click(object sender, RoutedEventArgs e)
        {
            if ((apotekaDataGrid.SelectedItem as Apoteka) != null)
            {
                if ((radniciDataGrid.SelectedItem as Radnik) != null)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();

                    bool? response = openFileDialog.ShowDialog();
                    if (response == true)
                    {

                        string filePath = openFileDialog.SafeFileName;
                        string putanjal = "C:/Users/duxaje/source/repos/ProjekatV2/ProjekatV2/SlikeRadnici/" + filePath;

                        //  apoteke[apotekaDataGrid.SelectedIndex].Radnici[radniciDataGrid.SelectedIndex].PathSlika = "C:/Users/duxaje/source/repos/ProjekatV2/ProjekatV2/SlikeRadnici/" + filePath;




                        Radnik novaStavka = validacijaRa(ime.Text, prezime.Text, datumPocetkaRada.Text, jmbg.Text, putanjal);
                        if (novaStavka == null)
                        {
                            MessageBox.Show("Loša validacija!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            int index = radniciDataGrid.SelectedIndex;
                            apoteke[apotekaDataGrid.SelectedIndex].Radnici.RemoveAt(index);
                            apoteke[apotekaDataGrid.SelectedIndex].Radnici.Insert(index, novaStavka);
                            MessageBox.Show("Uspešno izmenjen radnik!", "Uspešna operacija", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Radnik nije selektovan!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    Export("../Apoteke.txt", String.Join("*", apoteke));

                    Import("../Apoteke.txt");
                }
            }
        }

       

        private void dodaj2_Click(object sender, RoutedEventArgs e)
        {
            ime.Text = prezime.Text = datumPocetkaRada.Text = jmbg.Text = "";
            radniciDataGrid.SelectAllCells();
            radniciDataGrid.SelectedItem = null;

            ime.IsReadOnly = false;
            prezime.IsReadOnly = false;
            datumPocetkaRada.IsReadOnly = false;
            jmbg.IsReadOnly = false;
            radnikSlika.IsEnabled = true;
            potvrdiDodavanje2.IsEnabled = true;
            

            
        }

        private void potvrdiDodavanje2_Click(object sender, RoutedEventArgs e)
        {
            bool uspesno = true;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            bool? response = openFileDialog.ShowDialog();
            if (response == true)
            {

                string filePath = openFileDialog.SafeFileName;
                string putanjal = "C:/Users/duxaje/source/repos/ProjekatV2/ProjekatV2/SlikeRadnici/" + filePath;


                Radnik r = validacijaRa(ime.Text, prezime.Text, datumPocetkaRada.Text, jmbg.Text, putanjal);

                if (DodajRa(r))
                {
                    uspesno = true;
                }
                else
                {
                    uspesno = false;
                }





                if (uspesno)
                    MessageBox.Show("Uspešno dodavanje radnika!", "Uspešna operacija", MessageBoxButton.OK, MessageBoxImage.Information);

                else
                    MessageBox.Show("Neuspešno dodavanje", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);

                ime.Text = prezime.Text = datumPocetkaRada.Text = jmbg.Text = "";
                radnikSlika.Source = null;

                Export("../Apoteke.txt", String.Join("*", apoteke));

                Import("../Apoteke.txt");
            }
        }

       




        
    }
}
