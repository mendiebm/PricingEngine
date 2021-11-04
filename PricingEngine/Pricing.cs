using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngine
{
    internal class Pricing
    {
        internal IEnumerable<StockKeepingUnit> StockKeepingUnits
        {
            get; private set;
        }

        internal IEnumerable<Rule> Rules
        {
            get; private set;
        }

        internal Pricing(IEnumerable<StockKeepingUnit> stockKeepingUnits,  IEnumerable<Rule> rules)
        {
            if (stockKeepingUnits == null)
            {
                throw new ArgumentNullException(nameof(stockKeepingUnits));
            }

            if (rules == null)
            {
                throw new ArgumentNullException(nameof(rules));
            }

            StockKeepingUnits = stockKeepingUnits;
            Rules = rules;
        }
    }
}
