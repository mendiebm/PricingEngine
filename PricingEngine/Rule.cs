using System;
using System.Collections.Generic;
using System.Linq;

namespace PricingEngine
{
    public abstract class Rule
    {        
        public IEnumerable<string> AffectedSkus
        {
            get; private set;
        }
        

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

            AffectedSkus = affectedSkus;
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
    
    // Future extension public class PercenttageDiscountRule : Rule
}
