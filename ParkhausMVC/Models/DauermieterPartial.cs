using ParkhausMVC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkhausMVC.Models
{
    public partial class Dauermieter
    {
        ParkhausDBEntities _context = new ParkhausDBEntities();

        //ParkplatzController pc = new ParkplatzController();

        public void rechnung_bezahlen()
        {
            Zahlung zahlung = new Zahlung
            {
                DauermieterID = this.DauermieterID,
                Preis = 200,
                Datum = DateTime.Now.Date
            };

            _context.Zahlung.Add(zahlung);
            _context.SaveChanges();
        }

        public void hinzufuegen()
        {
            code_generieren();

            parkplatz_zuweisen();

            _context.Dauermieter.Add(this);

            _context.SaveChanges();
        }

        private void parkplatz_zuweisen()
        {
            ParkplatzListe parkplaetze = new ParkplatzListe();
            Parkplatz pp = parkplaetze.hole_Parkplatz();

            if (pp == null) throw new Exception("Alle Parkplätze sind belegt");

            this.ParkplatzID = pp.ParkplatzID;

        }

        private void code_generieren()
        {
            int count = 0;

            //Randomgenerator erstellen
            Random rndm = new Random();

            do { 
                //Code zwischen 10000 und 100000000 erstellen
                int code = rndm.Next(10000, 1000000000);
                //Prüft ob diese Code schon vergeben wurde
                count = _context.Dauermieter.Where(d => d.Code == code).Count();
            } while (count != 0);

            this.Code = Code;
        }
       

        public bool hat_rechung_bezahlt()
        {
            DateTime heute = DateTime.Now.Date;

            int count = _context.Zahlung.Where(z => z.DauermieterID == this.DauermieterID && z.Datum.Month == heute.Month && z.Datum.Year == heute.Year).Count();

            if(count != 0) return true;
            
            return false;
        }

        public bool hat_rechung_bezahlt(DateTime eingangsdatum)
        {
            //Liegt das Eingangsdatum von dem 15. wird geprüft, ob die Rechnung für den Vormonat bezahlt wurde
            if (eingangsdatum.Day < 15)
                {
                    eingangsdatum = eingangsdatum.AddMonths(-1);
                }

            int count = _context.Zahlung.Where(z => z.DauermieterID == this.DauermieterID && z.Datum.Month == eingangsdatum.Month && z.Datum.Year == eingangsdatum.Year).Count();

            if (count != 0) return true;
            
            return false;
        }



    }
}