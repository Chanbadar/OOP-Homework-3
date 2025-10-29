using UstaPlatform.Domain.Entities;

namespace UstaPlatform.Domain.Interfaces
{
    // DIP (Bağımlılıkların Tersine Çevrilmesi)
    // Üst katmanlar (Çizelge gibi) somut veritabanına değil, bu arayüze bağımlı olur.
    public interface IWorkOrderRepository
    {
        void Add(IsEmri isEmri);
        IsEmri GetById(int id);
        IEnumerable<IsEmri> GetByUstaId(int ustaId);
        IEnumerable<IsEmri> GetByDate(DateOnly date);
    }
}