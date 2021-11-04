using Microsoft.VisualStudio.TestTools.UnitTesting;
using PricingEngine;
using System;
using System.Collections.Generic;

namespace PricingEngineUnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [ClassInitialize]
        public static void InitializePricingEngine(TestContext context)
        {
            List<StockKeepingUnit> stockKeepingUnits = new List<StockKeepingUnit>
            {
                new StockKeepingUnit("A", 50),
                new StockKeepingUnit("B", 30),
                new StockKeepingUnit("C", 20),
                new StockKeepingUnit("D", 15)
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

            Pricing pricing = new Pricing(stockKeepingUnits, new[] { multiBuyA, multiBuyB, comboBuyCD });
        }

        [TestMethod]
        public void TestMethod1()
        {
            
        }
    }
}
