namespace UstaPlatform.Infrastructure.Helpers
{
    // Statik yardımcı sınıf
    public static class GeoHelper
    {
        // Basit Manhattan mesafesi (enlem/boylam yerine X,Y koordinatları için)
        public static int CalculateDistance((int X, int Y) p1, (int X, int Y) p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }
    }
}