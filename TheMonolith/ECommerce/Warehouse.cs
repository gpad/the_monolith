using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheMonolith.ECommerce
{
    public class Warehouse : IWarehouse
    {
        public Task DismissAsync(Product p)
        {
            throw new System.NotImplementedException();
        }

        public Task FillNewProductAsync(Product product, int v)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task RefillAsync(Product p, int v)
        {
            throw new System.NotImplementedException();
        }
    }
}
