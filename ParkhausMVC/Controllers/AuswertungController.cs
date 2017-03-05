using ParkhausMVC.Models;
using ParkhausMVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParkhausMVC.Controllers
{
    public class AuswertungController : Controller
    {
        // GET: Auswertung
        ParkhausDBEntities context = new ParkhausDBEntities();

        public ActionResult Index(int? jahr, string monat,string typ)
        {

            List<Umsatz_pro_Monat> umsaetze = context.Umsatz_pro_Monat.OrderBy(u=>u.Jahr).OrderBy(u => u.MonatNr).ToList();

           
            AuswertungViewModel viewModel = new AuswertungViewModel();

            viewModel.umsatzListeFilter = umsaetze;

            if (jahr.HasValue)
            { 
                umsaetze = umsaetze.Where(u => u.Jahr == jahr).ToList();
                viewModel.jahr = jahr.ToString();
            }
            if (!String.IsNullOrWhiteSpace(monat))
            {
                umsaetze = umsaetze.Where(u => u.Monat == monat).ToList();
                viewModel.monat = monat;
            }
            if (!String.IsNullOrWhiteSpace(typ))
            {
                umsaetze = umsaetze.Where(u => u.Typ == typ).ToList();
                viewModel.typ = typ;
            }
            viewModel.umsatzListe = umsaetze;


            return View(viewModel);
        }
        
        public ActionResult Protokollierung(int? seite)
        {

            if (!seite.HasValue) seite = 1;
            int anzahlEintraege = 15;

            int bis = anzahlEintraege * (int) seite;
            int von = bis - anzahlEintraege;

            ProtokollierungViewModel viewModel = new ProtokollierungViewModel
            {
                protokollierung = context.Protokollierung.OrderByDescending(p => p.LogID).Skip(von).Take(anzahlEintraege).ToList(),
                anzahlSeiten = (context.Protokollierung.Count() + anzahlEintraege-1) / anzahlEintraege,
                aktiveSeite = (int)seite
            };
            return View(viewModel);
        }
    }
}