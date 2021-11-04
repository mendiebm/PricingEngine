using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngine
{
    public abstract class Rule
    {        
        public IEnumerable<string> AffectedSkus
        {
            get; private set;
        }

        public Dictionary<string, decimal> StockKeepingUnits
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

            StockKeepingUnits = stockKeepingUnits;
            

            foreach (string affectedSku in affectedSkus)
            {
                if (!stockKeepingUnits.ContainsKey(affectedSku))
                {
                    throw new ArgumentException(nameof(affectedSkus), $"Unknown line item SKU {affectedSku}");
                }
            }

            AffectedSkus = affectedSkus;
        }
    }

    public class FixedPriceDiscountRule : Rule
    {
        internal decimal Price;
        internal string AffectedSku;
        internal uint RequiredQuantity;

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
    }

    public class ComboBuyDiscountRule : Rule
    {
        internal decimal Price;        
        internal uint RequiredQuantity;

        public ComboBuyDiscountRule(IEnumerable<string> affectedSkus, decimal price, Dictionary<string, decimal> stockKeepingUnits) : base(affectedSkus, stockKeepingUnits)
        {
            if (price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be less than 0");
            }

            Price = price;
        }
    }

    // Future extension public class PercenttageDiscountRule : Rule
}
