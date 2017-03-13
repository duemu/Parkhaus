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
            List<Dauermieter> dauermieters = context.Dauermieter.ToList();
            return View(dauermieters);
        }


        public ActionResult Hinzufuegen()
        {
            Dauermieter dauermieter = new Dauermieter();
            return View(dauermieter);
        }

        [HttpPost]
        public ActionResult Hinzufuegen(Dauermieter dauermieter)
        {

            dauermieter.hinzufuegen();

            return RedirectToAction("Index", "Dauermieter"); 
        }

        public ActionResult Bezahlen(int dauermieterID)
        {
            //Dauermieter holen
            Dauermieter dauermieter = context.Dauermieter.Where(d => d.DauermieterID == dauermieterID).FirstOrDefault();
            //Rechnung des dauermieters bezahlen
            dauermieter.rechnung_bezahlen();

            return RedirectToAction("Index", "Dauermieter");
        }



    }
}