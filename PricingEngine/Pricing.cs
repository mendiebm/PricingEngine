using System;
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

        public decimal CalculateTotal(Dictionary<string, uint> lineItems)
        {
            Dictionary<string, uint> remainingItems = new Dictionary<string, uint>(lineItems);

            decimal total = 0;

            foreach (Rule rule in Rules)
            {
                List<string> itemKeys = lineItems.Keys.ToList();
                if (itemKeys.Intersect(rule.AffectedSkus).SequenceEqual(rule.AffectedSkus))
                {
                    total = total + ProcessRule(ref remainingItems, rule);
                }
            }

            foreach (KeyValuePair<string, uint> remaining in remainingItems)
            {
                total = total + (StockKeepingUnits[remaining.Key] * remaining.Value);
            }

            return total;
        }

        private decimal ProcessRule(ref Dictionary<string, uint> lineItems, Rule rule)
        {
            decimal total = 0;
            FixedPriceDiscountRule fixedPriceDiscountRule = rule as FixedPriceDiscountRule;
            if (fixedPriceDiscountRule != null)
            {
                while (true)
                {
                    string affectedSku = fixedPriceDiscountRule.AffectedSkus.First();
                    if (lineItems[affectedSku] >= fixedPriceDiscountRule.RequiredQuantity)
                    {
                        lineItems[affectedSku] = lineItems[affectedSku] - fixedPriceDiscountRule.RequiredQuantity;
                        total = total + fixedPriceDiscountRule.Price;
                    }
                    else
                    {
                        break;
                    }
                }

            }

            ComboBuyDiscountRule comboBuyDiscountRule = rule as ComboBuyDiscountRule;
            if (comboBuyDiscountRule != null)
            {
                while (true)
                {
                    // at least one of every item in the combo rule must be present for the rule to take effect
                    if (lineItems
                        .Where(l => comboBuyDiscountRule.AffectedSkus.Contains(l.Key))
                        .All(l => l.Value >= 1))
                    {
                        foreach (string ruleSku in comboBuyDiscountRule.AffectedSkus)
                        {
                            lineItems[ruleSku] = lineItems[ruleSku] - 1;                            
                        }

                        total = total + comboBuyDiscountRule.Price;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return total;
        }
    }
}
