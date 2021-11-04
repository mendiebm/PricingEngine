using System;
using System.Collections.Generic;
using System.Linq;

namespace PricingEngine
{
    public class ComboBuyDiscountRule : Rule
    {
        internal decimal Price
        {
            get; private set;
        }

        public ComboBuyDiscountRule(IEnumerable<string> affectedSkus, decimal price, Dictionary<string, decimal> stockKeepingUnits) : base(affectedSkus, stockKeepingUnits)
        {
            if (price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be less than 0");
            }

            Price = price;
        }

        public override decimal CalculateTotal(ref Dictionary<string, uint> lineItems)
        {
            decimal total = 0;
            while (true)
            {
                // at least one of every item in the combo rule must be present for the rule to take effect
                if (lineItems
                    .Where(l => AffectedSkus.Contains(l.Key))
                    .All(l => l.Value >= 1))
                {
                    foreach (string ruleSku in AffectedSkus)
                    {
                        lineItems[ruleSku] = lineItems[ruleSku] - 1;
                    }

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
