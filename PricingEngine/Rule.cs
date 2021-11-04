using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngine
{
    public abstract class Rule
    {
        internal IEnumerable<string> StockKeepingUnits;
    }

    public class FixedPriceDiscountRule : Rule
    {
        public struct AffectedSku
        {
            public string Name
            {
                get; private set;
            }

            public uint Quantity
            {
                get; private set;
            }

            public AffectedSku(string name, uint quantity)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("Name cannot be null or empty", nameof(name));
                }

                Name = name;
                Quantity = quantity;
            }
        }

        internal decimal Price;

        public FixedPriceDiscountRule(IEnumerable<AffectedSku> affectedSkus, decimal price, IEnumerable<StockKeepingUnit> stockKeepingUnits)
        {
            if (affectedSkus == null)
            {
                throw new ArgumentNullException(nameof(affectedSkus));
            }

            if (price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be less than 0");
            }

            Dictionary<string, StockKeepingUnit> knownUnits = stockKeepingUnits.ToDictionary(s => s.Name, s => s);

            foreach (string skuName in affectedSkus)
            {
                if (!knownUnits.ContainsKey(skuName))
                {
                    throw new ArgumentException($"Unknown line item SKU {skuName}");
                }
            }
        }
    }

    // Future extension public class PercenttageDiscountRule : Rule
}
