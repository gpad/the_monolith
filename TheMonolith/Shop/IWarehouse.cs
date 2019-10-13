using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheMonolith.Shop
{
    public interface IWarehouse
    {
        Task DismissAsync(Product p);
        Task FillNewProductAsync(Product product, int v);
        Task<IEnumerable<Product>> GetActiveProductsAsync();
        Task RefillAsync(Product p, int v);
    }
}
