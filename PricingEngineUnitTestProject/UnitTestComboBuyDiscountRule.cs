using Microsoft.VisualStudio.TestTools.UnitTesting;
using PricingEngine;
using System;
using System.Collections.Generic;

namespace PricingEngineUnitTestProject
{
    [TestClass]
    public class UnitTestComboBuyDiscountRule
    {
        [TestMethod]
        public void TestMethodInvalidPrice()
        {
            ArgumentOutOfRangeException exception = 
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ComboBuyDiscountRule(
                new List<string> { "C", "D" },
                price: -30,
                stockKeepingUnits: new Dictionary<string, decimal>
                {
                    { "C", 5 },
                    { "D", 5 }
                }));
            Assert.AreEqual("Price cannot be less than 0\r\nParameter name: price", exception.Message);
        }
    }
}
