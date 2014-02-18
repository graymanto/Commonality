using System.Collections.Generic;
using System.Linq;
using Commonality.Main.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commonality.Tests.DataStructures
{
    /// <summary>
    /// Unit tests for the ternery tree
    /// </summary>
    [TestClass]
    public class TreeTest
    {
        [TestMethod]
        public void NothingInTreeTest()
        {
            var tree = new TernaryTree();

            Assert.IsFalse(tree.ContainsWord("anything"));

            var emptyMatchTest = tree.WordsFromPartial("anythingelse");
            Assert.IsTrue(emptyMatchTest.Count() == 0);
        }

        [TestMethod]
        public void FullWordAddTest()
        {
            var tree = new TernaryTree();

            var testString = "test";
            tree.Add(testString);

            Assert.IsTrue(tree.ContainsWord(testString));
        }

        [TestMethod]
        public void MultipleWordTest()
        {
            var testList = new List<string> 
            {
                "some",
                "other",
                "words"
            };

            var tree = new TernaryTree();
            tree.Add(testList);

            testList.ForEach(word => Assert.IsTrue(tree.ContainsWord(word)));
        }

        [TestMethod]
        public void MatchingWordsTest()
        {
            var testList = new List<string> 
            {
                "any",
                "above",
                "average",
                "abby",
                "bottom",
                "based"
            };

            var tree = new TernaryTree();
            tree.Add(testList);

            var testResult1 = tree.WordsFromPartial("a");

            Assert.IsTrue(testResult1 != null);
            Assert.IsTrue(testResult1.Count() == 4);
            Assert.IsTrue(testResult1.Contains("any") &&
                          testResult1.Contains("above") &&
                          testResult1.Contains("abby") &&
                          testResult1.Contains("average"));

            var testResult2 = tree.WordsFromPartial("ab");

            Assert.IsTrue(testResult2 != null);
            Assert.IsTrue(testResult2.Count() == 2);
            Assert.IsTrue(testResult2.Contains("abby") &&
                          testResult2.Contains("above"));

        }

    }
}
