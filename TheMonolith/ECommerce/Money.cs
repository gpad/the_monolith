namespace TheMonolith.ECommerce
{
    public class Money
    {
        private readonly decimal Value;
        private readonly Currency currency;

        public Money(decimal value, Currency currency)
        {
            this.Value = value;
            this.currency = currency;
        }
    }
}
