using System;
using System.Collections.Generic;
using System.Linq;

namespace PricingEngine
{
    public class FixedPriceDiscountRule : Rule
    {
        internal decimal Price
        {
            get; private set;
        }

        internal uint RequiredQuantity
        {
            get; private set;
        }

        public FixedPriceDiscountRule(string affectedSku, uint requiredQuantity, decimal price, Dictionary<string, decimal> stockKeepingUnits) : base(new List<string> { affectedSku }, stockKeepingUnits)
        {
            if (price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be less than 0");
            }

            if (string.IsNullOrEmpty(affectedSku))
            {
                throw new ArgumentNullException(nameof(price), "Affected SKU must be provided");
            }

            if (!stockKeepingUnits.ContainsKey(affectedSku))
            {
                throw new ArgumentException($"Unknown line item SKU {affectedSku}");
            }

            Price = price;
            RequiredQuantity = requiredQuantity;
        }

        public override decimal CalculateTotal(ref Dictionary<string, uint> lineItems)
        {
            decimal total = 0;

            while (true)
            {
                string affectedSku = AffectedSkus.First();
                if (lineItems[affectedSku] >= RequiredQuantity)
                {
                    lineItems[affectedSku] = lineItems[affectedSku] - RequiredQuantity;
                    total = total + Price;
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
