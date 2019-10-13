using System;

namespace TheMonolith.ECommerce
{
    public class Product
    {
        public Guid Id {get; private set ; }
        public string Name {get; private set;}
        public string Description {get; private set;}
        public Money Price {get; private set;}

        public Product(Guid id, string name, string description, Money price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }
    }
}
