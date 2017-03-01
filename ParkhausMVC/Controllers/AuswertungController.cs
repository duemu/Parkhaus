using ParkhausMVC.Models;
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

        public ActionResult Index()
        {
             List<Umsatz_pro_Monat> umsaetze = context.Umsatz_pro_Monat.ToList();
            return View(umsaetze);
        }
    }
}