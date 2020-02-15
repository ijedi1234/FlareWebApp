using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlareWebApp.Models;
using FlareWebApp.FileLogic;

namespace FlareWebApp.Controllers {
    public class HomeController : Controller {

        //This is called when a user first loads the website or refreshes.
        //Due to this, analyses states need to be reacquired from disk.
        public ActionResult Index() {
            HomePageModel model = new HomePageModel();
            string saveDir = HttpContext.Server.MapPath("~/SavedData");
            FileHandler fileHandler = new FileHandler(saveDir);
            model.AnalaysisStates = fileHandler.GetAnalysisStates();
            return View(model);
        }

        //This is called when a user submits a file.
        //Analyses states are reacquired from disk, in order to account for the new analysis.
        [HttpPost]
        public ActionResult Submit(HomePageModel model) {
            string saveDir = HttpContext.Server.MapPath("~/SavedData");
            string stopwordPath = HttpContext.Server.MapPath("~/FileLogic/StopWords");
            FileHandler fileHandler = new FileHandler(model.SomeFile, saveDir, model.IncludeStopwords);
            string filePathOriginal = fileHandler.GetFilePathOriginal();
            WordFile fileWithWords = new WordFile(filePathOriginal, stopwordPath, model.IncludeStopwords);
            fileHandler.AppendPostProcessInfo(fileWithWords.GetMostFrequentWords(), model.IncludeStopwords);
            model.AnalaysisStates = fileHandler.GetAnalysisStates();
            return View("Index", model);
        }
    }
}