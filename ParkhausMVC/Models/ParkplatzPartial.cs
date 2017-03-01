using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkhausMVC.Models
{
    public partial class Parkplatz
    {
        ParkhausDBEntities context = new ParkhausDBEntities();

        public bool ist_Frei()
        {
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

        public bool hat_dauermieter()
        {
            int count = context.Dauermieter.Where(d => d.ParkplatzID == this.ParkplatzID).Count();
            if(count == 0)
            {
                return false;
            }
            return true;
        }

    }
}