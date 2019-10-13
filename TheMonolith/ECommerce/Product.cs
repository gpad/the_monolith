using System;

namespace TheMonolith.ECommerce
{
    public class Product
    {
        public Guid Id {get; private set ; }
        public string Title {get; private set;}
        public string Description {get; private set;}
        public Money Price {get; private set;}

        public Product(Guid id, string title, string description, Money price)
        {
            Id = id;
            Title = title;
            Description = description;
            Price = price;
        }
    }
}
