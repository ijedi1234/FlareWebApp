using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlareWebApp.Models;
using FlareWebApp.FileLogic;

namespace FlareWebApp.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            HomePageModel model = new HomePageModel();
            string savePath = HttpContext.Server.MapPath("~/SavedData");
            model.AnalaysisStates = FileSaver.GetAnalysisStates(savePath);
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
            string stopwordPath = HttpContext.Server.MapPath("~/FileLogic/StopWords");
            FileSaver fileSaver = new FileSaver(model.SomeFile, savePath, model.IncludeStopwords);
            string filePathOriginal = fileSaver.GetFilePathOriginal();
            WordFile fileWithWords = new WordFile(filePathOriginal, stopwordPath, model.IncludeStopwords);
            fileSaver.AppendPostProcessInfo(fileWithWords.GetMostFrequentWords(), model.IncludeStopwords);
            model.AnalaysisStates = FileSaver.GetAnalysisStates(savePath);
            return View("Index", model);
        }
    }
}