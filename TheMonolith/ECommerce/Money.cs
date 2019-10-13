namespace TheMonolith.ECommerce
{
    public class Money
    {
        static public Money Zero = new Money(0, new Currency(""));
        public decimal Value { get; }
        public Currency currency { get; }

        public Money(decimal value, Currency currency)
        {
            this.Value = value;
            this.currency = currency;
        }
    }
}
