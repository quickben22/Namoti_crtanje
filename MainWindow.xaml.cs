
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Security.Principal;
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

namespace Crtanje_Namota
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        Podaci p = new Podaci();
        SQLiteDatabase db;
        List<string> popis = new List<string>();
        List<string> boxovi;
        List<ComboBox> comb;
        List<TextBox> tbox;
        public MainWindow()
        {

            p.PropertyChanged += Computer_PropertyChanged;
            p.broj_namota_popis = new ObservableCollection<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            p.broj_aks_popis = new ObservableCollection<int> { 1, 2 };
            p.broj_kan_popis = new ObservableCollection<int> { 0, 1, 2, 3 };
            if (!File.Exists("bazica"))
            {
                //SQLiteConnection.CreateFile("bazica.db");
                p.stvori(db, p);
            }
            else
            {
                db = new SQLiteDatabase();
            }

            this.DataContext = p;
            InitializeComponent();
        }

        List<Grid> namoti;
        List<TextBlock> sekcTB;
        List<TextBox> sekcT;
        List<TextBlock> kanTB;
        List<TextBox> kanT;
        bool skrivi = false;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            p.log_book("", "Upalio", db);
            boxovi = new List<string>
            {
                "T1","T2","T3","C4","T4a","T5","C6","C7","T8","T9","sekcije1a","kanal1a","T12","T13","T14","C15","C16",
                "T17","T18","sekcije2a","kanal2a","T21","T22","C23","C24","T25","T26","sekcije3a","kanal3a","T29","T30","C31","C32","T33",
                "T34","sekcije4a","kanal4a","T37","T38","C39","C40","T41","T42","sekcije5a","kanal5a",
                "T45","T46","C47","C48","T49","T50","sekcije6a","kanal6a","T53","T54","C55","C56","T57","T58","sekcije7a","kanal7a","T61",
                "T62","C63","C64","T65","T66","sekcije8a","kanal8a","T69","T70","C71","C72","T73","T74","sekcije9a","kanal9a","T77","T78",
                "C79","C80","T81","T82","sekcije10a","kanal10a","T85","T86","T87"
            };

            comb = new List<ComboBox>
            {
              C4,C6,C7,C15,C16,C23,C24,C31,C32,C39,C40,C47,C48,C55,C56,C63,C64,C71,C72,C79,C80
            };
            tbox = new List<TextBox>
            {
                T1,T2,T3,T4a,T5,T8,T9,sekcije1a,kanal1a,T12,T13,T14,T17,T18,sekcije2a,kanal2a,T21,T22,T25,T26,sekcije3a,kanal3a,T29,T30,T33,T34,sekcije4a,kanal4a,T37,T38,T41,T42,sekcije5a,kanal5a,
                T45,T46,T49,T50,sekcije6a,kanal6a,T53,T54,T57,T58,sekcije7a,kanal7a,T61,T62,T65,T66,sekcije8a,kanal8a,T69,T70,T73,T74,sekcije9a,kanal9a,T77,T78,T81,T82,sekcije10a,kanal10a,T85,T86,T87
 
            };


            namoti = new List<Grid> { Namot1, Namot2, Namot3, Namot4, Namot5, Namot6, Namot7, Namot8, Namot9, Namot10 };
            sekcTB = new List<TextBlock> { sekcije1, sekcije2, sekcije3, sekcije4, sekcije5, sekcije6, sekcije7, sekcije8, sekcije9, sekcije10, };
            sekcT = new List<TextBox> { sekcije1a, sekcije2a, sekcije3a, sekcije4a, sekcije5a, sekcije6a, sekcije7a, sekcije8a, sekcije9a, sekcije10a, };
            kanTB = new List<TextBlock> { kanal1, kanal2, kanal3, kanal4, kanal5, kanal6, kanal7, kanal8, kanal9, kanal10, };
            kanT = new List<TextBox> { kanal1a, kanal2a, kanal3a, kanal4a, kanal5a, kanal6a, kanal7a, kanal8a, kanal9a, kanal10a, };
            skrivi = true;
            skrivanje();

            DataTable tabla;
            String query = "select Projekt from Tablica;";
            tabla = db.GetDataTable(query);

            foreach (DataRow r in tabla.Rows)
            {
                popis.Add(r["Projekt"].ToString());
            }

            combo();
        }

        private void combo()
        {
            ProjektC.ItemsSource = new ObservableCollection<string>(popis);

        }

        private void pazi()
        {
            double[] pom = kontrola();
            double sirina = pom[0];
            double visina = pom[1];

            if (visina != p.visina_prozora) Visina.Foreground = Brushes.Red;
            else Visina.Foreground = Brushes.Green;
            if (sirina * 2 != p.sirina_prozora) Sirina.Foreground = Brushes.Red;
            else Sirina.Foreground = Brushes.Green;
            Visina.Text = visina.ToString();
            Sirina.Text = (sirina * 2).ToString();
        }

        private double[] kontrola()
        {
            double sirina = p.razmak_lijevo;

            int j = -1;
            for (int i = 0; i < p.broj_namota; i++)
            {
                sirina += p.razmak_desno[i] + p.sirina_namota[i];
                if (p.centriraj[i]) j = i;

            }
            double visina = 0;
            if (j != -1) visina = p.visina_namota[j] + p.razmak_gore + p.razmak_dole;

            return new double[] { sirina, visina };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            double[] pom = kontrola();
            double sirina = pom[0];
            double visina = pom[1];


            string messageBoxText = "";

            if (visina != p.visina_prozora) messageBoxText = "Visina prozora se ne slaže sa unesenim dimenzijama!!! Želite li nastaviti?";
            if (sirina * 2 != p.sirina_prozora) messageBoxText = "Širina prozora se ne slaže sa unesenim dimenzijama!!! Želite li nastaviti?";

            string caption = "Namoti";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Error;

            if (messageBoxText.Length > 0)
            {
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                // Process message box results
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        {
                            Slika dlg = new Slika(p, ProjektC.Text, db);
                            dlg.ShowDialog();
                            break;
                        }
                    case MessageBoxResult.No:
                        {
                            break;
                        }
                }
            }
            else
            {
                Slika dlg = new Slika(p, ProjektC.Text, db);
                dlg.ShowDialog();
            }


        }

        void Computer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            skrivanje();
        }

        private void skrivanje()
        {
            if (skrivi)
            {
                bool VN_bool = true;
                for (int i = 0; i < p.broj_namota; i++)
                {
                    namoti[i].Visibility = Visibility.Visible;
                    if (p.centriraj[i])
                    {
                        Namot_VN.Visibility = Visibility.Visible;
                        Thickness t = Namot_VN.Margin;
                        t.Left = namoti[i].Margin.Left;
                        Namot_VN.Margin = t;
                        VN_bool = false;
                    }

                    if (p.broj_aks[i] == 2)
                    {
                        sekcT[i].Visibility = Visibility.Visible;
                        sekcTB[i].Visibility = Visibility.Visible;
                    }
                    else
                    {
                        sekcT[i].Visibility = Visibility.Hidden;
                        sekcTB[i].Visibility = Visibility.Hidden;
                    }
                    if (p.broj_kan[i] > 0)
                    {
                        kanT[i].Visibility = Visibility.Visible;
                        kanTB[i].Visibility = Visibility.Visible;
                    }
                    else
                    {
                        kanT[i].Visibility = Visibility.Hidden;
                        kanTB[i].Visibility = Visibility.Hidden;
                    }


                }
                if (VN_bool) Namot_VN.Visibility = Visibility.Hidden;

                for (int i = p.broj_namota; i < 10; i++)
                {
                    namoti[i].Visibility = Visibility.Hidden;
                    sekcT[i].Visibility = Visibility.Hidden;
                    sekcTB[i].Visibility = Visibility.Hidden;
                    kanT[i].Visibility = Visibility.Hidden;
                    kanTB[i].Visibility = Visibility.Hidden;
                }
                pazi();
            }

        }



        private void prazni(int i)
        {
            p.centriraj[i] = false;
            p.broj_kan[i] = 0;
            p.broj_aks[i] = 1;
            p.naziv_namota[i] = "";
            p.razmak_desno[i] = 0;
            p.sirina_namota[i] = 0;
            p.visina_namota[i] = 0;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            skrivanje();
        }

        private void Otvori_cl(object sender, RoutedEventArgs e)
        {
            if (ProjektC.Text != null)
                if (ProjektC.Text.Length > 0)
                {
                    p.otvori(db, ProjektC.Text, p);

                    p.log_book(ProjektC.Text, "Otvara", db);
                }

        }

        private void Spremi_cl(object sender, RoutedEventArgs e)
        {
            if (ProjektT.Text != null)
                if (ProjektT.Text.Length > 0 && ProjektT.Text.Length < 30)
                {
                    bool postoji = popis.Contains(ProjektT.Text);
                    if (p.spremi(db, p, ProjektT.Text, postoji))
                    {
                        if (!postoji)
                        {
                            popis.Add(ProjektT.Text);
                            combo();
                        }
                        p.log_book(ProjektT.Text, "Sprema", db);
                    }
                }
        }



        private void Brisi_cl(object sender, RoutedEventArgs e)
        {

            if (ProjektC.Text.Length > 0 && ProjektC.SelectedIndex != -1)
            {
                string messageBoxText = "Sigurno želite izbrisati projekt:" + ProjektC.Text + "?";

                string caption = "Namoti";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                // Process message box results
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        {
                            try
                            {
                                db.Delete("Tablica", String.Format("Projekt = {0}", "\"" + ProjektC.Text + "\""));
                                p.log_book(ProjektC.Text, "Brisanje", db);
                                popis.RemoveAt(popis.IndexOf(ProjektC.Text));
                                combo();

                            }
                            catch
                            {
                                MessageBox.Show("Neuspjeh");
                            }
                            break;
                        }
                    case MessageBoxResult.No:
                        {
                            break;
                        }
                }

            }


        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            skrivanje();

        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Return && (Keyboard.Modifiers & ModifierKeys.Shift) == (ModifierKeys.Shift))
            {
                enter_enter(sender, -1);
                //Console.WriteLine("aaaaa");
            }
            else if (e.Key == Key.Return)
            {
                enter_enter(sender, 1);

            }
        }

        private void enter_enter(object sender, int gdje)
        {

            int i = -1;
            foreach (string box in boxovi)
            {
                i++;



                if (sender.GetType().Name == "ComboBox")
                {

                    if (((ComboBox)sender).Name == box)
                    {
                        i = i + gdje;
                        if (i >= boxovi.Count)
                            i = 0;
                        else if (i < 0)
                            i = boxovi.Count - 1;
                        break;
                    }
                }
                else if (sender.GetType().Name == "TextBox")
                {

                    if (((TextBox)sender).Name == box)
                    {
                        i = i + gdje;
                        //Console.WriteLine(i);
                        if (i >= boxovi.Count)
                            i = 0;
                        else if (i < 0)
                            i = boxovi.Count - 1;
                        break;
                    }
                }
            }
            int j = 1;
            // MessageBox.Show(((ComboBox)sender).Name);
            //MessageBox.Show(i.ToString());
            if (i >= 0)
            {




                while (j == 1)
                {
                    //Console.WriteLine("a"+i.ToString());
                    TextBox tex = null;
                    ComboBox com = null;
                    foreach (TextBox bo in tbox)
                    {
                        if (bo.Name == boxovi[i])
                        {
                            tex = bo;
                        }
                    }

                    foreach (ComboBox bo in comb)
                    {
                        if (bo.Name == boxovi[i])
                        {
                            com = bo;
                        }
                    }


                    if (tex != null)
                    {
                        //     MessageBox.Show(tex.Name);

                        if (((Grid)tex.Parent).Visibility == Visibility.Visible && tex.Visibility == Visibility.Visible && tex.IsEnabled == true)
                        {
                            tex.SelectAll();
                            tex.Focus();
                            break;
                        }
                        else
                        {
                            i = i + gdje;
                            if (i >= boxovi.Count)
                                i = 0;
                            else if (i < 0)
                                i = boxovi.Count - 1;
                        }
                    }


                    if (com != null)
                    {
                        //       MessageBox.Show(com.Name);
                        if (((Grid)com.Parent).Visibility == Visibility.Visible && com.Visibility == Visibility.Visible && com.IsEnabled == true)
                        {
                            com.IsDropDownOpen = true;
                            com.Focus();
                            break;
                        }
                        else
                        {
                            i = i + gdje;
                            if (i >= boxovi.Count)
                                i = 0;
                            else if (i < 0)
                                i = boxovi.Count - 1;
                        }
                    }



                }
            }

        }

       

        private void T_TextChanged(object sender, TextChangedEventArgs e)
        {
            skrivanje();
        }






    }
}
