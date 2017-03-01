using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkhausMVC.Models
{
    public partial class Stockwerk
    {
        ParkhausDBEntities _context = new ParkhausDBEntities();


        public Stockwerk(string bezeichnung, int anzParkplaetze)
        {
            //Setzt die Bezeichnung
            this.Bezeichung = bezeichnung;

            _context.Stockwerk.Add(this);

            for (int ppNr = 1; ppNr <= anzParkplaetze; ppNr++)
            {
                _context.Parkplatz.Add(new Parkplatz { Parkplatznummer = ppNr, StockwerkID = this.StockwerkID });
            }

            _context.SaveChanges();
        }


        public void bearbeiten(string bezeichnung, int anzParkplaetze)
        {
            this.Bezeichung = bezeichnung;

            if(this.Parkplatz.Count() > anzParkplaetze)
            {
                //var parkplaetze = this.Parkplatz.Where(p => p.Parkplatznummer > anzParkplaetze);

                var parkplaetze = _context.Parkplatz.Where(p => p.StockwerkID == this.StockwerkID && p.Parkplatznummer > anzParkplaetze);

                int cntParkplatz = parkplaetze.ToList().Where(p => p.ist_Frei() == false || p.hat_dauermieter() == true).Count();
              
                if (cntParkplatz != 0) throw new Exception("Anzahl der Parkplätze kann nicht verringert werden, da noch Parkplätze belegt sind");

                //Anzahl Parkplätze verringern
                _context.Parkplatz.RemoveRange(parkplaetze);
            }
            else
            {
                int max_ppNr;
                //Höchste Parkplatznummer holen
                if (this.Parkplatz.Count != 0) { 
                    max_ppNr = this.Parkplatz.Max(p => p.Parkplatznummer).Value;
                }else
                {
                    max_ppNr = 0;
                }
                //Parkplatznummer erhöhen
                max_ppNr++;
                //Anzahle neue Parkplätze generieren
                for (int ppNr = max_ppNr; ppNr <= anzParkplaetze; ppNr++)
                {
                    _context.Parkplatz.Add(new Parkplatz { Parkplatznummer = ppNr, StockwerkID = this.StockwerkID });
                }
            }
            //Änderungen speichern
            _context.SaveChanges();


        }

        public void loeschen()
        {
            var parkplaetze = _context.Parkplatz.Where(p => p.StockwerkID == this.StockwerkID);

            int cntParkplatz = parkplaetze.ToList().Where(p => p.ist_Frei() == false || p.hat_dauermieter() == true).Count();
            
            if(cntParkplatz != 0) throw new Exception("Stockwerk kann nicht gelöscht werden, da noch Parkplätze belegt sind");

            //Löscht das Stockwerk
            _context.Stockwerk.Remove(_context.Stockwerk.Where(s => s.StockwerkID == this.StockwerkID).First());

            //Löscht die zugehörigen Parkplätze
            _context.Parkplatz.RemoveRange(parkplaetze);

            //Speichert die Änderungen
            _context.SaveChanges();

        }




    }
}