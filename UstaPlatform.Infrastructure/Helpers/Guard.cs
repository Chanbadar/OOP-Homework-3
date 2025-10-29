namespace UstaPlatform.Infrastructure.Helpers
{
    // Statik yardımcı sınıf
    public static class Guard
    {
        public static void AgainstNull(object value, string paramName)
        {
            if (value == null)
                throw new ArgumentNullException(paramName);
        }

        public static void AgainstNegative(decimal value, string paramName)
        {
            if (value < 0)
                throw new ArgumentException("Değer negatif olamaz.", paramName);
        }
    }
}