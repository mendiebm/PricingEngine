using Microsoft.VisualStudio.TestTools.UnitTesting;
using PricingEngine;
using System;
using System.Collections.Generic;

namespace PricingEngineUnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        private static Pricing pricing;
        private static Dictionary<string, decimal> stockKeepingUnits;

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
                new[] { new FixedPriceDiscountRule.AffectedSku("A", 3) }, 
                130, 
                stockKeepingUnits);
            FixedPriceDiscountRule multiBuyB = new FixedPriceDiscountRule(
                new[] { new FixedPriceDiscountRule.AffectedSku("B", 2) }, 
                45, 
                stockKeepingUnits);
            FixedPriceDiscountRule comboBuyCD = new FixedPriceDiscountRule(
                new[] 
                {
                    new FixedPriceDiscountRule.AffectedSku("C", 1),
                    new FixedPriceDiscountRule.AffectedSku("D", 1) 
                }, 
                30, 
                stockKeepingUnits);

            pricing = new Pricing(stockKeepingUnits, new[] { multiBuyA, multiBuyB, comboBuyCD });
        }

        [TestMethod]
        public void TestMethodNoAppliedPromotions()
        {
            LineItem[] lineItems = new[] { new LineItem("A", 1, stockKeepingUnits), new LineItem("B", 1, stockKeepingUnits), new LineItem("C", 1, stockKeepingUnits) };            
            Assert.AreEqual(100, pricing.CalculateTotal(lineItems), "");
        }

        [TestMethod]
        public void TestMethodMultiBuyAB()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TestMethodComboBuyCD()
        {
            Assert.Inconclusive();
        }
    }
}
