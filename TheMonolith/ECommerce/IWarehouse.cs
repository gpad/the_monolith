using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheMonolith.ECommerce
{
    public interface IWarehouse
    {
        Task DismissAsync(Product p);
        Task AddNewProductAsync(Product product, int v);
        Task<IEnumerable<Product>> GetActiveProductsAsync();
        Task RefillAsync(Product p, int v);
    }
}
