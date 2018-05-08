using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuySellTrade.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "About Buy, Sell, Trade!";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Buy, Sell, Trade!";

            return View();
        }
    }
}