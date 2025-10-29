namespace UstaPlatform.Domain.Entities
{
    public class IsEmri
    {
        // init-only özellik
        public int Id { get; set; }

        public int TalepId { get; set; }
        public int UstaId { get; set; }
        public DateOnly Tarih { get; set; }
        public decimal TemelUcret { get; set; }
        public decimal NihaiUcret { get; set; } // Fiyat motoru burayı dolduracak
        public (int X, int Y) Konum { get; set; }
        public bool Tamamlandi { get; set; } = false;
    }
}