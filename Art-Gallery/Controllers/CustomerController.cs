using Art_Gallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Art_Gallery.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            DataStoreContext _myDataContext = new DataStoreContext();

            

            return View();
        }
    }
}