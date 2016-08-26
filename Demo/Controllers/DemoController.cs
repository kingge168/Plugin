using Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Demo.Controllers
{
    public class DemoController : Controller
    {
        //
        // GET: /Demo/

        public ActionResult Index()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");
            return View(new Car() { Name="bently", Price=3000000});
        }

    }
}
