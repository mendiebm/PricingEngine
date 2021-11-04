using System;

namespace PricingEngine
{
    internal class StockKeepingUnit
    {
        internal string Name
        {
            get; private set;
        }

        internal decimal Price
        {
            get; private set;
        }

        internal StockKeepingUnit(string name, decimal price)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name cannot be blank", nameof(name));
            }

            if (price < 0)
            {
                throw new ArgumentException("Price cannot be less than zero", nameof(price));
            }

            Name = name;
            Price = price;
        }
    }
}
