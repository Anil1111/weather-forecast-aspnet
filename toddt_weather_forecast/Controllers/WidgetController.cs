using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace toddt_weather_forecast.Controllers
{
    [Authorize]
    public class WidgetController : Controller
    {
        // GET: Widget
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
 
        // GET: Widget/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
 
        // GET: Widget/Create
        [Authorize(Roles = "Admin", Users = "Ross")]
        public ActionResult Create()
        {
            return View();
        }
 
        // POST: Widget/Create
        [HttpPost]
        [Authorize(Roles = "Admin", Users = "Ross")]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}