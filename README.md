# FlareWebApp

This web application is written in C# ASP.Net using Visual Studio 2013. It provides the following features:
1. This application can accept a text file from a client's computer and derive the word counts in the text file. Words within words are not counted.
2. The 9 results before the most recent analysis are also provided alongside the most recent analysis. To clear these previous analyses, go to the FlareWebApp/SavedData folder and clear all files.
3. The extensions of the words "talk", "play", "pass", and "copy" are all reduced to their stem forms before processing.
4. Words are compared against a list of stopwords. If the user has decided to include stopwords, stopwords will be removed. These stopwords are retrived from disk. The stopwords were pulled from the list "Long Stopword List" at https://www.ranks.nl/stopwords

The libraries used to create this project are ASP.Net, MVC, Razor, and Bootstrap.

The original file uploaded is saved under SavedData as FileN.txt, where N stands for the newest attempt, up to 10. If N is found to be greater than 10, file1.txt and its result file are deleted, and the other files are shifted down one N to make room for the new file10.txt. Upon generating a file, its result file is also generated as fileNresult.txt. The result file holds the toggle value of whether or not stopwords are included, the uploaded file name, and a cached record of the corresponding analysis.

The counting itself is handled via a trie. The root node begins at empty string, and expands on letter per level. When a new word enters the system, it traverses the existing graph until it reaches its last letter, creating new nodes where necessary to hold each of its letters. Upon reaching the end of the traversal, the node's count is incremented. When all words are processed, the entire trie is traversed, with each node placed into a list. LINQ orders by the count descending, and then by string compare on the word ascending. Once this sorting is complete, up to 25 elements are taken. This node list is then sent to the controller to be integrated into the list of analyses.
