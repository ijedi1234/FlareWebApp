using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace FlareWebApp.FileLogic {
    public class FileSaver {

        private const int maxSavedFiles = 10;
        private const string filePrefix = "file";
        private const string fileExtension = ".txt";
        private const string fileExtensionResults = "results.txt";

        private string saveDir;
        private bool includeStopwords;
        private string uploadedFileName;
        private string filePathOriginal;
        private string filePathResults;

        public FileSaver(HttpPostedFileBase fileHTTP, string saveDir, bool includeStopwords) {
            this.uploadedFileName = Path.GetFileName(fileHTTP.FileName);
            this.saveDir = saveDir; this.includeStopwords = includeStopwords;
            SaveFile(fileHTTP);
        }

        public static List<AnalysisState> GetAnalysisStates(string saveDir) {
            List<AnalysisState> states = new List<AnalysisState>();
            for (int i = 1; i <= maxSavedFiles; i++) {
                string filePathResults = GenerateResultsFilePath(saveDir, i);
                if (!File.Exists(filePathResults)) continue;
                AnalysisState state = new AnalysisState(filePathResults);
                states.Add(state);
            }
            return states;
        }

        public string SaveFile(HttpPostedFileBase fileHTTP) {
            string filename = Path.GetFileName(fileHTTP.FileName);
            int fileNumber = DetermineSaveFileNumber();
            filePathOriginal = GenerateFilePath(saveDir, GenerateFileName(fileNumber));
            filePathResults = GenerateResultsFilePath(saveDir, fileNumber);
            CreatePreProcessFiles(fileHTTP, filePathResults);
            return filePathOriginal;
        }

        private int DetermineSaveFileNumber() {
            int i;
            for (i = 1; i <= maxSavedFiles; i++) {
                string potentialFilename = GenerateFileName(i);
                string potentialFilepath = GenerateFilePath(saveDir, potentialFilename);
                if (!File.Exists(potentialFilepath)) return i;
            }
            MovePreviousFiles();
            return maxSavedFiles;
        }

        private void MovePreviousFiles() {
            for (int i = 2; i <= maxSavedFiles; i++) {
                string srcFile = GenerateFilePath(saveDir, i);
                string srcFileResults = GenerateResultsFilePath(saveDir, i);
                string destFile = GenerateFilePath(saveDir, i - 1);
                string destFileResults = GenerateResultsFilePath(saveDir, i - 1);
                MoveFile(srcFile, destFile);
                MoveFile(srcFileResults, destFileResults);
            }
        }

        private void CreatePreProcessFiles(HttpPostedFileBase fileHTTP, string filePathResults) {
            fileHTTP.SaveAs(filePathOriginal);
        }

        public void AppendPostProcessInfo(List<LetterNode> nodes, bool includeStopwords) {
            if (File.Exists(filePathResults)) File.Delete(filePathResults);
            using (StreamWriter writer = File.CreateText(filePathResults)) {
                writer.WriteLine(uploadedFileName);
                writer.WriteLine(includeStopwords);
                foreach (LetterNode node in nodes) {
                    writer.WriteLine(node.GetNodeWord() + ',' + node.GetCount());
                }
            }
        }

        public void WritePostProcessLine(StreamWriter writer, LetterNode node) {
            writer.WriteLine(node.GetNodeWord() + ',' + node.GetCount());
        }

        public static LetterNode ReadPostProcessLine(string line) {
            int comma = line.IndexOf(',');
            string word = line.Substring(0, comma);
            int count = int.Parse(line.Substring(comma + 1));
            return new LetterNode(count, word);
        }

        private static void MoveFile(string srcPath, string destPath) {
            if (File.Exists(destPath)) File.Delete(destPath);
            if (File.Exists(srcPath)) File.Move(srcPath, destPath);
        }

        private static string GenerateFileName(int number) {
            return filePrefix + number.ToString() + fileExtension;
        }

        private static string GenerateResultsFileName(int number) {
            return filePrefix + number.ToString() + fileExtensionResults;
        }

        private static string GenerateFilePath(string saveDir, int number) {
            return saveDir + "\\" + GenerateFileName(number);
        }

        private static string GenerateFilePath(string saveDir, string saveFile) {
            return saveDir + "\\" + saveFile;
        }

        private static string GenerateResultsFilePath(string saveDir, int number) {
            return saveDir + "\\" + GenerateResultsFileName(number);
        }

        public string GetFilePathOriginal() { return filePathOriginal; }

    }
}