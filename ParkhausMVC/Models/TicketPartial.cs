using ParkhausMVC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkhausMVC.Models
{
    public partial class Ticket
    {
        //Datenbankkontext
        private ParkhausDBEntities _context = new ParkhausDBEntities();

        public void eintreten(DateTime eingangsdatum, string typ, int code)
        {
            //Erstellt ein neues Parkplatzobjekt
            Parkplatz pp;
            //Prüft auf die Benutzerkategorie
            if (typ == "dauermieter")
            {
                //Sucht den Dauermieter mit dem eingegebenen Eintrittscode
                Dauermieter dauermieter = _context.Dauermieter.Where(d => d.Code == code).FirstOrDefault();
                //Wird keiner gefunden, ist der Code inkorrekt
                if (dauermieter == null) throw new Exception("Code inkorrekt");
                //Prüft, ob der Dauermieter die rechnung bezahlt hat
                if (!dauermieter.hat_rechung_bezahlt(eingangsdatum))
                {
                    //Wurde die rechnung nicht bezahlt, wird eine Exception geworfen
                    throw new Exception("Sie haben Ihre Rechnung noch nicht bezahlt!");
                }
                //Prüft ob ein Ticket ohne Austrittsdatum auf dem Parkplatz des Dauermieters vorhanden ist  
                int cnt = _context.Ticket.Where(ticket => ticket.ParkplatzID == dauermieter.ParkplatzID && ticket.Ausgangsdatum == null).Count();
                //Bedeutet, dass der Dauermieter noch im Parkhaus ist, wirft eine Exception
                if (cnt != 0) throw new Exception("Dieser Dauermieter ist bereits im Parkhaus");
                //Weist den Parkplatzobjekt den Parkplatz des dauermieters zu
                pp = dauermieter.Parkplatz;

            }
            else //Gelegenheitsnutzer
            {   //Erstellt ein neues Parkplatzlisteobjekt
                ParkplatzListe parkplätze = new ParkplatzListe();
                //Weist dem 
                pp = parkplätze.hole_Parkplatz();
            }
            //Setzt das Eintrittsdatum
            this.Eingangsdatum = eingangsdatum;
            //Setzt die ParkplatzID
            this.ParkplatzID = pp.ParkplatzID;

            //Fügt das Ticket der Tickettabelle hinzu
            _context.Ticket.Add(this);
            //Speichert die änderungen
            _context.SaveChanges();


        }

        public void austreten(DateTime ausgangsdatum)
        {
            //Ausgangsdatum setzen
            this.Ausgangsdatum = ausgangsdatum;

            //Nur berechnen wenn es sich nicht um ein Dauermieter handelt
            if (!this.Parkplatz.hat_dauermieter())
            {
                //Neuer Tarifberechner erstellen
                TarifBerechner tarifBerechner = new TarifBerechner();
                //Preis berechnen
                this.Preis = (decimal)tarifBerechner.hole_tarif(this.Eingangsdatum, (DateTime)this.Ausgangsdatum); 
            }
            //Änderungen speichern
            _context.SaveChanges();
        }
    }
}