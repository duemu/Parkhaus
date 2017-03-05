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

        public List<Umsatz_pro_Monat> umsatzListeFilter { get; set; }

        public string monat { get; set; } = "Alle Monate";


        public string jahr { get; set; } = "Alle Jahre";

        public string typ { get; set; } = "Alle";


    }
}