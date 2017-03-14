using ParkhausMVC.Filter;
using ParkhausMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParkhausMVC.Controllers
{
    [JsonFehlerbehandlung]
    public class KonfigurationController : Controller
    {

        ParkhausDBEntities _context = new ParkhausDBEntities();
        // GET: Konfiguration
        public ActionResult Index()
        {
            //Übergibt alle Stockwerke der View
            return View(_context.Stockwerk.ToList());
        }

        public ActionResult Tarif()
        {
            //Übergibt alle Tarife der View
            return View(_context.Parktarif.ToList());
        }
        //Wird per AJAX Request aufgerufen
        public ActionResult stockwerk_hinzufuegen(string stockwerkBezeichnung, int anzParkplaetze)
        {
            //Erstellt das Stockwerk mit der übergebenen Bezeichnung und der Anzahl Parkplätze
            Stockwerk stockwerk = new Stockwerk(stockwerkBezeichnung, anzParkplaetze);
            //Leitet der Benutzer zur Stockwerkkonfiguration weiter
            return RedirectToAction("Index");
        }
        //Wird per AJAX Request aufgerufen
        public ActionResult stockwerk_bearbeiten(int id, string bezeichnung, int anzParkplaetze)
        {
            //Holt das stockwerk mit der StockwerkID
            Stockwerk stockwerk = _context.Stockwerk.Where(s => s.StockwerkID == id).First();
            //Bearbeitet das Stockwerk
            stockwerk.bearbeiten(bezeichnung, anzParkplaetze);
            //Leitet der Benutzer zur Stockwerkkonfiguration weiter
            return RedirectToAction("Index");


        }
        //Wird per AJAX Request aufgerufen
        public void stockwerk_loeschen(int id)
        {
            //Holt das stockwerk mit der StockwerkID
            Stockwerk stockwerk = _context.Stockwerk.Where(s => s.StockwerkID == id).FirstOrDefault();
            //Löscht das stockwerk
            stockwerk.loeschen();

        }

        //Wird per AJAX Request aufgerufen
        public ActionResult tarif_hinzufuegen(int typ, decimal zeit, decimal preis)
        {
            //Erstellt einen neuen Tarif
            Parktarif tarif = new Parktarif(typ, zeit, preis);

            return RedirectToAction("Tarif");
        }
        //Wird per AJAX Request aufgerufen
        public ActionResult tarif_bearbeiten(int id, decimal zeit, decimal preis)
        {
            //Holt den Tarif mit der TarifID
            Parktarif tarif = _context.Parktarif.Where(t => t.TarifID == id).First();
            //Bearbeitet den Tarif
            tarif.bearbeiten(zeit, preis);
            //Speichert die änderung
            _context.SaveChanges();
            //Leitet den Benutzer zur Tarifübersicht weiter
            return RedirectToAction("Tarif");


        }
        //Wird per AJAX Request aufgerufen
        public void tarif_loeschen(int id)
        {
            //Holt den Tarif mit der TarifID
            Parktarif tarif = _context.Parktarif.Where(t => t.TarifID == id).First();

            //Löscht den Tarif
            tarif.loeschen();

        }



    }
}