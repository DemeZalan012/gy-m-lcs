using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace gyümölcs
{
    
    public partial class Form1 : Form
    {
        List<Gyumolcs> adatok = new List<Gyumolcs>();
        ListBox lbLista; 

        public Form1()
        {
            InitializeComponent();

            
            this.Text = "Gyümölcs Kereskedés";
            this.Size = new Size(550, 650);
            this.StartPosition = FormStartPosition.CenterScreen;

           
            lbLista = new ListBox();
            lbLista.Dock = DockStyle.Fill;
            lbLista.Font = new Font("Consolas", 10);
            this.Controls.Add(lbLista);

            
            AdatbazisLetrehozas();
            AdatokBetoltese();
            FeladatokMegoldasa();
        }

        private void AdatbazisLetrehozas()
        {
            string utvonal = "adatok.csv";
            if (!File.Exists(utvonal))
            {
                
                string tartalom = "TAzon;TNév;Magyar;ÉrkIdő;Szavatosság;Ár;Mennyiség\r\n" +
                                 "1;Alma;True;2009.10.12;60;200;20\r\n" +
                                 "2;Banán;False;2009.09.01;30;300;10\r\n" +
                                 "3;Citrom;False;2009.10.12;20;150;5\r\n" +
                                 "4;Körte;True;2009.09.30;30;180;15\r\n" +
                                 "5;Szőlő;True;2009.10.12;15;400;9";
                File.WriteAllText(utvonal, tartalom, System.Text.Encoding.UTF8);
            }
        }

        private void AdatokBetoltese()
        {
            try
            {
                var sorok = File.ReadAllLines("adatok.csv").Skip(1);
                foreach (var sor in sorok)
                {
                    if (string.IsNullOrWhiteSpace(sor)) continue;
                    var s = sor.Split(';');
                    adatok.Add(new Gyumolcs
                    {
                        TAzon = int.Parse(s[0]),
                        TNev = s[1],
                        Magyar = bool.Parse(s[2]),
                        ErkIdo = DateTime.Parse(s[3]),
                        Szavatossag = int.Parse(s[4]),
                        Ar = int.Parse(s[5]),
                        Mennyiseg = int.Parse(s[6])
                    });
                }
            }
            catch (Exception ex) { MessageBox.Show("Hiba a betöltéskor: " + ex.Message); }
        }

        private void FeladatokMegoldasa()
        {
            lbLista.Items.Add("=== GYÜMÖLCS KERESKEDÉS ADATOK ===");
            lbLista.Items.Add("");

            // 4. feladat
            lbLista.Items.Add("4. feladat (Magyar termékek):");
            foreach (var g in adatok.Where(x => x.Magyar))
                lbLista.Items.Add($" - {g.TNev} ({g.Ar} Ft/egységár)");

            // 5. feladat
            lbLista.Items.Add("");
            lbLista.Items.Add("5. feladat (Olcsóbb, mint 200 Ft):");
            foreach (var g in adatok.Where(x => x.Ar <= 200))
                lbLista.Items.Add($" - {g.TNev} | Beérkezett: {g.ErkIdo.ToShortDateString()}");

            // 6. feladat
            lbLista.Items.Add("");
            lbLista.Items.Add("6. feladat (8kg - 15kg közötti mennyiség):");
            foreach (var g in adatok.Where(x => x.Mennyiseg >= 8 && x.Mennyiseg <= 15))
                lbLista.Items.Add($" - {g.TNev}: {g.Mennyiseg} kg");

            // 7. feladat
            lbLista.Items.Add("");
            lbLista.Items.Add("7. feladat (Lejárt szavatosságúak):");
            foreach (var g in adatok.Where(x => x.ErkIdo.AddDays(x.Szavatossag) < DateTime.Now))
                lbLista.Items.Add($" - {g.TNev} (LEJÁRT)");

            // 8. feladat
            lbLista.Items.Add("");
            lbLista.Items.Add("8. feladat (Gyümölcsök összértéke):");
            foreach (var g in adatok)
                lbLista.Items.Add($" - {g.TNev}: {g.Ar * g.Mennyiseg} Ft");
        }
    }

    public class Gyumolcs
    {
        public int TAzon { get; set; }
        public string TNev { get; set; }
        public bool Magyar { get; set; }
        public DateTime ErkIdo { get; set; }
        public int Szavatossag { get; set; }
        public int Ar { get; set; }
        public int Mennyiseg { get; set; }
    }
}