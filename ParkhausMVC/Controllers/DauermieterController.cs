using ParkhausMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParkhausMVC.Controllers
{
    public class DauermieterController : Controller
    {

        ParkhausDBEntities context = new ParkhausDBEntities();
     
        // GET: Dauermieter
        public ActionResult Index()
        {
            //Holt alle Dauermieter
            List<Dauermieter> dauermieters = context.Dauermieter.ToList();
            //Übergibt die Dauermieter der View
            return View(dauermieters);
        }


        public ActionResult Hinzufuegen()
        {
            //Erstellt ein neues Dauermieterobjekt
            Dauermieter dauermieter = new Dauermieter();
            //Übergibt das Dauermieterobjekt der View
            return View(dauermieter);
        }
        //Wird aufgerufen, wenn das Dauermieterformular gepostet wird
        [HttpPost]
        public ActionResult Hinzufuegen(Dauermieter dauermieter)
        {
            //Fügt den neuen Dauermieter hinzu
            dauermieter.hinzufuegen();
            //Leitet den Benutzer zur Dauermieterübersicht weiter
            return RedirectToAction("Index", "Dauermieter"); 
        }

        public ActionResult Bezahlen(int dauermieterID)
        {
            //Dauermieter holen
            Dauermieter dauermieter = context.Dauermieter.Where(d => d.DauermieterID == dauermieterID).FirstOrDefault();
            //Rechnung des dauermieters bezahlen
            dauermieter.rechnung_bezahlen();
            //Leitet den Benutzer zur Dauermieterübersicht weiter
            return RedirectToAction("Index", "Dauermieter");
        }



    }
}