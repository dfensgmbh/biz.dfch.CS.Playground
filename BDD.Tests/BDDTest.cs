using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BDD.Tests
{
    [TestClass]
    public class BDDTest
    {
        [TestMethod]
        public void TheAnswerToEverythingReturns42()
        {
            Assert.AreEqual(42, BDD.getTheAnswer());
        }
    }
}
