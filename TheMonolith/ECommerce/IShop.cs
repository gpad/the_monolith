using System;
using System.Threading.Tasks;

namespace TheMonolith.ECommerce
{
    public interface IShop
    {
        Task PutProductInCartAsync(Customer customer, Product product);
        Task<Guid> CheckoutAsync(Customer customer);
        Task ResetCartAsync(Customer customer);
        Task PayAsync(Customer customer, Guid invoice_id);
    }
}
