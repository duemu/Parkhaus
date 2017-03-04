using ParkhausMVC.Filter;
using ParkhausMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParkhausMVC.Controllers
{
    [JsonErrorHandler]
    public class KonfigurationController : Controller
    {

        ParkhausDBEntities _context = new ParkhausDBEntities();
        // GET: Konfiguration
        public ActionResult Index()
        {
            return View(_context.Stockwerk.ToList());
        }

        public ActionResult Tarif()
        {
            List<Parktarif> parktarif = _context.Parktarif.ToList();

            return View(parktarif);
        }

        public ActionResult stockwerk_hinzufuegen(string stockwerkBezeichnung, int anzParkplaetze)
        {
            Stockwerk stockwerk = new Stockwerk(stockwerkBezeichnung, anzParkplaetze);
            return RedirectToAction("Index");
        }

        public ActionResult stockwerk_bearbeiten(int id, string bezeichnung, int anzParkplaetze)
        {
            Stockwerk stockwerk = _context.Stockwerk.Where(s => s.StockwerkID == id).First();
            stockwerk.bearbeiten(bezeichnung, anzParkplaetze);
            return RedirectToAction("Index");


        }

        public void stockwerk_loeschen(int id)
        {
            Stockwerk stockwerk = _context.Stockwerk.Where(s => s.StockwerkID == id).FirstOrDefault();

            stockwerk.loeschen();

        }


        public ActionResult tarif_hinzufuegen(int typ, decimal zeit, decimal preis)
        {
            Parktarif tarif = new Parktarif(typ, zeit, preis);

            return RedirectToAction("Tarif");
        }

        public ActionResult tarif_bearbeiten(int id, decimal zeit, decimal preis)
        {
            Parktarif tarif = _context.Parktarif.Where(t => t.TarifID == id).First();

            tarif.bearbeiten(zeit, preis);

            return RedirectToAction("Tarif");


        }

        public void tarif_loeschen(int id)
        {

            Parktarif tarif = _context.Parktarif.Where(t => t.TarifID == id).First();

            tarif.loeschen();

        }



    }
}