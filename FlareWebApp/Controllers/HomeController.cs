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
            string filepathToRead = FileSaver.SaveFile(model.SomeFile, savePath);
            WordFile fileWithWords = new WordFile(filepathToRead);
            model.FreqWords = fileWithWords.GetMostFrequentWords();
            return View("Index", model);
        }
    }
}