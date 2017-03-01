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
            return View(_context.Tarif.ToList());
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



    }
}