using UstaPlatform.Domain.Enums;

namespace UstaPlatform.Domain.Entities
{
    public class Talep
    {
        // init-only özellikler
        public int Id { get; init; }
        public DateTime KayitZamani { get; init; } = DateTime.Now;

        public int VatandasId { get; set; }
        public UzmanlikAlani GerekliUzmanlik { get; set; }
        public string IsTanimi { get; set; }
        public (int X, int Y) Konum { get; set; }
        public DateOnly IstenenTarih { get; set; }
    }
}