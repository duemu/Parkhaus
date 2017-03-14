using ParkhausMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkhausMVC
{
    class TarifBerechner
    {
        ParkhausDBEntities context = new ParkhausDBEntities();

        double totalPreis = 0;
        int tarifType;
        double schritt = 0.25d;

        public double hole_tarif(DateTime eintrittszeit, DateTime austrittszeit)
        {
            totalPreis = 0;
            //Millisekunden abschneiden
            eintrittszeit = eintrittszeit.AddTicks(-(eintrittszeit.Ticks % TimeSpan.TicksPerSecond));
            austrittszeit = austrittszeit.AddTicks(-(austrittszeit.Ticks % TimeSpan.TicksPerSecond));

            TimeSpan ts = austrittszeit - eintrittszeit;
            double totalTage = ts.TotalDays;

            if (totalTage > 1)
            {
                return berechne_Tag_Tarif(totalTage);
            }
            else
            {
                return berechne_Stunden_Tarif(eintrittszeit, austrittszeit);
            }
        }

        public double berechne_Tag_Tarif(double anz_tage)
        {
            //Holt den Tarif für den Tag
            List<Parktarif> tarife = context.Parktarif.Where(t => t.TarifTyp == 3).ToList();
            Parktarif tarif  =tarife.Last();
            int gerundete_tage = Convert.ToInt16(Math.Ceiling(anz_tage));
            return (double) tarif.Preis * gerundete_tage;
        }

        public double berechne_Stunden_Tarif(DateTime eintrittszeit, DateTime austrittszeit)
        {
            DayOfWeek wochentag = eintrittszeit.DayOfWeek;
            //Prüft ob 
            if (wochentag == DayOfWeek.Saturday || wochentag == DayOfWeek.Sunday || Feiertage.ist_Feiertag(eintrittszeit.Date))
            {
                tarifType = 2;
            }
            else {
                tarifType = 1;
            }

            //Stunde holen
            int hour = eintrittszeit.Hour;

            List<Parktarif> tarife = context.Parktarif.ToList();

            List<Parktarif> tarifs = context.Parktarif.Where(t => t.TarifTyp == tarifType && t.Zeit <= hour).OrderBy(t => t.Zeit).ToList();

            Parktarif tarif = tarifs.Last();

            totalPreis += (double) tarif.Preis * schritt;

            eintrittszeit = eintrittszeit.AddHours(schritt);

            if (eintrittszeit >= austrittszeit) return totalPreis;

            return berechne_Stunden_Tarif(eintrittszeit, austrittszeit);
        }
    }
}
