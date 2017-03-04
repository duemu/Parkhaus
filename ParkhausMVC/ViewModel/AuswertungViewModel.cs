using ParkhausMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkhausMVC.ViewModel
{
    public class AuswertungViewModel
    {

        public List<Umsatz_pro_Monat> umsatzListe { get; set; } 
        public List<Umsatz_pro_Monat> filter { get; set; }
    }
}