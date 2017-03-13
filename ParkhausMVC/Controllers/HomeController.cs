using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ParkhausMVC.Models;
using ParkhausMVC.Filter;
using System.Globalization;

namespace ParkhausMVC.Controllers
{
    public class HomeController : Controller
    {
        ParkhausDBEntities context = new ParkhausDBEntities();
                
        public ActionResult Index()
        {

            List<Stockwerk> stockwerke = context.Stockwerk.ToList();
            
            return View("Uebersicht",stockwerke);
        }

        [JsonFehlerbehandlung]
        public ActionResult eintreten(string eintritttsdatum, string typ, int code)
        {
            //Ticket erstellen
            Ticket t = new Ticket();

            //Eingangsdatum von Javascript-Format in Datetime umwandeln
            DateTime eingangsdatum = DateTime.ParseExact(eintritttsdatum, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);

            //Ticket eintreten
            t.eintreten(eingangsdatum, typ, code);

            //Parkplatz holen, um ihn auf dem Ticket anzuzeigen 
            Parkplatz p = context.Parkplatz.Where(pp => pp.ParkplatzID == t.ParkplatzID).FirstOrDefault();


            //Json zurückgeben
            return Json(new
            {
                success = true,
                tiketID = t.TicketID,
                parkplatzID = p.ParkplatzID,
                parkplatzNr = p.Parkplatznummer,
                stockwerk = p.Stockwerk.Bezeichung,
                stockwerkID = p.StockwerkID
            });
        }

        public ActionResult austreten(int parkplatzID, string austrittsdatum)
        {
            //Ticket über die ParkplatzID ermitteln
            Ticket ticket = context.Ticket.Where(t => t.ParkplatzID == parkplatzID && t.Ausgangsdatum == null).First();

            //Austrittsdatum kommt von JavaScript als String, in Datetime umwandeln
            DateTime austrittsdatum_d = DateTime.ParseExact(austrittsdatum, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);

            //Ticket austreten lassen
            ticket.austreten(austrittsdatum_d);

            context.SaveChanges();
            //Änderungen als JSON zurückmelden
            return Json(new
            {
                success = true,
                preis = ticket.Preis
            });
        }

        public ActionResult ticket_anzeigen(int ParkplatzID)
        {
            Ticket ticket = context.Ticket.Where(t => t.ParkplatzID == ParkplatzID && t.Ausgangsdatum == null).First();

            return Json(new
            {
                eingangsdatum = ticket.Eingangsdatum,
                ticketID = ticket.TicketID,
                parkplatzNr = ticket.Parkplatz.Parkplatznummer,
                stockwerk = ticket.Parkplatz.Stockwerk.Bezeichung
            });
        }



    }
}