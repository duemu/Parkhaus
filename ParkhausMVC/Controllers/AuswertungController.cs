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
            //Holt alle Umsätze sortiert nach Jahr und Monat
            List<Umsatz> umsaetze = context.Umsatz.OrderBy(u => u.Jahr).OrderBy(u => u.MonatNr).ToList();

            //Erstellt ein neues AuswertungsViewModel
            AuswertungViewModel viewModel = new AuswertungViewModel();

            //Übergibt dem ViewModel die ungefilterten Umsätze für den Filter 
            viewModel.umsatzListeFilter = umsaetze;

            //Prüft ob der Jahresparameter leer ist
            if (jahr.HasValue)
            {
                //Wenn nicht leer wird nach dem Jahr gefiltert
                umsaetze = umsaetze.Where(u => u.Jahr == jahr).ToList();
                //Übergibt das Jahr dem ViewModel
                viewModel.jahr = jahr.ToString();
            }
            //Prüft ob der Monatsparameter leer ist
            if (!String.IsNullOrWhiteSpace(monat))
            {
                //Wenn nicht leer wird nach dem Monat gefiltert
                umsaetze = umsaetze.Where(u => u.Monat == monat).ToList();
                //Übergibt den Monat dem ViewModel
                viewModel.monat = monat;
            }
            //Prüft ob die Benutzerkategorie leer ist
            if (!String.IsNullOrWhiteSpace(typ))
            {
                //Wenn nicht leer wird nach der Benutzerkategorie gefiltert
                umsaetze = umsaetze.Where(u => u.Typ == typ).ToList();
                //Übergibt die Benutzerkategorie dem ViewModel
                viewModel.typ = typ;
            }
            //Übergibt die gefilterten Umsätze dem ViewModel
            viewModel.umsatzListe = umsaetze;


            return View(viewModel);
        }

        //Für die Protokollierung wird eine spezielle Route definiert, damit die Seitenzahl als route übergeben werden kann
        [Route("Auswertung/Protokollierung/{seite?}")]
        public ActionResult Protokollierung(int? seite, string von_datum_string, string bis_datum_string, string typ)
        {

            //Erstellt ein neues ProtokollierungViewModel
            ProtokollierungViewModel viewModel = new ProtokollierungViewModel();

            //Wenn kein Seite gewählt wird, erste seite laden
            if (!seite.HasValue) seite = 1;
            //Definiert die Anzahl Einträge pro seite
            int anzahlEintraege = 15;
            //Bis Anzahl wird mit anzahlEintäge mal Seitenzahl berechnet 
            int bis = anzahlEintraege * (int) seite;
            //Die von Anzahl ist die bis Anzahl minus einmal die anzahl Einträge
            int von = bis - anzahlEintraege;
            //Die Protokollierung wird absteigend angezeigt
            List<Protokollierung> protokollierung = context.Protokollierung.OrderByDescending(p =>p.Datum).ToList();
            
            //Prüft ob das von-Datum leer ist
            if (!String.IsNullOrWhiteSpace(von_datum_string)) { 
                //Das Von-Datum muss zuerst geparst werden
                DateTime? von_datum = DateTime.ParseExact(von_datum_string, "ddMMyyyyHHmm", CultureInfo.InvariantCulture);
                //Filtert nach dem Von-Datum
                protokollierung = protokollierung.Where(p => p.Datum >= von_datum).ToList();
                //Speichert das Von-Datum als String auf dem ViewModel
                viewModel.von_datum = von_datum.Value.ToString("dd.MM.yyyy HH:mm");
            }
            //Prüft ob das bis-Datum leer ist
            if (!String.IsNullOrWhiteSpace(bis_datum_string))
            {
                //Das Von-Datum muss zuerst geparst werden
                DateTime? bis_datum = DateTime.ParseExact(bis_datum_string, "ddMMyyyyHHmm", CultureInfo.InvariantCulture);
                //Filtert nach dem Bis-Datum
                protokollierung = protokollierung.Where(p => p.Datum <= bis_datum).ToList();
                //Speichert das Bis-Datum als String auf dem ViewModel
                viewModel.bis_datum = bis_datum.Value.ToString("dd.MM.yyyy HH:mm");
            }
            //Prüft ob die Benutzerkategorie leer ist
            if (!String.IsNullOrWhiteSpace(typ))
            {
                //Filtert nach der Benutzerkategorie
                protokollierung = protokollierung.Where(p => p.Typ == typ).ToList();
                //Speichert die Benutzerkategorie auf dem ViewModel
                viewModel.typ = typ;
            }
            
            //Holt nur die aktuelle Seite
            viewModel.protokollierung = protokollierung.Skip(von).Take(anzahlEintraege).ToList();
            //Übergibt die anzahlt der Seiten, die zur auswahl stehen sollen
            viewModel.anzahlSeiten = (protokollierung.Count() + anzahlEintraege - 1) / anzahlEintraege;
            //Übergibt die aktive seite
            viewModel.aktiveSeite = (int)seite;

     
            return View(viewModel);
        }
    }
}