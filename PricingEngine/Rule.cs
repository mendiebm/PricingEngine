using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngnie
{
    public abstract class Rule
    {
        internal IEnumerable<string> StockKeepingUnits;
    }

    public class FixedPriceDiscountRule : Rule
    {
        internal decimal Price;
    }

    // Future extension public class PercenttageDiscountRule : Rule
}
