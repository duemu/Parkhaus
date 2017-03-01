using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkhausMVC
{
    static class Feiertage
    {
        private static List<string> statischeFeiertage = new List<string>();
        static Feiertage()
        {
            statischeFeiertage.Add("01.01"); //Neujahr
            statischeFeiertage.Add("02.01"); //Berchtholdtag
            statischeFeiertage.Add("01.08"); //1. August
            statischeFeiertage.Add("25.12"); //Weihnachten
            statischeFeiertage.Add("26.12"); //Stephanstag
        }

        public static bool ist_Feiertag(DateTime datum)
        {
            return ist_statischer_feiertag(datum) || ist_flexibler_feiertag(datum);
        }

        private static bool ist_statischer_feiertag(DateTime datum)
        {
            //Formatiert das Datum, damit das Jahr nicht berücksitigt wird
            string formatierterTag = datum.ToString("dd.MM");

            //Prüft ob der formatierter Tag in der Liste ist
            return statischeFeiertage.Any(formatierterTag.Contains);
        }

        /**
        *Berechnung der Ostertage nach Gauss 
        *Quelle: http://www.msgauss-pir.de/Osterformel1.htm
        **/
        private static bool ist_flexibler_feiertag(DateTime datum)
        {
            int jahr = int.Parse(datum.Year.ToString());

            //Konstante, noch bis ins Jahr 2099 gültig         
            const byte M = 24;
            const byte N = 5;

            int a = jahr % 19;
            int b = jahr % 4;
            int c = jahr % 7;
            int d = (M + 19 * a) % 30;

            int e = (N + (2 * b) + (4 * c) + (6 * d)) % 7;

            int tag = d + e + 22;
            int monat = 3;

            if (tag > 31)
            {
                tag = (d + e - 9);
                monat = 4;
            }

            DateTime osterDatum = new DateTime(jahr, monat, tag);
            //Karfreitag ist 2 Tage vor Ostern
            DateTime karfreitag = osterDatum.AddDays(-2);
            //Auffahrt ist 39 Tage nach Ostern
            DateTime auffahrt = osterDatum.AddDays(39);
            //Pfingsten ist 49 Tage nach Ostern
            DateTime pfingsten = osterDatum.AddDays(49);

            //Prüfung auf die Feiertage
            if (osterDatum == datum) return true;
            if (karfreitag == datum) return true;
            if (auffahrt == datum) return true;
            if (pfingsten == datum) return true;

            return false;
        }


    }
}
