using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkhausMVC.Models
{
    public partial class Parktarif
    {
        ParkhausDBEntities _context = new ParkhausDBEntities();

        public Parktarif() { }
        public Parktarif(int typ,decimal zeit,decimal preis)
        {
            this.TarifTyp = typ;
            this.Zeit = zeit;
            this.Preis = preis;

            _context.Parktarif.Add(this);
            _context.SaveChanges();    
        }

        public void loeschen()
        {
            var tarif = _context.Parktarif.Where(t => t.TarifID == this.TarifID).First();
            _context.Parktarif.Remove(tarif);
            _context.SaveChanges();
        }

        public void bearbeiten(decimal zeit, decimal preis)
        {

            this.Zeit =  zeit;
            this.Preis = preis;
            _context.SaveChanges();
        }



    }
}