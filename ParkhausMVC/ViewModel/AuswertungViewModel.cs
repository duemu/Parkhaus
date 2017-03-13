using ParkhausMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkhausMVC.ViewModel
{
    public class AuswertungViewModel
    {

        public List<Umsatz> umsatzListe { get; set; }

        public List<Umsatz> umsatzListeFilter { get; set; }

        public string monat { get; set; } = "Alle Monate";


        public string jahr { get; set; } = "Alle Jahre";

        public string typ { get; set; } = "Alle";


    }
}