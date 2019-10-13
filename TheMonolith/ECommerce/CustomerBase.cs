using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheMonolith.ECommerce
{
    public class CustomerBase : ICustomerBase
    {
        public Task<IEnumerable<Customer>> ActiveCustomerAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
