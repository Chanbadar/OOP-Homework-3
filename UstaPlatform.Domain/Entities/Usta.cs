using UstaPlatform.Domain.Enums;

namespace UstaPlatform.Domain.Entities
{
    public class Usta
    {
        // init-only özellik: Sadece oluşturulurken atanabilir
        public int Id { get; init; }
        public string AdSoyad { get; set; }
        public UzmanlikAlani Alan { get; set; }
        public double Puan { get; set; }
        public bool MusaitMi { get; set; } = true;
    }
}