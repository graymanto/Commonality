using System.Collections.Generic;
using System.Linq;
using Commonality.Main.Extensions;

namespace Commonality.Main.DataStructures
{
    /// <summary>
    /// A ternary tree designed to hold words and look up completions for partial matches.
    /// </summary>
    public class TernaryTree
    {
        private Node _rootNode;

        /// <summary>
        /// Adds the given word to the tree
        /// </summary>
        /// <param name="wordToAdd">The word to add to the tree</param>
        public void Add(string wordToAdd)
        {
            Add(wordToAdd, 0, ref _rootNode);
        }

        /// <summary>
        /// Adds a list of words to the tree. 
        /// </summary>
        /// <param name="words">List of words to add</param>
        public void Add(IEnumerable<string> words)
        {
            // Sort randomly to balance tree
            words.OrderByRandom().ForEach(Add);
        }

        /// <summary>
        /// Indicates if the tree contains the given word
        /// </summary>
        /// <param name="word">The word being searched for</param>
        /// <returns>If the tree contains the given word</returns>
        public bool ContainsWord(string word)
        {
            return WordsFromPartial(word).Contains(word);
        }

        /// <summary>
        /// Returns a list of full word matchs from the tree given a partial string.
        /// </summary>
        /// <param name="partial">The partial word.</param>
        /// <returns>A list of partial words</returns>
        public IEnumerable<string> WordsFromPartial(string partial)
        {
            int index = 0;
            var currentNode = _rootNode;
            while (currentNode != null)
            {
                if (partial[index] < currentNode.Char)
                    currentNode = currentNode.Left;
                else if (partial[index] > currentNode.Char)
                    currentNode = currentNode.Right;
                else
                {
                    if (++index == partial.Length)
                    {
                        var wordList = new List<string>();
                        if (currentNode.IsEndOfWord)
                            wordList.Add(partial);
                        GetWordCompletionsFromNode(wordList, partial, currentNode.Middle);
                        return wordList;
                    }

                    currentNode = currentNode.Middle;
                }
            }

            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Adds a word to the tree
        /// </summary>
        /// <param name="wordToAdd">The word to add</param>
        /// <param name="index">The index pointing at the letter of the word currently being added</param>
        /// <param name="node">The node for the letter being added</param>
        private static void Add(string wordToAdd, int index, ref Node node)
        {
            if (node == null)
                node = new Node { Char = wordToAdd[index], IsEndOfWord = false };

            if (wordToAdd[index] < node.Char)
                Add(wordToAdd, index, ref node.Left);
            else if (wordToAdd[index] > node.Char)
                Add(wordToAdd, index, ref node.Right);
            else
            {
                if (index + 1 == wordToAdd.Length)
                    node.IsEndOfWord = true;
                else
                    Add(wordToAdd, index + 1, ref node.Middle);
            }
        }

        /// <summary>
        /// Finds complete words given a starting node and a partial match
        /// </summary>
        /// <param name="words">List of completed words found by this method</param>
        /// <param name="wordSoFar">The partial word being worked on</param>
        /// <param name="node">The current working node</param>
        private static void GetWordCompletionsFromNode(ICollection<string> words, string wordSoFar, Node node)
        {
            if (node == null)
                return;

            if (node.IsEndOfWord)
            {
                words.Add(wordSoFar + node.Char);
            }

            GetWordCompletionsFromNode(words, wordSoFar, node.Left);
            GetWordCompletionsFromNode(words, wordSoFar, node.Right);
            GetWordCompletionsFromNode(words, wordSoFar + node.Char, node.Middle);
        }
    }
}