using ParkhausMVC.Models;
using ParkhausMVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ParkhausMVC.Controllers
{
    public class AuswertungController : Controller
    {
        // GET: Auswertung
        ParkhausDBEntities context = new ParkhausDBEntities();

        public ActionResult Index(int? jahr, string monat, string typ)
        {

            List<Umsatz> umsaetze = context.Umsatz.OrderBy(u => u.Jahr).OrderBy(u => u.MonatNr).ToList();


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
        [Route("Auswertung/Protokollierung/{seite?}")]
        public ActionResult Protokollierung(int? seite, string von_datum_string, string bis_datum_string, string typ)
        {

            ProtokollierungViewModel viewModel = new ProtokollierungViewModel();

            if (!seite.HasValue) seite = 1;
            int anzahlEintraege = 15;

            int bis = anzahlEintraege * (int) seite;
            int von = bis - anzahlEintraege;

            List<Protokollierung> protokollierung = context.Protokollierung.OrderByDescending(p =>p.Datum).ToList();
 
            if (!String.IsNullOrWhiteSpace(von_datum_string)) { 
                DateTime? von_datum = DateTime.ParseExact(von_datum_string, "ddMMyyyyHHmm", CultureInfo.InvariantCulture);
                protokollierung = protokollierung.Where(p => p.Datum >= von_datum).ToList();
                viewModel.von_datum = von_datum.Value.ToString("dd.MM.yyyy HH:mm");
            }

            if (!String.IsNullOrWhiteSpace(bis_datum_string))
            {
                DateTime? bis_datum = DateTime.ParseExact(bis_datum_string, "ddMMyyyyHHmm", CultureInfo.InvariantCulture);
                protokollierung = protokollierung.Where(p => p.Datum <= bis_datum).ToList();
                viewModel.bis_datum = bis_datum.Value.ToString("dd.MM.yyyy HH:mm");
            }

            if (!String.IsNullOrWhiteSpace(typ))
            {
                protokollierung = protokollierung.Where(p => p.Typ == typ).ToList();
                viewModel.typ = typ;
            }
            

            viewModel.protokollierung = protokollierung.Skip(von).Take(anzahlEintraege).ToList();
            viewModel.anzahlSeiten = (protokollierung.Count() + anzahlEintraege - 1) / anzahlEintraege;
            viewModel.aktiveSeite = (int)seite;

     
            return View(viewModel);
        }
    }
}