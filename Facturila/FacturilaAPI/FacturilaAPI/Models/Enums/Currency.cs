namespace FacturilaAPI.Models.Enums
{
    public enum CurrencyEnum
    {
        Ron = 0, 
        Euro = 1,
    }

    public static class Currency
    {
        public static int ToInt(CurrencyEnum currency)
        {
            return (int)currency;
        }

        public static CurrencyEnum ToCurrency(int value)
        {
            if (Enum.IsDefined(typeof(CurrencyEnum), value))
            {
                return (CurrencyEnum)value;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Value is not a valid currency");
            }
        }
    }
}
