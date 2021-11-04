using System;
using System.Collections.Generic;
using System.Linq;

namespace PricingEngine
{
    /// <summary>
    /// The base class for rules. 
    /// All rules require the SKUs affected, and the known SKUs (for validation)
    /// Rules must implement a method returning the calculated price, and the remaining line items after discounted items are removed
    /// </summary>
    public abstract class Rule
    {        
        /// <summary>
        /// The Stock Keeping Units that this rule involves
        /// </summary>
        public IEnumerable<string> AffectedSkus
        {
            get; private set;
        }
        
        /// <summary>
        /// The constructor for a base rule
        /// </summary>
        /// <param name="affectedSkus">
        /// The list of stock keeping units affects (<seealso cref="AffectedSkus"/>
        /// </param>
        /// <param name="stockKeepingUnits">
        /// The list of known SKUs, used to validate <paramref name="affectedSkus"/>
        /// </param>
        protected Rule(IEnumerable<string> affectedSkus, Dictionary<string, decimal> stockKeepingUnits)
        {
            if (affectedSkus == null || !affectedSkus.Any())
            {
                throw new ArgumentNullException(nameof(affectedSkus), "Affected SKUs must be provided");
            }

            if (stockKeepingUnits == null || !stockKeepingUnits.Any())
            {
                throw new ArgumentNullException(nameof(stockKeepingUnits), "Stock keeping units must be provided");
            }

            foreach (string affectedSku in affectedSkus)
            {
                if (!stockKeepingUnits.ContainsKey(affectedSku))
                {
                    throw new ArgumentOutOfRangeException(nameof(affectedSkus), $"Unknown line item SKU '{affectedSku}'");
                }
            }

            AffectedSkus = affectedSkus.Distinct();
        }

        /// <summary>
        /// Given a list of items for which the total is to be calculated, calculate the value of the items affected, and remove these from the list 
        /// of items
        /// </summary>
        /// <param name="lineItems">A list of the items remaining to be totalled up. The quantity of affected items will be removed from this list</param>
        /// <returns>
        /// The value of the affected items in <paramref name="lineItems"/>
        /// </returns>
        public abstract decimal CalculateTotal(ref Dictionary<string, uint> lineItems);
    }    
}
