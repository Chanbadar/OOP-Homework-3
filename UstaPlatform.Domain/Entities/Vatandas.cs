namespace UstaPlatform.Domain.Entities
{
    public class Vatandas
    {
        // init-only özellik
        public int Id { get; init; }
        public string AdSoyad { get; set; }
        public (int X, int Y) Konum { get; set; }
    }
}