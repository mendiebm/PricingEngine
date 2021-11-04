using System.Collections.Generic;
using System.Linq;

namespace PricingEngine
{
    /// <summary>
    ///  A rule representing a percentage reduction on the price of a give item. 
    ///  For example, if A is usually 50, a rule with a %5 reduction would provide each unit of A at 45
    ///  
    /// </summary>
    public class PercentageDiscountRule : Rule
    {
        private readonly decimal percentageReduction;
        private readonly decimal standardPrice;

        /// <summary>
        /// Create an instance of a percentage discount price rule
        /// </summary>
        /// <param name="affectedSku">
        /// The SKU for which the rule takes effect
        /// </param>
        /// <param name="percentageReduction">
        /// The percentage reduction per unit of <paramref name="affectedSku"/>
        /// </param>
        /// <param name="stockKeepingUnits">
        /// A list of the SKUs. 
        /// Note that the standard price is obtained from this list at creation time and is not dynamic.
        /// </param>
        public PercentageDiscountRule(string affectedSku, decimal percentageReduction, Dictionary<string, decimal> stockKeepingUnits) : base(new List<string> { affectedSku }, stockKeepingUnits)
        {
            standardPrice = stockKeepingUnits[affectedSku];
            this.percentageReduction = percentageReduction;
        }

        /// <summary>
        /// <see cref="Rule.CalculateTotal(ref Dictionary{string, uint})"/>
        /// </summary>
        /// <param name="lineItems"></param>
        /// <returns></returns>
        public override decimal CalculateTotal(ref Dictionary<string, uint> lineItems)
        {
            decimal total = 0;

            while (true)
            {
                string affectedSku = AffectedSkus.First();
                if (lineItems[affectedSku] >= 1)
                {
                    lineItems[affectedSku] = lineItems[affectedSku] - 1;
                    total = total + (standardPrice * (1 - percentageReduction / 100));
                }
                else
                {
                    break;
                }
            }

            return total;
        }
    }
}
