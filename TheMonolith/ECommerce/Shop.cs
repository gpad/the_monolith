using System.Threading.Tasks;

namespace TheMonolith.ECommerce
{
    public class Shop : IShop
    {
        public Task CheckoutAsync(Customer customer)
        {
            throw new System.NotImplementedException();
        }

        public Task PayAsync(Customer customer)
        {
            throw new System.NotImplementedException();
        }

        public Task PutProductInCartAsync(Customer customer, Product product)
        {
            throw new System.NotImplementedException();
        }

        public Task ResetCartAsync(Customer customer)
        {
            throw new System.NotImplementedException();
        }
    }
}
