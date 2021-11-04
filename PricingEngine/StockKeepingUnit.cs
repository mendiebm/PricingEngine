using System;

namespace PricingEngine
{
    public class StockKeepingUnit
    {
        internal string Name
        {
            get; private set;
        }

        internal decimal Price
        {
            get; private set;
        }

        public StockKeepingUnit(string name, decimal price)
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
