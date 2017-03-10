using ParkhausMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkhausMVC.ViewModel
{
    public class ProtokollierungViewModel
    {
        public List<Protokollierung> protokollierung { get; set; }

        public int anzahlSeiten { get; set; }

        public int aktiveSeite { get; set; }

        public string von_datum { get; set; }

        public string bis_datum { get; set; }

        public string typ { get; set; } = "Alle";
    }
}