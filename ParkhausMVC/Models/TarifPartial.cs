using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkhausMVC.Models
{
    public partial class Parktarif
    {
        ParkhausDBEntities _context = new ParkhausDBEntities();
        //Konstruktor für Entity Framework
        public Parktarif() { }
        //Konstruktur für die erstellung eines neuen Tarifes
        public Parktarif(int typ,decimal zeit,decimal preis)
        {
            //Setzt die Tarifeigenschaften
            this.TarifTyp = typ;
            this.Zeit = zeit;
            this.Preis = preis;
            //Fügt den Tarif der Liste hinzu
            _context.Parktarif.Add(this);
            //Speichert die Änderungen
            _context.SaveChanges();    
        }

        public void loeschen()
        {
            //Holt den Tarif(workaround, direkt über das Objekt kann der Tarif nicht gelöscht werden, da Entity Frameword den bezug nicht herstellen kann.
            var tarif = _context.Parktarif.Where(t => t.TarifID == this.TarifID).First();
            //Löscht den Tarif
            _context.Parktarif.Remove(tarif);
            //Speichert die Änderungen
            _context.SaveChanges();
        }

        public void bearbeiten(decimal zeit, decimal preis)
        {
            //Setzt die Änderungen
            this.Zeit =  zeit;
            this.Preis = preis;
            //Speichert die Änderungen
            _context.SaveChanges();
        }



    }
}