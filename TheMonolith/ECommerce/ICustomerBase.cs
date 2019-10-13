using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheMonolith.ECommerce
{
    public interface ICustomerBase
    {
        Task<IEnumerable<Customer>> ActiveCustomerAsync();
        Task<Customer> CreateCustomerAsync(Customer customer);
    }
}
