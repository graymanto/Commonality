using System;
using System.Collections.Generic;

using Commonality.Main.Extensions;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commonality.Tests.Extensions
{
    [TestClass]
    public class DictionaryExtensionsTests
    {
        [TestMethod]
        public void GetOrAddGetValueNotThereExpectInvokesFunc()
        {
            const string TestKey = "TestKey";
            const string ExpectedValue = "ExpectedValue";

            var dic = new Dictionary<string, string>();
            var returned = dic.GetOrAdd(TestKey, s => ExpectedValue);

            returned.Should().Be(ExpectedValue);
            dic.Should().ContainKey(TestKey);
            dic[TestKey].Should().Be(ExpectedValue);
        }

        [TestMethod]
        public void GetOrAddGetValueNotThereExpectAddsGivenValue()
        {
            const string TestKey = "TestKey";
            const string ExpectedValue = "ExpectedValue";

            var dic = new Dictionary<string, string>();
            var returned = dic.GetOrAdd(TestKey, ExpectedValue);

            returned.Should().Be(ExpectedValue);
            dic.Should().ContainKey(TestKey);
            dic[TestKey].Should().Be(ExpectedValue);
        }

        [TestMethod]
        public void GetOrDefaultValueNotPresentReturnsDefaultForType()
        {
            var dic = new Dictionary<string, string>();
            dic.GetOrDefault(Guid.NewGuid().ToString()).Should().Be(default(string));

            var intDic = new Dictionary<string, int>();
            intDic.GetOrDefault(Guid.NewGuid().ToString()).Should().Be(default(int));
        }
    }
}