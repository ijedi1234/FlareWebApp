using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlareWebApp.FileLogic {
    public class LetterNode {

        private int count;
        private string nodeWord;
        private char letter;
        List<LetterNode> childLetters;

        public LetterNode() {
            count = 0;
            childLetters = new List<LetterNode>();
        }

        public LetterNode(char nextChar) {
            letter = nextChar;
            count = 0;
            childLetters = new List<LetterNode>();
        }

        public LetterNode(int count, string word) {
            this.count = count; nodeWord = word;
        }

        //Should this node be reported? If the count is zero, the word is too neglibible to report, as it would not have appeared in the file.
        public bool IsRecordedWord() { return count != 0; }

        //A phase of adding a new word. This method is called once per char to make each node or reuse a node where applicable.
        public void AddWord(string writtenWord, string wordRemaining) {
            nodeWord = writtenWord;
            //If at the end, the traversal is done.
            if (wordRemaining.Length == 0) {
                count++;
                return;
            }
            //Grab the next letter from remaining for node logic.
            char nextLetter = wordRemaining.First();
            //Grab the applicable child. If not found, build the node. In either case, add the word, advanced one letter, through the child.
            LetterNode child = childLetters.FirstOrDefault(i => i.letter == nextLetter);
            if (child == null) {
                child = new LetterNode(nextLetter);
                childLetters.Add(child);
            }
            child.AddWord(writtenWord + nextLetter, wordRemaining.Substring(1));
        }

        //Get all words eith a count of at least one. Filtering is done in WordFile.
        public List<LetterNode> GetWords() {
            List<LetterNode> nodes = new List<LetterNode>();
            foreach (LetterNode node in childLetters) {
                if (node.IsRecordedWord()) nodes.Add(node);
                List<LetterNode> childNodes = node.GetWords();
                nodes.AddRange(childNodes);
            }
            return nodes;
        }

        public int GetCount() { return count; }
        public string GetNodeWord() { return nodeWord; }

    }
}