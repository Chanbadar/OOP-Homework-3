using UstaPlatform.Domain.Entities;
using UstaPlatform.Domain.Interfaces;

namespace UstaPlatform.Pricing.Rules
{
    // Bu, ana uygulama ile birlikte gelen VARSAYILAN bir kuraldır.
    public class HaftasonuEkUcretiKurali : IPricingRule
    {
        public string RuleName => "Hafta Sonu Ek Ücreti (%50)";

        public decimal CalculatePrice(decimal currentPrice, IsEmri isEmri)
        {
            var day = isEmri.Tarih.DayOfWeek;
            if (day == DayOfWeek.Saturday || day == DayOfWeek.Sunday)
            {
                return currentPrice * 1.5m; // %50 ekle
            }
            return currentPrice;
        }
    }
}