using System;
using System.Collections.Generic;
using System.Linq;

namespace PricingEngine
{
    /// <summary>
    /// A rule representing a fixed price for multiple units of the same SKU. 
    /// For example, if A is normally 50, but 3 are bought, the price for these 3 would be 120
    /// </summary>
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

        /// <summary>
        /// Create an instance of a fixed price rule
        /// </summary>
        /// <param name="affectedSku">
        /// The SKU for which the rule takes effect
        /// </param>
        /// <param name="requiredQuantity">
        /// The number of items of <paramref name="affectedSku"/> that are required to invoke the rule
        /// </param>
        /// <param name="price">
        /// The price of <paramref name="requiredQuantity"/> items
        /// </param>
        /// <param name="stockKeepingUnits">
        /// A list of the known SKUs, for validation
        /// </param>
        public FixedPriceDiscountRule(string affectedSku, uint requiredQuantity, decimal price, Dictionary<string, decimal> stockKeepingUnits) : base(new List<string> { affectedSku }, stockKeepingUnits)
        {
            if (price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be less than 0");
            }

            // base rule already checks affected sku is valid            

            Price = price;
            RequiredQuantity = requiredQuantity;
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
