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

        ParkplatzController pc = new ParkplatzController();

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

        public void parkplatz_zuweisen()
        {
            Parkplatz pp = pc.hole_Parkplatz();

            if (pp == null) throw new Exception("Alle Parkplätze sind belegt");

            this.ParkplatzID = pp.ParkplatzID;

        }

        public void code_generieren()
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
       

        public bool hat_rechung_bezahlt(bool vor_dem_15en)
        {
            DateTime heute = DateTime.Now.Date;

            if (vor_dem_15en)
            {
                if(heute.Day < 15)
                {
                    heute.AddMonths(-1);
                }
            }

            int count = _context.Zahlung.Where(z => z.DauermieterID == this.DauermieterID && z.Datum.Month == heute.Month && z.Datum.Year == heute.Year).Count();

            if(count != 0)
            {
                return true;
            }
            return false;
        }


      

    }
}