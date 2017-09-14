using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Printing;
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

namespace Crtanje_Namota
{
    /// <summary>
    /// Interaction logic for Slika.xaml
    /// </summary>
    public partial class Slika : MetroWindow
    {
        Podaci P;
        Polygon[] strele;
        Grid[] vertikale;
        Grid[] horizontale;
        string projekt;
        Button bt;
        SQLiteDatabase db;
        public Slika(Podaci a, string b, SQLiteDatabase c)
        {
            db = c;
            projekt = b;
            P = a;
            this.DataContext = P;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            strele = new Polygon[] { arrow1, arrow2, arrow3, arrow4,arrow5,arrow6,arrow7,
                arrow8,arrow9,arrow10,arrow11,arrow12,arrow13,arrow14,arrow15,arrow16,
                arrow17,arrow18,arrow19,arrow20,arrow21,arrow22 };
            vertikale = new Grid[] {Vertikal1,Vertikal2,Vertikal3,Vertikal4,Vertikal5,Vertikal6,Vertikal7,Vertikal8,Vertikal9,
            Vertikal10,Vertikal11,Vertikal12,Vertikal13,Vertikal14,Vertikal15,Vertikal16,Vertikal17,Vertikal18,Vertikal19,Vertikal20};
            horizontale = new Grid[] {Horizontal1,Horizontal2,Horizontal3,Horizontal4,Horizontal5,Horizontal6,Horizontal7,Horizontal8,Horizontal9,
            Horizontal10,Horizontal11,Horizontal12,Horizontal13,Horizontal14,Horizontal15,Horizontal16,Horizontal17,Horizontal18,Horizontal19,Horizontal20};

            double h = Glavni.ActualHeight;
            double w = Glavni.ActualWidth;
            Prozor.Height = h - 120;
            Prozor.Width = w - 120;

            centriranje();
            promjere_r();
            try
            {
                crtanje();
            }
            catch
            { }
        }
        double[] razmaci_gore = new double[10];
        double[] razmaci_dole = new double[10];
        double[] promjeri;

        private void promjere_r()
        {
            promjeri = new double[P.broj_namota * 2 + 2];

            promjeri[0] = P.promjer_jezgre;
            promjeri[1] = P.promjer_jezgre + P.razmak_lijevo * 2;
            for (int i = 0; i < P.broj_namota; i++)
            {

                promjeri[2 * i + 2] = promjeri[2 * i + 1] + P.sirina_namota[i] * 2;
                promjeri[2 * i + 3] = promjeri[2 * i + 2] + P.razmak_desno[i] * 2;


            }
        }


        private Size MeasureString(string candidate, FontFamily fontF, FontStyle fontS, FontWeight fontW, FontStretch fontSt, double fontSi)
        {
            var formattedText = new FormattedText(
                candidate,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(fontF, fontS, fontW, fontSt),
                fontSi,
                Brushes.Black);

            return new Size(formattedText.Width, formattedText.Height);
        }

        private void centriranje()
        {
            int j = 0;
            for (int i = 0; i < P.broj_namota; i++)
            {
                if (P.centriraj[i])
                {
                    j = i;
                    break;
                }
            }


            for (int i = 0; i < P.broj_namota; i++)
            {
                if (i != j)
                {
                    double v = P.visina_namota[j] - P.visina_namota[i];
                    razmaci_dole[i] = P.razmak_dole + v / 2;
                    razmaci_gore[i] = P.razmak_gore + v / 2;
                }
                else
                {
                    razmaci_dole[i] = P.razmak_dole;
                    razmaci_gore[i] = P.razmak_gore;
                }
            }

        }


