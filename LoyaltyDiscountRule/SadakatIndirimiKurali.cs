using UstaPlatform.Domain.Entities;
using UstaPlatform.Domain.Interfaces;

namespace LoyaltyDiscountRule
{
    // BU DIŞSAL BİR DLL OLACAK.
    // ANA PROJE (UstaPlatform.App) BU PROJEYİ BİLMEZ, ONA REFERANS VERMEZ.
    // Sadece 'UstaPlatform.Domain' projesine referans verir (IPricingRule için).

    public class SadakatIndirimiKurali : IPricingRule
    {
        public string RuleName => "Sadakat İndirimi (%10)";

        public decimal CalculatePrice(decimal currentPrice, IsEmri isEmri)
        {
            // Normalde burada veritabanından vatandasın geçmiş
            // talepleri kontrol edilir. Biz basitçe simüle ediyoruz.
            // Diyelim ki 101 ID'li vatandaş (Talep 1 ve 2'nin sahibi) sadık müşteri.

            if (isEmri.TalepId == 1 || isEmri.TalepId == 2) // Simülasyon
            {
                // %10 indirim yap
                return currentPrice * 0.90m;
            }

            return currentPrice;
        }
    }
}