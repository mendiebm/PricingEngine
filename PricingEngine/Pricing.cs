using System;
using System.Collections.Generic;
using System.Linq;

namespace PricingEngine
{
    public class Pricing
    {
        private readonly Dictionary<string, decimal> stockKeepingUnits;

        private readonly IEnumerable<Rule> rules;
        
        /// <summary>
        /// Create a pricing object for calculating the total of a list of items
        /// </summary>
        /// <param name="stockKeepingUnits">
        /// A list of the stock keeping units and the standard retail price for each
        /// </param>
        /// <param name="rules">
        /// A list of rules that applies to <paramref name="stockKeepingUnits"/>        
        /// These rules will be applied in the order provided.
        /// </param>
        /// <remarks>
        /// Rules referencing an item not in <paramref name="stockKeepingUnits"/> will be silently ignored
        /// </remarks>
        public Pricing(Dictionary<string, decimal> stockKeepingUnits,  IEnumerable<Rule> rules)
        {
            if (stockKeepingUnits == null)
            {
                throw new ArgumentNullException(nameof(stockKeepingUnits));
            }

            if (rules == null)
            {
                throw new ArgumentNullException(nameof(rules));
            }

            this.stockKeepingUnits = stockKeepingUnits;
            this.rules = rules;
        }

        /// <summary>
        /// Given a list of items and the quantities of each, calculate the final price of the items, after all rules have been applied
        /// </summary>
        /// <param name="lineItems">
        /// A list of items and the quantities of each for which the total should be calculated
        /// </param>
        /// <returns>
        /// The items total after all discounts have been applied
        /// </returns>
        /// <throws>
        /// <see cref="ArgumentOutOfRangeException"/> if any of the provided items are not a part of the instance stock keeping units
        /// </throws>
        public decimal CalculateTotal(Dictionary<string, uint> lineItems)
        {
            IEnumerable<string> unknownItems = lineItems.Keys.Except(stockKeepingUnits.Keys);
            if (unknownItems.Any())
            {
                string message = unknownItems.Count() == 1 ?
                    $"Line item {unknownItems.First()} is not a valid stock keeping unit" :
                    $"Line items {string.Join(", ", unknownItems)} are not valid stock keeping units";
                throw new ArgumentOutOfRangeException(nameof(lineItems), message);                    
            }

            Dictionary<string, uint> remainingItems = new Dictionary<string, uint>(lineItems);

            decimal total = 0;

            foreach (Rule rule in rules)
            {
                List<string> itemKeys = lineItems.Keys.ToList();
                if (itemKeys.Intersect(rule.AffectedSkus).SequenceEqual(rule.AffectedSkus))
                {
                    total = total + rule.CalculateTotal(ref remainingItems);
                }
            }

            foreach (KeyValuePair<string, uint> remaining in remainingItems)
            {
                total = total + (stockKeepingUnits[remaining.Key] * remaining.Value);
            }

            return total;
        }
    }
}
