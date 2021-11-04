﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngine
{
    public class Pricing
    {
        internal Dictionary<string, decimal> StockKeepingUnits
        {
            get; private set;
        }

        internal IEnumerable<Rule> Rules
        {
            get; private set;
        }

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

            StockKeepingUnits = stockKeepingUnits;
            Rules = rules;
        }

        public decimal CalculateTotal(IEnumerable<LineItem> lineItems)
        {
            decimal total = 0;



            return total;
        }
    }
}
