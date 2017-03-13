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
       
        public void rechnung_bezahlen()
        {
            //Holt den Tarif für die monatliche miete aus der Tariftabelle 
            decimal preis = _context.Parktarif.Where(p => p.TarifTyp == 4).First().Preis;

            //Erstellt eine neuen Zahlungseintrag
            Zahlung zahlung = new Zahlung
            {
                DauermieterID = this.DauermieterID,
                Preis = preis,
                Datum = DateTime.Now.Date
            };
            //Fügt der Zahlungseintrag der Tabelle hinzu
            _context.Zahlung.Add(zahlung);
            //Speichert die Änderungen
            _context.SaveChanges();
        }
        
        //Fügt einen neuen Dauermieter hinzu
        public void hinzufuegen()
        {
            //Generiert einen neuen Eintrittscode
            code_generieren();

            //Weist einen neuen Parkplatz zu
            parkplatz_zuweisen();
            
            //Fügt einen neuen Dauermietereintrag der Tabelle hinzu
            _context.Dauermieter.Add(this);
            //Speichert die Änderung
            _context.SaveChanges();
        }

        private void parkplatz_zuweisen()
        {   
            ParkplatzListe parkplaetze = new ParkplatzListe();
            //Holt eine freien Parkplatz
            Parkplatz pp = parkplaetze.hole_Parkplatz();
            //Speichert die ParkplatzID auf dem Dauermieter
            this.ParkplatzID = pp.ParkplatzID;

        }

        private void code_generieren()
        {
            int count = 0;

            //Randomgenerator erstellen
            Random rndm = new Random();
            int code = 0;
            do {
                //Code zwischen 100000 und 999999 erstellen
                code = rndm.Next(100000, 999999);
                //Prüft ob diese Code schon vergeben wurde, wenn ja (chancen sind sehr klein) wird ein neuer Code erstellt
                count = _context.Dauermieter.Where(d => d.Code == code).Count();
            } while (count != 0);

            //Speichert den Code auf dem Dauermieter
            this.Code = code;
        }
       
        //Methode wird für die Dauermieterübersicht benötigt
        public bool hat_rechung_bezahlt()
        {
            //Holt das aktuelle Datum
            DateTime heute = DateTime.Now.Date;
            //Prüft ob ein Eintrag für diesen Dauermieter im aktuellen Jahr und Monat existiert
            int count = _context.Zahlung.Where(z => z.DauermieterID == this.DauermieterID && z.Datum.Month == heute.Month && z.Datum.Year == heute.Year).Count();
            //Wenn Count != 0 (Kann nicht mehr als 1 sein) existiert ein Eintrag 
            if(count != 0) return true;
            //Wenn nicht wird false zurückgegeben
            return false;
        }

        public bool hat_rechung_bezahlt(DateTime eingangsdatum)
        {
            /*Liegt das Eingangsdatum von dem 15. wird geprüft, ob die Rechnung für den Vormonat bezahlt wurde.
            Ausgenommen sind die Dauermieter, die sich erst in diesem Monat angemeldet haben
             */
            if (eingangsdatum.Day < 15 && !(this.Erstelldatum.Value.Month == eingangsdatum.Month && this.Erstelldatum.Value.Year == eingangsdatum.Year))
                {
                    //Setzt das datum auf den Vormonat
                    eingangsdatum = eingangsdatum.AddMonths(-1);
                }
            //Prüft ob ein Eintrag für diesen Dauermieter im aktuellen Jahr und Monat existiert
            int count = _context.Zahlung.Where(z => z.DauermieterID == this.DauermieterID && z.Datum.Month == eingangsdatum.Month && z.Datum.Year == eingangsdatum.Year).Count();
            //Wenn Count != 0 (Kann nicht mehr als 1 sein) existiert ein Eintrag 
            if (count != 0) return true;
            //Wenn nicht wird false zurückgegeben
            return false;
        }



    }
}