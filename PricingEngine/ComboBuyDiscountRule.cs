using System;
using System.Collections.Generic;
using System.Linq;

namespace PricingEngine
{
    /// <summary>
    /// Represents a rule where a bundle of items is a different price to the price if these were bought individually
    /// For example, if item C costs 50, and D costs 50, the rule could provide C and D if bought together for 80
    /// </summary>
    public class ComboBuyDiscountRule : Rule
    {
        internal decimal Price
        {
            get; private set;
        }

        /// <summary>
        /// Create an instance of a combination price rule
        /// </summary>
        /// <param name="affectedSkus">
        /// A list of SKUs that must all be present for the rule to take effect
        /// Note that duplicates will be filtered out.
        /// </param>
        /// <param name="price">
        /// The price for one each of <paramref name="affectedSkus"/>
        /// </param>
        /// <param name="stockKeepingUnits">
        /// The known stock keeping units, used for validation
        /// </param>
        public ComboBuyDiscountRule(IEnumerable<string> affectedSkus, decimal price, Dictionary<string, decimal> stockKeepingUnits) : base(affectedSkus, stockKeepingUnits)
        {
            if (price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be less than 0");
            }

            Price = price;
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
