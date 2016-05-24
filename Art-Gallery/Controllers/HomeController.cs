using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Art_Gallery.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult Customer()
        //{
        //    ViewBag.Message = "Customer view";

        //    return View();
        //}

        public ActionResult Owner()
        {
            ViewBag.Message = "Owner view";

            return View();
        }
    }
}