namespace TheMonolith.ECommerce
{
    public class Currency
    {
        public static Currency EUR = new Currency("EUR");
        public string Value { get;}
        public Currency(string value)
        {
            this.Value = value;
        }
    }
}
