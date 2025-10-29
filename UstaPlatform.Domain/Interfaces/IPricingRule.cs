using UstaPlatform.Domain.Entities;

namespace UstaPlatform.Domain.Interfaces
{
    // PROJENİN KALBİ (OCP - Açık/Kapalı Prensibi)
    // Tüm fiyat kuralları (dahili ve harici DLL'ler) bu arayüzü uygulamak zorundadır.
    public interface IPricingRule
    {
        // Kuralın adını loglama için tutar
        string RuleName { get; }

        // Fiyatı hesaplar.
        // 'currentPrice' o ana kadarki hesaplanmış fiyattır.
        // Kural, bu fiyatı değiştirir ve yeni fiyatı döner.
        decimal CalculatePrice(decimal currentPrice, IsEmri isEmri);
    }
}