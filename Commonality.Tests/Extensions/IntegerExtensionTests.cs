using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commonality.Main.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commonality.Tests.Extensions
{
    [TestClass]
    public class IntegerExtensionTests
    {
        [TestMethod]
        public void SequenceToTest()
        {
            var collection = 1.SequenceTo(10);

            Assert.AreEqual(collection.Count(), 10);

            for (int i = 1; i <= 10; i++)
            {
                Assert.AreEqual(collection.ElementAt(i - 1), i);
            }
        }

        [TestMethod]
        public void TimesTest()
        {
            int i = 0;

            5.Times(index => i++);
            Assert.AreEqual(i, 5);

            i = 0;
            5.Times(() => i++);
            Assert.AreEqual(i, 5);
        }
    }
}
