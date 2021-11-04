using Microsoft.VisualStudio.TestTools.UnitTesting;
using PricingEngine;
using System;
using System.Collections.Generic;

namespace PricingEngineUnitTestProject
{
    [TestClass]
    public class UnitTestFixedPriceDiscountRule
    {
        [TestMethod]
        public void TestMethodInvalidPrice()
        {
            ArgumentOutOfRangeException exception =
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => new FixedPriceDiscountRule(
                affectedSku: "C",
                requiredQuantity: 2,
                price: -30,
                stockKeepingUnits: new Dictionary<string, decimal>
                {
                    { "C", 5 },
                    { "D", 5 }
                }));

            Assert.AreEqual("Price cannot be less than 0\r\nParameter name: price", exception.Message);
        }

        [TestMethod]
        public void TestMethodInvalidSku()
        {
            ArgumentOutOfRangeException exception =
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => new FixedPriceDiscountRule(
                affectedSku: string.Empty,
                requiredQuantity: 2,
                price: 30,
                stockKeepingUnits: new Dictionary<string, decimal>
                {
                    { "C", 5 },
                    { "D", 5 }
                }));

            Assert.AreEqual("Unknown line item SKU ''\r\nParameter name: affectedSkus", exception.Message);
        }
    }
}
