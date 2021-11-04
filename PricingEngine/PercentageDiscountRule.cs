using System.Collections.Generic;
using System.Linq;

namespace PricingEngine
{
    public class PercentageDiscountRule : Rule
    {
        private readonly decimal percentageReduction;
        private readonly decimal standardPrice;

        public PercentageDiscountRule(string affectedSku, decimal percentageReduction, Dictionary<string, decimal> stockKeepingUnits) : base(new List<string> { affectedSku }, stockKeepingUnits)
        {
            standardPrice = stockKeepingUnits[affectedSku];
            this.percentageReduction = percentageReduction;
        }

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
