using UstaPlatform.Domain.Interfaces;

namespace UstaPlatform.Domain.Entities
{
    public class Cizelge
    {
        // DIP: Somut repository'e değil, arayüze bağımlı
        private readonly IWorkOrderRepository _repository;

        public Cizelge(IWorkOrderRepository repository)
        {
            _repository = repository;
        }

        // KRİTİK: Dizinleyici (Indexer) kullanımı
        // Schedule[DateOnly gün] şeklinde erişim sağlar.
        public IEnumerable<IsEmri> this[DateOnly gun]
        {
            get
            {
                // Sorumluluğu repository'e devreder (SRP)
                return _repository.GetByDate(gun);
            }
        }

        public bool IsUstaMusait(int ustaId, DateOnly gun)
        {
            // Basit bir kural: Bir ustanın günde 5'ten fazla işi olamaz.
            var isler = _repository.GetByUstaId(ustaId)
                                  .Count(i => i.Tarih == gun && !i.Tamamlandi);
            return isler < 5;
        }
    }
}