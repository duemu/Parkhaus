using ParkhausMVC.Models;
using ParkhausMVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParkhausMVC.Controllers
{
    public class AuswertungController : Controller
    {
        // GET: Auswertung
        ParkhausDBEntities context = new ParkhausDBEntities();

        public ActionResult Index(FormCollection forms)
        {

            List<Umsatz_pro_Monat> umsaetze = context.Umsatz_pro_Monat.ToList();

            if (forms != null)
            {


            }
            

            

            return View(new AuswertungViewModel { umsatzListe = umsaetze, filter = umsaetze });
        }
    }
}