using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkhausMVC.Models
{
    public partial class Tarif
    {
        ParkhausDBEntities _context = new ParkhausDBEntities();

        public Tarif(int typ,decimal zeit,decimal preis)
        {
            this.TarifTyp = typ;
            this.Zeit = zeit;
            this.Preis = preis;

            _context.Tarif.Add(this);
            _context.SaveChanges();    
        }

        public void loeschen()
        {
            var tarif = _context.Tarif.Where(t => t.TarifID == this.TarifID).First();
            _context.Tarif.Remove(tarif);
            _context.SaveChanges();
        }

        public void bearbeiten(decimal zeit, decimal preis)
        {
            this.Zeit = zeit;
            this.Preis = preis;
            _context.SaveChanges();
        }



    }
}