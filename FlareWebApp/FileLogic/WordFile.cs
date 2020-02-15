using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

namespace FlareWebApp.FileLogic {
    public class WordFile {

        private const int topNFreq = 25;
        private const string stopwordsFilename = "stopwords.txt";


        LetterNode nodeGraph;
        private Dictionary<string, string[]> stemToPostfixes;

        //This constructor reads a designated source file and constructs a node graph from it.
        public WordFile(string filepath, string stopwordDir, bool includeStopwords) {
            stemToPostfixes = GenerateStemToPostfixes();
            string fileText = File.ReadAllText(filepath);
            fileText = SanitizeText(fileText);
            string[] words = fileText.Split(' ');
            for (int i = 0; i < words.Length; i++) {
                words[i] = SanitizeWord(words[i], stopwordDir, includeStopwords);
            }
            nodeGraph = new LetterNode();
            foreach (string word in words) {
                if(word != "")
                    nodeGraph.AddWord("", word);
            }
        }

        //Get the words for the node list -> order the list by counts descending (highest first) -> order by node word ascending (when counts are the same) -> take top 25 elements
        public List<LetterNode> GetMostFrequentWords() {
            List<LetterNode> nodes = nodeGraph.GetWords().OrderByDescending(i => i.GetCount()).ThenBy(i => i.GetNodeWord()).Take(topNFreq).ToList();
            return nodes;
        }

        //Due to the simplified case, it was decided to create a small map to hold the correct word to its variations. This was done to not corrupt other words.
        private static Dictionary<string, string[]> GenerateStemToPostfixes() {
            Dictionary<string, string[]> map = new Dictionary<string,string[]>();
            string[] talkPostfix = { "talks", "talking", "talked" };
            string[] playPostfix = { "plays", "playing", "played" };
            string[] passPostfix = { "passes", "passing", "passed" };
            string[] copyPostfix = { "copies", "copying", "copied" };
            map.Add("talk", talkPostfix); map.Add("play", playPostfix);
            map.Add("pass", passPostfix); map.Add("copy", copyPostfix);
            return map;
        }

        //After the general text is formatted, the words themselves need sanitizing. This method forces every word to lower case for safety, purifies special words to their stem, and removes stop words if need be.
        private string SanitizeWord(string inputWord, string stopwordDir, bool includeStopwords) {
            string word = inputWord.ToLower();

            //Remove stem test cases
            foreach (var pair in stemToPostfixes) {
                if (pair.Value.Contains(word)) word = pair.Key;
            }

            if (!includeStopwords) {
                //Remove stop words
                string[] lines = File.ReadAllLines(stopwordDir + "\\" + stopwordsFilename);
                if (lines.Contains(word)) word = "";
            }

            return word;
        }

        //Remove line breaks, tabs, and anything that isn't an alpha. Then, condense adjacent spaces to one space.
        private static string SanitizeText(string fileText) {
            fileText = fileText.Replace("\n", " ");
            fileText = fileText.Replace("\r", " ");
            fileText = fileText.Replace("\t", " ");
            fileText = Regex.Replace(fileText, "[^A-Za-z ]", "");

            fileText = Regex.Replace(fileText, @"\s+", " ");
            return fileText;
        }

    }
}