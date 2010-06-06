using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace stringtemplate_prototyping.Controllers
{
    public class MainController : Controller
    {
        public ActionResult Index(string viewPath)
        {
            return View("~/" +viewPath);
        }

    }
}
