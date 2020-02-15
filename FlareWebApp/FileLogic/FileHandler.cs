using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace FlareWebApp.FileLogic {
    public class FileHandler {

        private const int maxSavedFiles = 10;

        private PersistentFileNamer pfn;
        private bool includeStopwords;
        private string uploadedFileName;
        private string filePathOriginal;
        private string filePathResults;

        //A simpler constructor if a file was not uploaded.
        public FileHandler(string saveDir) {
            pfn = new PersistentFileNamer(saveDir);
        }

        //The constructor for saving the uploaded file.
        public FileHandler(HttpPostedFileBase fileHTTP, string saveDir, bool includeStopwords) {
            pfn = new PersistentFileNamer(saveDir);
            this.uploadedFileName = Path.GetFileName(fileHTTP.FileName);
            this.includeStopwords = includeStopwords;
            SaveFile(fileHTTP);
        }

        //This function retrives analyses from disk.
        public List<AnalysisState> GetAnalysisStates() {
            List<AnalysisState> states = new List<AnalysisState>();
            for (int i = 1; i <= maxSavedFiles; i++) {
                string filePathResults = pfn.GenerateResultsFilePath(i);
                if (!File.Exists(filePathResults)) continue;
                AnalysisState state = new AnalysisState(filePathResults);
                states.Add(state);
            }
            return states;
        }

        //Saves the uploaded file.
        public string SaveFile(HttpPostedFileBase fileHTTP) {
            int fileNumber = DetermineSaveFileNumber();
            filePathOriginal = pfn.GenerateFilePath(pfn.GenerateOriginalFileName(fileNumber));
            filePathResults = pfn.GenerateResultsFilePath(fileNumber);
            CreatePreProcessFiles(fileHTTP, filePathResults);
            return filePathOriginal;
        }

        //Determine what the next filename is going to be, for both the original file and the results one.
        private int DetermineSaveFileNumber() {
            int i;
            for (i = 1; i <= maxSavedFiles; i++) {
                string potentialFilename = pfn.GenerateOriginalFileName(i);
                string potentialFilepath = pfn.GenerateFilePath(potentialFilename);
                if (!File.Exists(potentialFilepath)) return i;
            }
            MovePreviousFiles();
            return maxSavedFiles;
        }

        //If there is no more space, shift downwards.
        private void MovePreviousFiles() {
            for (int i = 2; i <= maxSavedFiles; i++) {
                string srcFile = pfn.GenerateFilePath(i);
                string srcFileResults = pfn.GenerateResultsFilePath(i);
                string destFile = pfn.GenerateFilePath(i - 1);
                string destFileResults = pfn.GenerateResultsFilePath(i - 1);
                MoveFile(srcFile, destFile);
                MoveFile(srcFileResults, destFileResults);
            }
        }

        //Save the uploaded file
        private void CreatePreProcessFiles(HttpPostedFileBase fileHTTP, string filePathResults) {
            fileHTTP.SaveAs(filePathOriginal);
        }

        //Create the results file. Note this is done after the original file is made, and the node graph is created.
        public void AppendPostProcessInfo(List<LetterNode> nodes, bool includeStopwords) {
            if (File.Exists(filePathResults)) File.Delete(filePathResults);
            using (StreamWriter writer = File.CreateText(filePathResults)) {
                writer.WriteLine(uploadedFileName);
                writer.WriteLine(includeStopwords);
                foreach (LetterNode node in nodes) {
                    WritePostProcessLine(writer, node);
                }
            }
        }

        //Function for writing the results analysis.
        public void WritePostProcessLine(StreamWriter writer, LetterNode node) {
            writer.WriteLine(node.GetNodeWord() + ',' + node.GetCount());
        }

        //Read an analysis row as a LetterNode. Note that the LetterNode no longer remembers its children - it only knows enough to rebuild the cached data for the user.
        public static LetterNode ReadPostProcessLine(string line) {
            int comma = line.IndexOf(',');
            string word = line.Substring(0, comma);
            int count = int.Parse(line.Substring(comma + 1));
            return new LetterNode(count, word);
        }

        //Overwrite another file.
        private static void MoveFile(string srcPath, string destPath) {
            if (File.Exists(destPath)) File.Delete(destPath);
            if (File.Exists(srcPath)) File.Move(srcPath, destPath);
        }

        public string GetFilePathOriginal() { return filePathOriginal; }

    }
}