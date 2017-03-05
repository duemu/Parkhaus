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

        public int anzahlSeiten;

        public int aktiveSeite;
    }
}