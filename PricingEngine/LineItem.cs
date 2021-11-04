using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngine
{
    public class LineItem
    {
        internal string Name;
        internal uint Quantity;

        public LineItem(string name, uint quantity, Dictionary<string, decimal> stockKeepingUnits)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name cannot be null or empty", nameof(name));
            }

            if (!stockKeepingUnits.ContainsKey(name))
            {
                throw new ArgumentException($"Unknown line item SKU {name}");
            }

            Name = name;
            Quantity = quantity;
        }
    }
}
