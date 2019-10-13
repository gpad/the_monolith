using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheMonolith.ECommerce
{
    public interface ICustomerBase
    {
        Task<IEnumerable<Customer>> ActiveCustomerAsync();
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<Guid> CreateShipmentAddressAsync(Customer customer, string v1, string v2, string v3);
    }
}
