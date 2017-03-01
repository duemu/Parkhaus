using ParkhausMVC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkhausMVC.Models
{
    public partial class Ticket
    {
        private ParkhausDBEntities _context = new ParkhausDBEntities();
        private ParkplatzController pc = new ParkplatzController();

        public void eintreten(DateTime eingangsdatum, string typ, int code)
        {

            Parkplatz pp;

            if (typ == "dauermieter")
            {
                Dauermieter dauermieter = _context.Dauermieter.Where(d => d.Code == code).FirstOrDefault();

                if (dauermieter == null) throw new Exception("Code inkorrekt");

                if (!dauermieter.hat_rechung_bezahlt(true))
                {
                    throw new Exception("Sie haben Ihre Rechnung noch nicht bezahlt!");
                }

                int cnt = _context.Ticket.Where(ticket => ticket.ParkplatzID == dauermieter.ParkplatzID && ticket.Ausgangsdatum == null).Count();

                if (cnt != 0) throw new Exception("Dieser Dauermieter ist bereits im Parkhaus");

                pp = dauermieter.Parkplatz;

            }
            else
            {
                pp = pc.hole_Parkplatz();
                if (pp == null) throw new Exception("Alle Parkplätze sind belegt");
            }

            this.Eingangsdatum = eingangsdatum;
            this.ParkplatzID = pp.ParkplatzID;

            _context.Ticket.Add(this);

            _context.SaveChanges();


        }

        public void austreten(DateTime ausgangsdatum)
        {
            //Ausgangsdatum setzen
            this.Ausgangsdatum = ausgangsdatum;

            //Nur berechnen wenn es sich nicht um ein Dauermieter handelt
            if (!this.Parkplatz.hat_dauermieter())
            {
                //Neuer Tarifcontroller erstellen
                TarifController tarifController = new TarifController();

                //Preis berechnen
                this.Preis = (decimal)tarifController.hole_tarif(this.Eingangsdatum, (DateTime)this.Ausgangsdatum);
                //Änderungen speichern
                _context.SaveChanges();
            }
        }
    }
}