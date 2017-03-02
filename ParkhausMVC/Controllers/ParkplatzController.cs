using ParkhausMVC.Filter;
using ParkhausMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParkhausMVC.Controllers
{
    public class ParkplatzController : Controller
    {
        ParkhausDBEntities context = new ParkhausDBEntities();

        public Parkplatz hole_Parkplatz()
        {
            int countParkplätze = context.Freie_Parkplaetze.Count();

            if (countParkplätze == 0)
            {
                return null;
            }

            int min = context.Stockwerk.Max(s => s.Anzahl_freie_Parkplaetze());

            int stockwerkID = context.Stockwerk.Where(s => s.Anzahl_freie_Parkplaetze() == min).OrderBy(s => s.StockwerkID).First().StockwerkID;

            //int stockwerkID = stockwerk_mit_meisten_plaetzen().StockwerkID;


            List <Freie_Parkplaetze> freieParkplätze = context.Freie_Parkplaetze.Where(fr => fr.StockwerkID == stockwerkID).ToList();

            Random rnd = new Random();

            int rndom = rnd.Next(freieParkplätze.Count);

            int parkplatzID = freieParkplätze.ElementAt(rndom).ParkplatzID;

            return context.Parkplatz.Where(p => p.ParkplatzID == parkplatzID).First();

        }


        private Stockwerk stockwerk_mit_meisten_plaetzen()
        {

            var maxFrei = context.Freie_Plaetze_Pro_Stockwerk.Max(s => s.AnzahlParkplätze);

            int stockwerkID = context.Freie_Plaetze_Pro_Stockwerk.Where(s => s.AnzahlParkplätze == maxFrei).FirstOrDefault().StockwerkID;

            return context.Stockwerk.Where(s => s.StockwerkID == stockwerkID).FirstOrDefault();
        }

        [JsonErrorHandler]
        public ActionResult add_stockwerk(string stockwerkBezeichnung, int anzParkplaetze)
        {
            Stockwerk stockwerk = new Stockwerk { Bezeichung = stockwerkBezeichnung };

            context.Stockwerk.Add(stockwerk);
            context.SaveChanges();

            generiere_parkplatz(stockwerk.StockwerkID, anzParkplaetze);

            return Json(new
            {
                stockwerkID = stockwerk.StockwerkID
            });
        }

        public void generiere_parkplatz(int stockwerkID,int anzParkplaetze)
        {

            List<Parkplatz> parkplaetze = context.Parkplatz.Where(p => p.StockwerkID == stockwerkID).ToList();
            int parkplatznummer;

            if (parkplaetze.Count == 0) {
                parkplatznummer = 0;
            }
            else { 
                parkplatznummer = parkplaetze.Max(p => p.Parkplatznummer).Value;
            }

            for (int i = 0; i<anzParkplaetze; i++)
            {
                parkplatznummer++;
                context.Parkplatz.Add(new Parkplatz { Parkplatznummer = parkplatznummer, StockwerkID = stockwerkID });
            }

            context.SaveChanges();
        }



    }
}