using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheMonolith.Shop
{
    public interface ICustomerBase
    {
        Task<IEnumerable<Customer>> ActiveCustomerAsync();
    }
}
