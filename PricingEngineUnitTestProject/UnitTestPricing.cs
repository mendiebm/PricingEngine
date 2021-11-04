using Microsoft.VisualStudio.TestTools.UnitTesting;
using PricingEngine;
using System;
using System.Collections.Generic;

namespace PricingEngineUnitTestProject
{
    [TestClass]
    public class UnitTestPricing
    {        
        private static Dictionary<string, decimal> stockKeepingUnits = new Dictionary<string, decimal>
            {
                {"A", 50},
                {"B", 30},
                {"C", 20},
                {"D", 15}
            };

        private static FixedPriceDiscountRule multiBuyA = new FixedPriceDiscountRule(
            affectedSku: "A",
            requiredQuantity: 3,
            price: 130,
            stockKeepingUnits: stockKeepingUnits);
        private static FixedPriceDiscountRule multiBuyB = new FixedPriceDiscountRule(
            affectedSku: "B",
            requiredQuantity: 2,
            price: 45,
            stockKeepingUnits: stockKeepingUnits);
        private static ComboBuyDiscountRule comboBuyCD = new ComboBuyDiscountRule(
            affectedSkus: new List<string> { "C", "D" },
            price: 30,
            stockKeepingUnits: stockKeepingUnits);
        private static PercentageDiscountRule percentageDiscountRuleA = new PercentageDiscountRule(
            affectedSku : "A",
            percentageReduction: 10,            
            stockKeepingUnits: stockKeepingUnits);

        [TestMethod]
        public void TestMethodPercentageReductionRuleLast()
        {
            List<Rule> rules = new List<Rule> { multiBuyA, multiBuyB, comboBuyCD, percentageDiscountRuleA };
            Pricing pricing = new Pricing(stockKeepingUnits, rules);

            Dictionary<string, uint> lineItems = new Dictionary<string, uint>
            {
                { "A", 4 }, // 3 for 130, 1 at 45
                { "B", 1 }, // 1 at 30 
                { "C", 2 } // 2 at 20
            };

            Assert.AreEqual(130 + (1 * 45) + (1 * 30) + (2 * 20), pricing.CalculateTotal(lineItems), "3xA at 130 + 1xA at 45, 1xB at 30 and 2xC");
        }

        [TestMethod]
        public void TestMethodPercentageReductionRuleFirst()
        {
            List<Rule> rules = new List<Rule> { percentageDiscountRuleA, multiBuyA, multiBuyB, comboBuyCD };
            Pricing pricing = new Pricing(stockKeepingUnits, rules);

            Dictionary<string, uint> lineItems = new Dictionary<string, uint>
            {
                { "A", 4 }, // 4 at 45
                { "B", 1 }, // 1 at 30 
                { "C", 2 } // 2 at 20
            };

            Assert.AreEqual((4 * 45) + (1 * 30) + (2 * 20), pricing.CalculateTotal(lineItems), "4xA at 45 each, 1xB at 30 and 2xC");
        }

        [TestMethod]
        public void TestMethodNoAppliedPromotions1()
        {
            List<Rule> rules = new List<Rule> { multiBuyA, multiBuyB, comboBuyCD };
            Pricing pricing = new Pricing(stockKeepingUnits, rules);

            Dictionary<string, uint> lineItems = new Dictionary<string, uint>
            {
                { "A", 1 }, 
                { "B", 1 }, 
                { "C", 1 } 
            };            
            
            Assert.AreEqual(50 + 30 + 20, pricing.CalculateTotal(lineItems), "No promotions should take effect");
        }

        [TestMethod]
        public void TestMethodNoAppliedPromotions2()
        {
            List<Rule> rules = new List<Rule> { multiBuyA, multiBuyB, comboBuyCD };
            Pricing pricing = new Pricing(stockKeepingUnits, rules);

            Dictionary<string, uint> lineItems = new Dictionary<string, uint>
            {
                { "A", 2 }, 
                { "B", 1 }, 
                { "C", 2 } 
            };

            Assert.AreEqual((50 * 2) + 30 + (20 * 2) , pricing.CalculateTotal(lineItems), "No promotions should take effect");
        }

        [TestMethod]
        public void TestMethodMultiBuyAB()
        {
            List<Rule> rules = new List<Rule> { multiBuyA, multiBuyB, comboBuyCD };
            Pricing pricing = new Pricing(stockKeepingUnits, rules);

            Dictionary<string, uint> lineItems = new Dictionary<string, uint>
            {
                { "A", 5 }, // 3 for 130, 2 at 50 each 
                { "B", 5 }, // 2 of 2 for 45, 1 at 30
                { "C", 1 } // 1 at 15
            };

            Assert.AreEqual(370, pricing.CalculateTotal(lineItems), "Promotions A and B should take effect");
        }

        [TestMethod]
        public void TestMethodMultiBuyABComboBuyCD()
        {
            List<Rule> rules = new List<Rule> { multiBuyA, multiBuyB, comboBuyCD };
            Pricing pricing = new Pricing(stockKeepingUnits, rules);

            Dictionary<string, uint> lineItems = new Dictionary<string, uint>()
            {
                { "A", 3 }, // 3 for 130, 2 at 50 each 
                { "B", 5 }, // 2 of 2 for 45, 1 at 30
                { "C", 1 }, 
                { "D", 1 } // C and D for 30
            };

            Assert.AreEqual(280, pricing.CalculateTotal(lineItems), "Multibuy A and B, and combo C and D should take effect");
        }

        [TestMethod]
        public void TestMethodUnknownItem()
        {
            List<Rule> rules = new List<Rule> { multiBuyA, multiBuyB, comboBuyCD };
            Pricing pricing = new Pricing(stockKeepingUnits, rules);

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
            List<Rule> rules = new List<Rule> { multiBuyA, multiBuyB, comboBuyCD };
            Pricing pricing = new Pricing(stockKeepingUnits, rules);

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
            List<Rule> rules = new List<Rule> { multiBuyA, multiBuyB, comboBuyCD };
            
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
