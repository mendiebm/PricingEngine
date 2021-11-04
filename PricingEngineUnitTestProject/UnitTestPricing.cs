using Microsoft.VisualStudio.TestTools.UnitTesting;
using PricingEngine;
using System;
using System.Collections.Generic;

namespace PricingEngineUnitTestProject
{
    [TestClass]
    public class UnitTestPricing
    {
        private static Pricing pricing;
        private static Dictionary<string, decimal> stockKeepingUnits;
        private static List<Rule> rules;

        [ClassInitialize]
        public static void InitializePricingEngine(TestContext context)
        {
            stockKeepingUnits = new Dictionary<string, decimal>
            {
                {"A", 50 },
                {"B", 30},
                {"C", 20},
                {"D", 15}
            };

            FixedPriceDiscountRule multiBuyA = new FixedPriceDiscountRule(                
                affectedSku: "A", 
                requiredQuantity: 3, 
                price: 130, 
                stockKeepingUnits: stockKeepingUnits);
            FixedPriceDiscountRule multiBuyB = new FixedPriceDiscountRule(
                affectedSku: "B", 
                requiredQuantity: 2, 
                price: 45, 
                stockKeepingUnits: stockKeepingUnits);
            ComboBuyDiscountRule comboBuyCD = new ComboBuyDiscountRule(
                new List<string> { "C", "D" }, 
                price: 30, 
                stockKeepingUnits: stockKeepingUnits);

            rules = new List<Rule> { multiBuyA, multiBuyB, comboBuyCD };
            pricing = new Pricing(stockKeepingUnits, rules);
        }

        [TestMethod]
        public void TestMethodNoAppliedPromotions1()
        {
            Dictionary<string, uint> lineItems = new Dictionary<string, uint>
            {
                { "A", 1 }, 
                { "B", 1 }, 
                { "C", 1 } 
            };            
            
            Assert.AreEqual(100, pricing.CalculateTotal(lineItems), "No promotions should take effect");
        }

        [TestMethod]
        public void TestMethodNoAppliedPromotions2()
        {
            Dictionary<string, uint> lineItems = new Dictionary<string, uint>
            {
                { "A", 2 }, 
                { "B", 1 }, 
                { "C", 2 } 
            };

            Assert.AreEqual(170, pricing.CalculateTotal(lineItems), "No promotions should take effect");
        }

        [TestMethod]
        public void TestMethodMultiBuyAB()
        {
            Dictionary<string, uint> lineItems = new Dictionary<string, uint>
            {
                { "A", 5 },
                { "B", 5 },
                { "C", 1 }
            };

            Assert.AreEqual(370, pricing.CalculateTotal(lineItems), "Promotions A and B should take effect");
        }

        [TestMethod]
        public void TestMethodMultiBuyABComboBuyCD()
        {
            Dictionary<string, uint> lineItems = new Dictionary<string, uint>()
            {
                { "A", 3 },
                { "B", 5 },
                { "C", 1 },
                { "D", 1 }
            };

            Assert.AreEqual(280, pricing.CalculateTotal(lineItems), "Multibuy A and B, and combo C and D should take effect");
        }

        [TestMethod]
        public void TestMethodUnknownItem()
        {
            Dictionary<string, uint> lineItems = new Dictionary<string, uint>()
            {
                { "A", 3 },
                { "B", 5 },
                { "C", 1 },
                { "D", 1 },
                { "E", 1 }
            };

            ArgumentOutOfRangeException exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => pricing.CalculateTotal(lineItems));
            Assert.AreEqual("Line item E is not a valid stock keeping unit\r\nParameter name: lineItems", exception.Message);
        }

        [TestMethod]
        public void TestMethodUnknownItems()
        {
            Dictionary<string, uint> lineItems = new Dictionary<string, uint>()
            {
                { "A", 3 },
                { "B", 5 },
                { "C", 1 },
                { "D", 1 },
                { "E", 1 },
                { "F", 1 }
            };

            ArgumentOutOfRangeException exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => pricing.CalculateTotal(lineItems));
            Assert.AreEqual("Line items E, F are not valid stock keeping units\r\nParameter name: lineItems", exception.Message);
        }

        [TestMethod]
        public void TestMethodInvalidStockKeepingItems()
        {
            ArgumentNullException exception = Assert.ThrowsException<ArgumentNullException>(() => new Pricing(null, rules));
            Assert.AreEqual("Value cannot be null.\r\nParameter name: stockKeepingUnits", exception.Message);
        }

        [TestMethod]
        public void TestMethodInvalidRules()
        {
            ArgumentNullException exception = Assert.ThrowsException<ArgumentNullException>(() => new Pricing(new Dictionary<string, decimal>(), null));
            Assert.AreEqual("Value cannot be null.\r\nParameter name: rules", exception.Message);
        }
    }
}
