using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ParkhausMVC.Models;

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

        public ActionResult Uebersicht()
        {

            List<Stockwerk> stockwerke = context.Stockwerk.ToList();
            return View(stockwerke);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}