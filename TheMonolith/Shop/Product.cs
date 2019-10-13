using System;

namespace TheMonolith.Shop
{
    public class Product
    {
        Guid Id;
        string Title;
        string Description;
        Money Price;

        public Product(Guid id, string title, string description, Money price)
        {
            Id = id;
            Title = title;
            Description = description;
            Price = price;
        }
    }
}
