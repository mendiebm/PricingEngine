using Microsoft.VisualStudio.TestTools.UnitTesting;
using PricingEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngineUnitTestProject
{
    [TestClass]
    public class UnitTestRule
    {
        [TestMethod]
        public void TestMethodNullAffectedSkus()
        {
            ArgumentNullException exception =
                Assert.ThrowsException<ArgumentNullException>(() => new ComboBuyDiscountRule(
                null,
                price: 30,
                stockKeepingUnits: new Dictionary<string, decimal>
                {
                    { "C", 5 },
                    { "D", 5 }
                }));
            Assert.AreEqual("Affected SKUs must be provided\r\nParameter name: affectedSkus", exception.Message);
        }

        [TestMethod]
        public void TestMethodEmptyAffectedSkus()
        {
            ArgumentNullException exception =
                Assert.ThrowsException<ArgumentNullException>(() => new ComboBuyDiscountRule(
                new List<string>(),
                price: 30,
                stockKeepingUnits: new Dictionary<string, decimal>
                {
                    { "C", 5 },
                    { "D", 5 }
                }));
            Assert.AreEqual("Affected SKUs must be provided\r\nParameter name: affectedSkus", exception.Message);
        }

        [TestMethod]
        public void TestMethodNullStockKeepingUnits()
        {
            ArgumentNullException exception =
                Assert.ThrowsException<ArgumentNullException>(() => new ComboBuyDiscountRule(
                new List<string> { "C", "D"},
                price: 30,
                stockKeepingUnits: null));
            Assert.AreEqual("Stock keeping units must be provided\r\nParameter name: stockKeepingUnits", exception.Message);
        }

        [TestMethod]
        public void TestMethodEmptyStockKeepingUnits()
        {
            ArgumentNullException exception =
                Assert.ThrowsException<ArgumentNullException>(() => new ComboBuyDiscountRule(
                new List<string> { "C", "D" },
                price: 30,
                stockKeepingUnits: new Dictionary<string, decimal>()));
            Assert.AreEqual("Stock keeping units must be provided\r\nParameter name: stockKeepingUnits", exception.Message);
        }
    }
}
