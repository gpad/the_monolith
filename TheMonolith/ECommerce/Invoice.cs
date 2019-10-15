using System;
using System.Collections.Generic;

namespace TheMonolith.ECommerce
{
    public class Invoice
    {
        public Guid Id { get; }
        public string Number { get; }
        public Customer Customer { get; }
        public string ShippingAddress { get; }
        public Money Total { get; }
        public IEnumerable<InvoiceItem> Items { get; }
        public Guid CartId { get; }

        public Invoice(
            Guid id,
            string number,
            Customer customer,
            string shippingAddress,
            Money total,
            IEnumerable<InvoiceItem> items,
            Guid cartId)
        {
            this.CartId = cartId;
            this.Items = items;
            this.Total = total;
            this.ShippingAddress = shippingAddress;
            this.Customer = customer;
            this.Number = number;
            this.Id = id;
        }
    }

    public class InvoiceItem
    {
        public Guid ProductId { get; }
        public string Name { get; }
        public Money Price { get; }
        public int Qty { get; }
        public InvoiceItem(
            Guid id,
            Guid productId,
            string name,
            Money price,
            int qty)
        {
            this.Qty = qty;
            this.Price = price;
            this.Name = name;
            this.ProductId = productId;
        }
    }
}
