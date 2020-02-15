using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FlareWebApp.FileLogic;

namespace FlareWebApp.Models {
    public class HomePageModel {

        public HomePageModel() {
            IncludeStopwords = false;
            FreqWords = new List<LetterNode>();
        }

        public List<AnalysisState> AnalaysisStates { get; set; }
        public bool IncludeStopwords { get; set; }
        public HttpPostedFileBase SomeFile { get; set; }
        public List<LetterNode> FreqWords { get; set; }

    }
}