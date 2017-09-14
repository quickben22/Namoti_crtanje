using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Crtanje_Namota
{
    public class Podaci : INotifyPropertyChanged
    {


        //broj lima u slogu
        public string[] _naziv_namota = new string[10];
        public string[] naziv_namota { get { return _naziv_namota; } set { _naziv_namota = value; this.OnPropertyChanged("naziv_namota"); } }
        public int[] _broj_aks = new int[10];
        public int[] broj_aks { get { return _broj_aks; } set { _broj_aks = value; this.OnPropertyChanged("broj_aks"); } }
        public ObservableCollection<int> broj_aks_popis { get; set; }
        public int[] _broj_kan = new int[10];
        public int[] broj_kan { get { return _broj_kan; } set { _broj_kan = value; this.OnPropertyChanged("broj_kan"); } }
        public ObservableCollection<int> broj_kan_popis { get; set; }
        public double[] _visina_namota = new double[10];
        public double[] visina_namota { get { return _visina_namota; } set { _visina_namota = value; this.OnPropertyChanged("visina_namota"); } }
        public double[] _sirina_namota = new double[10];
        public double[] sirina_namota { get { return _sirina_namota; } set { _sirina_namota = value; this.OnPropertyChanged("sirina_namota"); } }
        public double[] _razmak_desno = new double[10];
        public double[] razmak_desno { get { return _razmak_desno; } set { _razmak_desno = value; this.OnPropertyChanged("razmak_desno"); } }
        public double[] _razmak_sekc = new double[10];
        public double[] razmak_sekc { get { return _razmak_sekc; } set { _razmak_sekc = value; this.OnPropertyChanged("razmak_sekc"); } }
        public double[] _razmak_kan = new double[10];
        public double[] razmak_kan { get { return _razmak_kan; } set { _razmak_kan = (value); this.OnPropertyChanged("razmak_kan"); } }
        public bool[] _centriraj = new bool[10];
        public bool[] centriraj { get { return _centriraj; } set { _centriraj = value; this.OnPropertyChanged("centriraj"); } }


        private string _tip_trafa="";
        public string tip_trafa { get { return _tip_trafa; } set { _tip_trafa = (value); this.OnPropertyChanged("tip_trafa"); } }
        
        private double _visina_prozora;
        public double visina_prozora { get { return _visina_prozora; } set { _visina_prozora = pretvori(value); this.OnPropertyChanged("visina_prozora"); } }
        private double _sirina_prozora;
        public double sirina_prozora { get { return _sirina_prozora; } set { _sirina_prozora = pretvori(value); this.OnPropertyChanged("sirina_prozora"); } }
        private double _promjer_jezgre;
        public double promjer_jezgre { get { return _promjer_jezgre; } set { _promjer_jezgre = pretvori(value); this.OnPropertyChanged("promjer_jezgre"); } }
        private int _broj_namota;
        public int broj_namota { get { return _broj_namota; } set { _broj_namota = (value); this.OnPropertyChanged("broj_namota"); } }
        public ObservableCollection<int> broj_namota_popis { get; set; }
        private double _razmak_lijevo;
        public double razmak_lijevo { get { return _razmak_lijevo; } set { _razmak_lijevo = pretvori(value); this.OnPropertyChanged("razmak_lijevo"); } }
        private double _razmak_gore;
        public double razmak_gore { get { return _razmak_gore; } set { _razmak_gore = pretvori(value); this.OnPropertyChanged("razmak_gore"); } }
        private double _razmak_dole;
        public double razmak_dole { get { return _razmak_dole; } set { _razmak_dole = pretvori(value); this.OnPropertyChanged("razmak_dole"); } }
 

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propName)
        {

            if (this.PropertyChanged != null)
                this.PropertyChanged(
                    this, new PropertyChangedEventArgs(propName));
        }

        private double pretvori(double a)
        {
            if (a.ToString() == "Infinity" || a.ToString() == "-Infinity" || a.ToString() == "NaN")
                return 0;
            return a;
        }

        public bool otvori(SQLiteDatabase db, string projekt, object g)
        {
            DataTable tabla;
            String query = "select * from Tablica WHERE (Projekt = \"" + projekt + "\")";
            tabla = db.GetDataTable(query);



            foreach (PropertyInfo p in g.GetType().GetProperties())
            {
                string propertyName = p.Name;
                if (tabla.Columns.Contains(propertyName))
                    PropertySet(g, propertyName, tabla.Rows[0][propertyName].ToString());
            }



            return true;
        }

        public bool spremi(SQLiteDatabase db, object g, string projekt, bool postoji)
        {
            Dictionary<String, String> data1 = new Dictionary<String, String>();


            foreach (PropertyInfo p in g.GetType().GetProperties())
            {
                string propertyName = p.Name;
                string b = g.GetType().GetProperty(propertyName).PropertyType.ToString();
                if (b == "System.Double[]")
                {
                    double[] rezultat = (double[])g.GetType().GetProperty(propertyName).GetValue(g, null);
                    string s = "";
                    foreach (double v in rezultat)
                    {
                        if (s != "")
                            s += "," + v.ToString().Replace(",", ".");
                        else
                            s += v.ToString().Replace(",", ".");
                    }
                    b = s;
                    data1.Add(propertyName, b);
                }
                else if (b == "System.String[]")
                {
                    string[] rezultat = (string[])g.GetType().GetProperty(propertyName).GetValue(g, null);
                    string s = "";
                    foreach (string v in rezultat)
                    {
                        string v2 = v;
                        if (v2 == null) v2 = "";
                        if (s != "")
                            s += "," + v2.ToString().Replace(",", ".");
                        else
                            s += v2.ToString().Replace(",", ".");
                    }
                    b = s;
                    data1.Add(propertyName, b);
                }
                else if (b == "System.Int32[]")
                {
                    Int32[] rezultat = (Int32[])g.GetType().GetProperty(propertyName).GetValue(g, null);
                    string s = "";
                    foreach (Int32 v in rezultat)
                    {
                        if (s != "")
                            s += "," + v.ToString().Replace(",", ".");
                        else
                            s += v.ToString().Replace(",", ".");
                    }
                    b = s;
                    data1.Add(propertyName, b);
                }
                else if (b == "System.Boolean[]")
                {
                    Boolean[] rezultat = (Boolean[])g.GetType().GetProperty(propertyName).GetValue(g, null);
                    string s = "";
                    foreach (Boolean v in rezultat)
                    {
                        if (s != "")
                            s += "," + v.ToString().Replace(",", ".");
                        else
                            s += v.ToString().Replace(",", ".");
                    }
                    b = s;
                    data1.Add(propertyName, b);
                }
                else if (b == "System.Double" || b == "System.String" || b == "System.Int32")
                {
                    string rezultat = g.GetType().GetProperty(propertyName).GetValue(g, null).ToString();
                    data1.Add(propertyName, rezultat.ToString());
                }


            }

            data1.Add("Projekt", projekt);
            if (postoji)
                db.Update("Tablica", data1, String.Format("Projekt = {0}", "\"" + projekt + "\""));
            else
                db.Insert("Tablica", data1);

            return true;
        }




        public bool stvori(SQLiteDatabase db, object g)
        {
            db = new SQLiteDatabase();
            String query = "create table Tablica (Projekt varchar(30)";
            foreach (PropertyInfo p in g.GetType().GetProperties())
            {
                string propertyName = p.Name;
                //string b = g.GetType().GetProperty(propertyName).GetValue(g, null).ToString();
                string b = g.GetType().GetProperty(propertyName).PropertyType.ToString();
                if (b == "System.Double[]" || b == "System.String[]" || b == "System.Int32[]" || b == "System.Boolean[]")
                {
                    query += ", " + propertyName + " varchar(100)";
                }
                else if (b == "System.Double")
                {
                    query += ", " + propertyName + " REAL";
                }
                else if (b == "System.String")
                {
                    query += ", " + propertyName + " varchar(40)";
                }
                else if (b == "System.Int32")
                {
                    query += ", " + propertyName + " INTEGER";
                }

            }

            //query = "create table Tablica (Projekt varchar(20), textBox1 varchar(50), textBox2 varchar(50), textBox3 varchar(50), textBox4 varchar(50), textBox5 varchar(50), textBox6 varchar(50), textBox7 varchar(50), textBox8 varchar(50), textBox11 varchar(50), textBox12 varchar(50), textBox13 varchar(50), textBox15 varchar(50), textBox16 varchar(50), textBox17 varchar(50), textBox18 varchar(50), textBox19 varchar(50), textBox20 varchar(50), textBox21 varchar(50), textBox22 varchar(50), textBox23 varchar(50), textBox24 varchar(50), textBox25 varchar(50), textBox26 varchar(50), textBox27 varchar(50), textBox28 varchar(50), textBox29 varchar(50), textBox30 varchar(50), textBox31 varchar(50), textBox32 varchar(50), textBox33 varchar(50), textBox34 varchar(50), textBox35 varchar(50), textBox36 varchar(50), textBox37 varchar(50), textBox38 varchar(50), textBox39 varchar(50), textBox40 varchar(50), textBox41 varchar(50), textBox42 TEXT, textBox43 TEXT, textBox44 varchar(50), textBox45 varchar(50), textBox46 TEXT, textBox47 TEXT)";
            query += ")";
            db.ExecuteNonQuery(query);
            query = "create table Log_book (Projekt varchar(30), Datum varchar(50),Korisnik varchar(30),Radnja varchar(30))";
            db.ExecuteNonQuery(query);
            return true;
        }





        public void log_book(string proj, string radnja, SQLiteDatabase db)
        {
            Dictionary<String, String> data1 = new Dictionary<String, String> { { "Projekt", proj }, { "Datum", DateTime.Now.ToString() }, { "Korisnik", WindowsIdentity.GetCurrent().Name.ToString() }, { "Radnja", radnja } };
            db.Insert("Log_book", data1);
        }



        private void PropertySet(object p, string propName, string value)
        {

            if (value != null)
                if (value.Length > 0)
                    if (value.Last() == ',')
                    {
                        value = value.Substring(0, value.Length - 1);
                    }

            Type t = p.GetType();
            PropertyInfo info = t.GetProperty(propName);
            if (info == null)
                return;
            if (!info.CanWrite)
                return;

            string s = info.PropertyType.ToString();
            if (s == "System.Double")
                info.SetValue(p, pretvori(value), null);
            else if (s == "System.String")
                info.SetValue(p, value, null);
            else if (s == "System.Int32")
                info.SetValue(p, Convert.ToInt32(pretvori(value)), null);
            else if (s == "System.Double[]")
                info.SetValue(p, pretvori2(value), null);
            else if (s == "System.String[]")
                info.SetValue(p, pretvori3(value), null);
            else if (s == "System.Int32[]")
                info.SetValue(p, pretvori4(value), null);
            else if (s == "System.Boolean[]")
                info.SetValue(p, pretvori5(value), null);

        }


        private double pretvori(object b)
        {
            double rezultat;
            string a = b.ToString();
            bool isNum = double.TryParse(a, out rezultat);

            if (isNum)
                rezultat = double.Parse(a.Replace(",", "."), System.Globalization.NumberStyles.Any, CultureInfo.CreateSpecificCulture("en-us"));
            else if (a == "∞")
                rezultat = double.PositiveInfinity;
            else
                rezultat = 0;

            return rezultat;
        }


        private double[] pretvori2(object b)
        {
            double[] rezultat;
            string a = b.ToString();
            string[] c = a.Split(',');
            rezultat = new double[10];
            for (int i = 0; i < c.Length; i++)
            {
                rezultat[i] = pretvori(c[i]);
            }

            return rezultat;
        }
        private string[] pretvori3(object b)
        {

            string a = b.ToString();
            string[] c = a.Split(',');
            string[] rezultat = new string[10];
            for (int i = 0; i < c.Length; i++)
            {
                rezultat[i] = (c[i]);
            }
            return  rezultat;

        }
        private int[] pretvori4(object b)
        {
            int[] rezultat;
            string a = b.ToString();
            string[] c = a.Split(',');
            rezultat = new int[10];
            for (int i = 0; i < c.Length; i++)
            {
                rezultat[i] = Convert.ToInt32(pretvori(c[i]));
            }

            return rezultat;
        }
        private bool[] pretvori5(object b)
        {
            bool[] rezultat;
            string a = b.ToString();
            string[] c = a.Split(',');
            rezultat = new bool[10];
            for (int i = 0; i < c.Length; i++)
            {
                rezultat[i] = Convert.ToBoolean(c[i]);
            }

            return rezultat;
        }




    }


    class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            string strValue = (string)value;
            return strValue.Replace(",", ".");
        }
    }
}