        private void crtanje()
        {

            string datum = "";
            if (DateTime.Now.Minute > 9)
            {
                if (DateTime.Now.Hour > 9)
                {
                    datum = DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + ". - " + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                }
                else
                    datum = DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + ". - 0" + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
            }
            else
            {
                if (DateTime.Now.Hour > 9)
                    datum = DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + ". - " + DateTime.Now.Hour + ":" + "0" + DateTime.Now.Minute;
                else
                    datum = DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + ". - 0" + DateTime.Now.Hour + ":" + "0" + DateTime.Now.Minute;
            }

            Datum.Text = datum;


            double koeficijent_visine = Prozor.Height / P.visina_prozora;
            double koeficijent_duzine = Prozor.Width / P.sirina_prozora;

            double max_razmak_sekcija = 0;
            double sredina = 0;
            for (int i = 0; i < P.broj_namota; i++)
            {
                if (P.broj_aks[i] == 2 && P.razmak_sekc[i] > max_razmak_sekcija)
                {
                    max_razmak_sekcija = P.razmak_sekc[i];
                    sredina = Prozor.Height / 2 - max_razmak_sekcija * koeficijent_visine / 2;
                }
            }

            for (int i = 0; i < P.broj_namota; i++)
            {


                Polyline p = new Polyline();

                p.Stroke = Brushes.Gray;
                //p.Fill = Brushes.LightBlue;
                p.StrokeThickness = 2;
                p.HorizontalAlignment = HorizontalAlignment.Left;
                p.VerticalAlignment = VerticalAlignment.Top;
                if (P.broj_aks[i] != 2)
                {
                    p.Points = new PointCollection() 
            { 
              
                new Point((promjeri[2*i+2]-promjeri[0])*koeficijent_duzine, 
                    Math.Round(razmaci_gore[i] )*koeficijent_visine), 
                new Point((promjeri[2*i+2]-promjeri[0])*koeficijent_duzine, 
                  Math.Round(razmaci_gore[i] +P.visina_namota[i])*koeficijent_visine),
                new Point((promjeri[2*i+1]-promjeri[0])*koeficijent_duzine,
                      (Math.Round(razmaci_gore[i] +P.visina_namota[i])*koeficijent_visine)),  
                        new Point((promjeri[2*i+1]-promjeri[0])*koeficijent_duzine,
                      (Math.Round(razmaci_gore[i] )*koeficijent_visine)),
                   
            };
                }
                else // 2 aks sekcije
                {
                    p.Points = new PointCollection() 
            { 
              
                new Point((promjeri[2*i+2]-promjeri[0])*koeficijent_duzine, 
                    Math.Round(razmaci_gore[i] )*koeficijent_visine), 
                new Point((promjeri[2*i+2]-promjeri[0])*koeficijent_duzine, 
                  Math.Round(razmaci_gore[i] +P.visina_namota[i]/2-P.razmak_sekc[i]/2)*koeficijent_visine),
                new Point((promjeri[2*i+1]-promjeri[0])*koeficijent_duzine,
                      (Math.Round(razmaci_gore[i] +P.visina_namota[i]/2-P.razmak_sekc[i]/2)*koeficijent_visine)),  
                        new Point((promjeri[2*i+1]-promjeri[0])*koeficijent_duzine,
                      (Math.Round(razmaci_gore[i] )*koeficijent_visine)),
                   
            };
                    Polyline p2 = new Polyline();

                    p2.Stroke = Brushes.Gray;
                    //p.Fill = Brushes.LightBlue;
                    p2.StrokeThickness = 2;
                    p2.HorizontalAlignment = HorizontalAlignment.Left;
                    p2.VerticalAlignment = VerticalAlignment.Top;

                    p2.Points = new PointCollection() 
            { 
              
                new Point((promjeri[2*i+2]-promjeri[0])*koeficijent_duzine, 
                    Math.Round(razmaci_gore[i]+P.visina_namota[i]/2+P.razmak_sekc[i]/2 )*koeficijent_visine), 
                new Point((promjeri[2*i+2]-promjeri[0])*koeficijent_duzine, 
                  Math.Round(razmaci_gore[i] +P.visina_namota[i])*koeficijent_visine),
                new Point((promjeri[2*i+1]-promjeri[0])*koeficijent_duzine,
                      (Math.Round(razmaci_gore[i] +P.visina_namota[i])*koeficijent_visine)),  
                        new Point((promjeri[2*i+1]-promjeri[0])*koeficijent_duzine,
                      (Math.Round(razmaci_gore[i]+P.visina_namota[i]/2+P.razmak_sekc[i]/2 )*koeficijent_visine)),
                       new Point((promjeri[2 * i + 2] - promjeri[0]) * koeficijent_duzine,
                        Math.Round(razmaci_gore[i]+P.visina_namota[i]/2+P.razmak_sekc[i]/2) * koeficijent_visine)
                   
            };

                    TextBlock ta = new TextBlock();
                    ta.Text = P.razmak_sekc[i].ToString();
                    Size zz2 = MeasureString(P.razmak_sekc[i].ToString(), new FontFamily("Arial"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal, 14);

                    ta.Margin = new Thickness((promjeri[2 * i + 1] - promjeri[0]) * koeficijent_duzine-20, Math.Round(razmaci_gore[i] + P.visina_namota[i] / 2 -zz2.Height) * koeficijent_visine, 0, 0);
                    Prozor.Children.Add(ta);

                    Prozor.Children.Add(p2);

                }
                if (P.broj_kan[i] == 0)
                    p.Points.Add(new Point((promjeri[2 * i + 2] - promjeri[0]) * koeficijent_duzine,
                        Math.Round(razmaci_gore[i]) * koeficijent_visine));
                else
                {
                    for (int j = 0; j < P.broj_kan[i]; j++)
                    {
                        double lijevo = promjeri[2 * i + 1] - promjeri[0];
                        System.Windows.Shapes.Path myPath = new System.Windows.Shapes.Path();
                        myPath.Stroke = System.Windows.Media.Brushes.Gray;
                        myPath.Fill = System.Windows.Media.Brushes.Gray;
                        myPath.StrokeThickness = 2;
                        myPath.HorizontalAlignment = HorizontalAlignment.Left;
                        myPath.VerticalAlignment = VerticalAlignment.Top;
                        LineGeometry linija0 = new LineGeometry();
                        if (j == 0)
                        {
                            linija0.StartPoint = new Point(lijevo * koeficijent_duzine, (razmaci_gore[i]) * koeficijent_visine);
                            linija0.EndPoint = new Point(lijevo * koeficijent_duzine + P.sirina_namota[i] * 2 * koeficijent_duzine / (P.broj_kan[i] + 1) * (j + 1) - P.razmak_kan[i] * koeficijent_duzine / 2, (razmaci_gore[i]) * koeficijent_visine);
                        }
                        LineGeometry linija0a = new LineGeometry();
                        if (j == P.broj_kan[i] - 1)
                        {

                            linija0a.StartPoint = new Point(P.razmak_kan[i] * koeficijent_duzine + lijevo * koeficijent_duzine + P.sirina_namota[i] * 2 * koeficijent_duzine / (P.broj_kan[i] + 1) * (j + 1) - P.razmak_kan[i] * koeficijent_duzine / 2, (razmaci_gore[i]) * koeficijent_visine);
                            linija0a.EndPoint = new Point((promjeri[2 * i + 2] - promjeri[0]) * koeficijent_duzine, (razmaci_gore[i]) * koeficijent_visine);

                        }
                        else
                        {
                            linija0a.StartPoint = new Point(P.razmak_kan[i] * koeficijent_duzine + lijevo * koeficijent_duzine + P.sirina_namota[i] * 2 * koeficijent_duzine / (P.broj_kan[i] + 1) * (j + 1) - P.razmak_kan[i] * koeficijent_duzine / 2, (razmaci_gore[i]) * koeficijent_visine);
                            linija0a.EndPoint = new Point(lijevo * koeficijent_duzine + P.sirina_namota[i] * 2 * koeficijent_duzine / (P.broj_kan[i] + 1) * (j + 2) - P.razmak_kan[i] * koeficijent_duzine / 2, (razmaci_gore[i]) * koeficijent_visine);

                        }


                        LineGeometry linija1 = new LineGeometry();
                        linija1.StartPoint = new Point(lijevo * koeficijent_duzine + P.sirina_namota[i] * 2 * koeficijent_duzine / (P.broj_kan[i] + 1) * (j + 1) - P.razmak_kan[i] * koeficijent_duzine / 2, (razmaci_gore[i]) * koeficijent_visine);
                        linija1.EndPoint = new Point(lijevo * koeficijent_duzine + P.sirina_namota[i] * 2 * koeficijent_duzine / (P.broj_kan[i] + 1) * (j + 1) - P.razmak_kan[i] * koeficijent_duzine / 2, (razmaci_gore[i]) * koeficijent_visine + (P.visina_namota[i]) * koeficijent_visine / 10);
                        
                         TextBlock ta = new TextBlock();
                    ta.Text = P.razmak_kan[i].ToString();
                    Size zz2 = MeasureString(P.razmak_kan[i].ToString(), new FontFamily("Arial"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal, 14);

                    ta.Margin = new Thickness(lijevo * koeficijent_duzine + P.sirina_namota[i] * 2 * koeficijent_duzine / (P.broj_kan[i] + 1) * (j + 1) - P.razmak_kan[i] * koeficijent_duzine / 2+P.razmak_kan[i]/2*koeficijent_duzine-zz2.Width/2, (razmaci_gore[i]) * koeficijent_visine+(P.visina_namota[i]) * koeficijent_visine / 20, 0, 0);
                    Prozor.Children.Add(ta);
                        
                        LineGeometry linija2 = new LineGeometry();
                        linija2.StartPoint = new Point(P.razmak_kan[i] * koeficijent_duzine + lijevo * koeficijent_duzine + P.sirina_namota[i] * 2 * koeficijent_duzine / (P.broj_kan[i] + 1) * (j + 1) - P.razmak_kan[i] * koeficijent_duzine / 2, (razmaci_gore[i]) * koeficijent_visine);
                        linija2.EndPoint = new Point(P.razmak_kan[i] * koeficijent_duzine + lijevo * koeficijent_duzine + P.sirina_namota[i] * 2 * koeficijent_duzine / (P.broj_kan[i] + 1) * (j + 1) - P.razmak_kan[i] * koeficijent_duzine / 2, (razmaci_gore[i]) * koeficijent_visine + (P.visina_namota[i]) * koeficijent_visine / 10);

                        GeometryGroup grupalinija = new GeometryGroup();
                        grupalinija.Children.Add(linija0);
                        grupalinija.Children.Add(linija1);
                        grupalinija.Children.Add(linija2);
                        grupalinija.Children.Add(linija0a);


                        myPath.Data = grupalinija;
                        Canvas.SetZIndex(myPath, 1000);
                        Prozor.Children.Add(myPath);
                    }
                }


                Prozor.Children.Add(p);

                double dodatak = 0;
                if (sredina > 0 && 370 + 30 * 2 * i >= sredina ) dodatak = max_razmak_sekcija + 30;

                System.Windows.Shapes.Path myPath2 = new System.Windows.Shapes.Path();
                myPath2.Stroke = System.Windows.Media.Brushes.Gray;
                myPath2.Fill = System.Windows.Media.Brushes.Gray;
                myPath2.StrokeThickness = 2;
                myPath2.HorizontalAlignment = HorizontalAlignment.Left;
                myPath2.VerticalAlignment = VerticalAlignment.Top;
                LineGeometry linija = new LineGeometry();
                linija.StartPoint = new Point(0, 370 + 30 * 2 * i+dodatak);
                linija.EndPoint = new Point(80 + (promjeri[2 * i + 1] - promjeri[0]) * koeficijent_duzine, 370 + 30 * 2 * i + dodatak);
                strele[2 * i + 1].Margin = new Thickness(80 - 7 + (promjeri[2 * i + 1] - promjeri[0]) * koeficijent_duzine, 370 + 30 * 2 * i - 3.75 + dodatak, 0, 0);
                strele[2 * i + 1].Visibility = Visibility.Visible;
                TextBlock t1 = new TextBlock();
                t1.Text = promjeri[2 * i + 1].ToString();
                t1.Margin = new Thickness(20, 370 + 30 * 2 * i - 20+dodatak, 0, 0);
                Glavni.Children.Add(t1);
                vertikale[2 * i].Visibility = Visibility.Visible;
                vertikale[2 * i].Margin = new Thickness((promjeri[2 * i + 2] + promjeri[2 * i + 1] - promjeri[0] * 2) / 2 * koeficijent_duzine - 5, 0, 0, 0);
                vertikale[2 * i].Height = Math.Round(razmaci_gore[i]) * koeficijent_visine;

                TextBlock tt2 = new TextBlock();
                tt2.VerticalAlignment = VerticalAlignment.Top;
                Size zzz = MeasureString(razmaci_gore[i].ToString(), new FontFamily("Arial"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal, 14);
                tt2.Margin = new Thickness((promjeri[2 * i + 2] + promjeri[2 * i + 1] - promjeri[0] * 2) / 2 * koeficijent_duzine - 20, Math.Round(razmaci_gore[i]) * koeficijent_visine / 2 - zzz.Width/2, 0, 0);
                tt2.Text = razmaci_gore[i].ToString();
                tt2.LayoutTransform = new RotateTransform(270);
                Prozor.Children.Add(tt2);

                LineGeometry linijaa = new LineGeometry();

                if (sredina > 0 && 370 + (30 * 2 * i+1) >= sredina) dodatak = max_razmak_sekcija + 30;
                linijaa.StartPoint = new Point(0, 370 + 30 * (2 * i + 1) + dodatak);
                linijaa.EndPoint = new Point(80 + (promjeri[2 * i + 2] - promjeri[0]) * koeficijent_duzine, 370 + 30 * (2 * i + 1) + dodatak);
                strele[2 * i + 2].Margin = new Thickness(80 - 7 + (promjeri[2 * i + 2] - promjeri[0]) * koeficijent_duzine, 370 + 30 * (2 * i + 1) - 3.75 + dodatak, 0, 0);
                strele[2 * i + 2].Visibility = Visibility.Visible;
                TextBlock t2 = new TextBlock();
                t2.Text = promjeri[2 * i + 2].ToString();
                t2.Margin = new Thickness(20, 370 + 30 * (2 * i + 1) - 20+dodatak, 0, 0);
                Glavni.Children.Add(t2);
                vertikale[2 * i + 1].Visibility = Visibility.Visible;
                vertikale[2 * i + 1].Margin = new Thickness((promjeri[2 * i + 2] + promjeri[2 * i + 1] - promjeri[0] * 2) / 2 * koeficijent_duzine - 5, Math.Round(razmaci_gore[i] + P.visina_namota[i]) * koeficijent_visine, 0, 0);
                vertikale[2 * i + 1].Height = Math.Round(razmaci_dole[i]) * koeficijent_visine;

                TextBlock tt = new TextBlock();
                tt.VerticalAlignment = VerticalAlignment.Top;
                Size zzz2 = MeasureString(razmaci_dole[i].ToString() , new FontFamily("Arial"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal, 14);
                tt.Margin = new Thickness((promjeri[2 * i + 2] + promjeri[2 * i + 1] - promjeri[0] * 2) / 2 * koeficijent_duzine - 20, Math.Round(razmaci_gore[i] + P.visina_namota[i] + razmaci_dole[i] / 2) * koeficijent_visine-zzz2.Width/2, 0, 0);
                tt.Text = razmaci_dole[i].ToString();
                tt.LayoutTransform = new RotateTransform(270);
                Prozor.Children.Add(tt);


                GeometryGroup grupalinijaa = new GeometryGroup();
                grupalinijaa.Children.Add(linija);
                grupalinijaa.Children.Add(linijaa);

                horizontale[2 * i].Visibility = Visibility.Visible;
                horizontale[2 * i].Margin = new Thickness((promjeri[2 * i] - promjeri[0]) * koeficijent_duzine, 250, 0, 0);
                horizontale[2 * i].Width = Math.Round(promjeri[2 * i + 1] - promjeri[2 * i]) * koeficijent_duzine;

                TextBlock tt3 = new TextBlock();
                tt3.VerticalAlignment = VerticalAlignment.Top;
                tt3.Margin = new Thickness((promjeri[2 * i] + promjeri[2 * i + 1] - 2 * promjeri[0]) / 2 * koeficijent_duzine - 5, 236, 0, 0);
                tt3.Text = ((promjeri[2 * i + 1] - promjeri[2 * i]) / 2).ToString();
                Prozor.Children.Add(tt3);

                horizontale[2 * i + 1].Visibility = Visibility.Visible;
                horizontale[2 * i + 1].Margin = new Thickness((promjeri[2 * i + 1] - promjeri[0]) * koeficijent_duzine, 250, 0, 0);
                horizontale[2 * i + 1].Width = Math.Round(promjeri[2 * i + 2] - promjeri[2 * i + 1]) * koeficijent_duzine;

                TextBlock tt4 = new TextBlock();
                tt4.VerticalAlignment = VerticalAlignment.Top;
                tt4.Margin = new Thickness((promjeri[2 * i + 2] + promjeri[2 * i + 1] - promjeri[0] * 2) / 2 * koeficijent_duzine - 5, 236, 0, 0);
                tt4.Text = ((promjeri[2 * i + 2] - promjeri[2 * i + 1])/2).ToString();
                Prozor.Children.Add(tt4);


                TextBlock tt5 = new TextBlock();
                tt5.VerticalAlignment = VerticalAlignment.Top;
                Size zz = MeasureString(P.naziv_namota[i], new FontFamily("Arial"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal, 20);
                tt5.Margin = new Thickness((promjeri[2 * i + 2] + promjeri[2 * i + 1] - promjeri[0] * 2) / 2 * koeficijent_duzine - zz.Width / 2, 190, 0, 0);
                tt5.Text = P.naziv_namota[i];
                tt5.FontSize = 20;
                Prozor.Children.Add(tt5);


                TextBlock ttt = new TextBlock();
                ttt.VerticalAlignment = VerticalAlignment.Top;
                Size zz3 = MeasureString("Ln=" + P.visina_namota[i].ToString() + " mm", new FontFamily("Arial"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal, 14);

                ttt.Margin = new Thickness((promjeri[2 * i + 2] - promjeri[0]) * koeficijent_duzine - 20, Math.Round(razmaci_gore[i] + P.visina_namota[i]) * koeficijent_visine - zz3.Width - 5, 0, 0);
                ttt.Text = "Ln="+P.visina_namota[i].ToString()+" mm";
                ttt.LayoutTransform = new RotateTransform(270);
                Prozor.Children.Add(ttt);


                if (i == 0)
                {
                    LineGeometry linijaaa = new LineGeometry();
                    linijaaa.StartPoint = new Point(0, 340 + 30 * 2 * i);
                    linijaaa.EndPoint = new Point(80, 340 + 30 * 2 * i);
                    grupalinijaa.Children.Add(linijaaa);
                    strele[0].Margin = new Thickness(80 - 7, 340 + 30 * 2 * i - 3.75, 0, 0);
                    strele[0].Visibility = Visibility.Visible;
                    TextBlock t0 = new TextBlock();
                    t0.Text = promjeri[0].ToString();
                    t0.Margin = new Thickness(20, 340 + 30 * 2 * i - 20, 0, 0);
                    Glavni.Children.Add(t0);
                }
                if (i == P.broj_namota - 1)
                {
                    LineGeometry linijaaa = new LineGeometry();
                    linijaaa.StartPoint = new Point(0, 400 + 30 * (2 * i + 1) + dodatak);
                    linijaaa.EndPoint = new Point(80 + (promjeri[2 * i + 3] - promjeri[0]) * koeficijent_duzine, 400 + 30 * (2 * i + 1)+dodatak);
                    grupalinijaa.Children.Add(linijaaa);
                    strele[2 * i + 3].Margin = new Thickness(80 - 7 + (promjeri[2 * i + 3] - promjeri[0]) * koeficijent_duzine, 400 + 30 * (2 * i + 1) - 3.75 + dodatak, 0, 0);
                    strele[2 * i + 3].Visibility = Visibility.Visible;
                    TextBlock t0 = new TextBlock();
                    t0.Text = promjeri[2 * i + 3].ToString();
                    t0.Margin = new Thickness(20, 400 + 30 * (2 * i + 1) - 20+dodatak, 0, 0);
                    Glavni.Children.Add(t0);

                    horizontale[2 * i + 2].Visibility = Visibility.Visible;
                    horizontale[2 * i + 2].Margin = new Thickness((promjeri[2 * i + 2] - promjeri[0]) * koeficijent_duzine, 250, 0, 0);
                    horizontale[2 * i + 2].Width = Math.Round(promjeri[2 * i + 3] - promjeri[2 * i + 2]) * koeficijent_duzine;

                    TextBlock tt6 = new TextBlock();
                    tt6.VerticalAlignment = VerticalAlignment.Top;
                    tt6.Margin = new Thickness((promjeri[2 * i + 3] + promjeri[2 * i + 2] - promjeri[0] * 2) / 2 * koeficijent_duzine - 5, 240, 0, 0);
                    tt6.Text = ((promjeri[2 * i + 3] - promjeri[2 * i + 2]) / 2).ToString();
                    Prozor.Children.Add(tt6);

                }

                myPath2.Data = grupalinijaa;
                Glavni.Children.Add(myPath2);




            }
            bt = new Button();

            bt.FontSize = 16;
            bt.HorizontalAlignment = HorizontalAlignment.Left;
            bt.VerticalAlignment = VerticalAlignment.Top;
            bt.Content = "PRINT";
            
            bt.Margin = new Thickness(555, 1095, 0, 0);
            bt.Width = 105;
            bt.Height = 30;
            bt.Click += Print_Click;
            Glavni.Children.Add(bt);
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            P.log_book(projekt,"Printa",db);
            Print(Glavni);
        }

        private void Print(Visual v)
        {

            //Glavni.Children.Remove(bt);
            bt.Visibility = Visibility.Hidden;
            Datum.Visibility = Visibility.Visible;

            System.Windows.FrameworkElement e = v as System.Windows.FrameworkElement;
            if (e == null)
                return;

            PrintDialog pd = new PrintDialog();
            pd.PrintTicket.PageOrientation = PageOrientation.Landscape;
            if (pd.ShowDialog() == true)
            {
                //store original scale
                Transform originalScale = e.LayoutTransform;
                //get selected printer capabilities
                System.Printing.PrintCapabilities capabilities = pd.PrintQueue.GetPrintCapabilities(pd.PrintTicket);

                //get scale of the print wrt to screen of WPF visual
                double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / e.ActualWidth, capabilities.PageImageableArea.ExtentHeight /
                               e.ActualHeight);

                //Transform the Visual to scale
                e.LayoutTransform = new ScaleTransform(scale, scale);

                //get the size of the printer page
                System.Windows.Size sz = new System.Windows.Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

                //update the layout of the visual to the printer page size.
                e.Measure(sz);
                e.Arrange(new System.Windows.Rect(new System.Windows.Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight + 50), sz));

                //now print the visual to printer to fit on the one page.

                pd.PrintVisual(v, MakeValidFileName(projekt));

                //apply the original transform.
                e.LayoutTransform = originalScale;
            }



            bt.Visibility = Visibility.Visible;
            Datum.Visibility = Visibility.Hidden;

            //Close();
        }

        public string MakeValidFileName(string name)
        {
            var builder = new StringBuilder();
            var invalid = System.IO.Path.GetInvalidFileNameChars();
            foreach (var cur in name)
            {
                if (!invalid.Contains(cur))
                {
                    builder.Append(cur);
                }
                else
                    builder.Append("_");
            }
            return builder.ToString();
        }

    }
}
