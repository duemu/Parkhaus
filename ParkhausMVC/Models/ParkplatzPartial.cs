using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkhausMVC.Models
{
    public partial class Parkplatz
    {
        ParkhausDBEntities context = new ParkhausDBEntities();
        //Gibt zurück, ob der Parkplatz frei ist
        public bool ist_Frei()
        {
           //Prüft, ob ein Ticket mit dem Parkplatz existiert, das kein Austrittsdatum gesetzt hat
           int count = context.Ticket.Where(t => t.ParkplatzID == this.ParkplatzID && t.Ausgangsdatum == null).Count();
           if(count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Gib zurück, ob der Parkplatz einem Dauermieter zugewiesen ist
        public bool hat_dauermieter()
        {   //Prüft, ob ein Dauermieter existiert, der den Parkplatz zugewiesen hat
            int count = context.Dauermieter.Where(d => d.ParkplatzID == this.ParkplatzID).Count();
            if(count == 0)
            {
                return false;
            }
            return true;
        }

    }
}