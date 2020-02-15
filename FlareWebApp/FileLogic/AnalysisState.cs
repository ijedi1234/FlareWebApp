using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace FlareWebApp.FileLogic {
    public class AnalysisState {

        private string uploadedFileName;
        private bool includeStopwords;
        private List<LetterNode> nodes;

        //Creates an analysis state. A FileHandler method is used to read the analysis lines.
        public AnalysisState(string filePathResults) {
            string[] results = File.ReadAllLines(filePathResults);
            nodes = new List<LetterNode>();
            //Reads the header lines.
            uploadedFileName = results[0];
            includeStopwords = bool.Parse(results[1]);
            for (int i = 2; i < results.Length; i++) {
                LetterNode node = FileHandler.ReadPostProcessLine(results[i]);
                nodes.Add(node);
            }
        }

        public string GetUploadedFilename() { return uploadedFileName; }
        //Instead of the boolean, this function returns the boolean as formatted text for the user.
        public string GetIncludeStopwords() {
            if (includeStopwords) return "Includes Stopwords";
            else return "Does not include Stopwords";
        }
        public List<LetterNode> GetResults() { return nodes; }

    }
}