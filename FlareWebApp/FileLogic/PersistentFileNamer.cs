using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlareWebApp.FileLogic {
    public class PersistentFileNamer {

        private const string filePrefix = "file";
        private const string fileOriginalExtension = ".txt";
        private const string fileResultsExtension = "results.txt";

        private string filesLocation;

        public PersistentFileNamer(string filesLocation) {
            this.filesLocation = filesLocation;
        }

        //Functions to handle the variance of filename or filepath requests for original or results.
        public string GenerateOriginalFileName(int number) {
            return filePrefix + number.ToString() + fileOriginalExtension;
        }

        public string GenerateResultsFileName(int number) {
            return filePrefix + number.ToString() + fileResultsExtension;
        }

        public string GenerateFilePath(int number) {
            return filesLocation + "\\" + GenerateOriginalFileName(number);
        }

        public string GenerateFilePath(string saveFile) {
            return filesLocation + "\\" + saveFile;
        }

        public string GenerateResultsFilePath(int number) {
            return filesLocation + "\\" + GenerateResultsFileName(number);
        }

    }
}