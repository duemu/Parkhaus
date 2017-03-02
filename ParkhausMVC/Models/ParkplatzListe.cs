using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkhausMVC.Models
{
    public class ParkplatzListe
    {

        ParkhausDBEntities context = new ParkhausDBEntities();

        public Parkplatz hole_Parkplatz()
        {
            //Holt alle Stockwerke            
            List<Stockwerk> stockwerke = context.Stockwerk.ToList();

            //Holt Anzahl der freien Parkplätze auf dem Stockwerk mit den meisten freien Parkplätze
            int anzFrei = stockwerke.Max(s => s.Anzahl_freie_Parkplaetze());

            //Exception werfen, wenn keine freien Parkplätze gefunden wurden
            if(anzFrei == 0) throw new Exception("Alle Parkplätze sind belegt");
            
            //Erstes Stockwerk mit den maximal freien Parkplätzen holen
            Stockwerk stockwerk = stockwerke.Where(s => s.Anzahl_freie_Parkplaetze() == anzFrei).OrderBy(s => s.StockwerkID).First();

            //Zufallszahl generieren zwischen 0 und < AnzahlFreierParkplätze
            int rndom = new Random().Next(anzFrei);

            //Zufälliger freier Parkplatz zurückgeben
            return stockwerk.Freie_Parkplaetze().ElementAt(rndom);

        }




    }
}