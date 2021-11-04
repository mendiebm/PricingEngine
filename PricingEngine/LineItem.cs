using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngine
{
    internal class LineItem
    {
        internal string Name;
        internal uint Quantity;

        internal LineItem(string name, uint quantity, IEnumerable<StockKeepingUnit> stockKeepingUnits)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name cannot be null or empty", nameof(name));
            }


            Dictionary<string, StockKeepingUnit> knownUnits = stockKeepingUnits.ToDictionary(s => s.Name, s => s);

            if (!knownUnits.ContainsKey(name))
            {
                throw new ArgumentException($"Unknown line item SKU {name}");
            }

            Name = name;
            Quantity = Quantity;
        }
    }
}
