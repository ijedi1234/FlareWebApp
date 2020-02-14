using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlareWebApp.Models;
using FlareWebApp.FileLogic;
//using System.Web.UI.WebControls;

namespace FlareWebApp.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            HomePageModel model = new HomePageModel();
            return View(model);
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Submit(HomePageModel model) {
            string savePath = HttpContext.Server.MapPath("~/SavedData");
            FileSaver.SaveFile(model.SomeFile, savePath);
            model.FreqWords = new List<string>();
            model.FreqWords.Add("Example");
            return View("Index", model);
        }
    }
}