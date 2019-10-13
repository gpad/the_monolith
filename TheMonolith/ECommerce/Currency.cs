namespace TheMonolith.ECommerce
{
    public class Currency
    {
        public string Value { get; private set; }
        public Currency(string value)
        {
            this.Value = value;
        }
    }
}
