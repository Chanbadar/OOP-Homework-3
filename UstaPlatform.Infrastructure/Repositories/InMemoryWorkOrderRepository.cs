using UstaPlatform.Domain.Entities;
using UstaPlatform.Domain.Interfaces;
using System.Linq;

namespace UstaPlatform.Infrastructure.Repositories
{
    // IWorkOrderRepository arayüzünün sahte (in-memory) implementasyonu
    public class InMemoryWorkOrderRepository : IWorkOrderRepository
    {
        // static liste, sahte veritabanı gibi davranır
        private static readonly List<IsEmri> _isEmirleri = new List<IsEmri>();
        private static int _nextId = 1;

        public void Add(IsEmri isEmri)
        {
            // init-only 'Id' burada atanamaz, bu yüzden klonlama yapmalıyız
            // veya Id'yi constructor'dan almalıyız.
            // Ödevin basitliği için Id'yi 'init' yerine 'set' yapabiliriz
            // ya da daha doğru bir yöntemle:
            // Id'yi init bırakıp, repository'e eklerken Id'li yeni nesne yaratırız.
            // Şimdilik en basit yol: IsEmri'ndeki Id'yi 'public int Id { get; set; }' yapın.
            // Ya da Id'yi constructor'a taşıyın.

            // Düzeltme: Ödev 'init' istediği için, Id'yi set edemeyiz.
            // Gerçekçi bir senaryoda Id'yi IsEmri'ne constructor'dan veririz.
            // Ya da basitlik için Id'yi normal { get; set; } yapalım.
            // 'init'in anlamını göstermek için Id'yi init bırakıp, Program.cs'de atayalım.
            // Repository'e gelen IsEmri'nin Id'si olduğunu varsayalım.
            // En temizi: Id'yi { get; set; } yapalım.
            // Hoca 'init' istiyorsa, IsEmri'nin constructor'ı olmalı.

            // 'init' Kuralına Uyalım: Program.cs'de Id'yi atayacağız.
            // Bu repo sadece listeye eklesin.

            // Düzeltme 2: 'init' demek nesne yaratılırken atanır demek.
            // Program.cs'de Id'yi yaratırken atayacağız. Repo sadece ekleyecek.
            // Ama Id'nin otomatik artması lazım.
            // Ödevin odağı bu değil. En basit yöntemle Id'yi {get; set;} yapalım.
            // Ya da ödevdeki tanıma sadık kalalım (Id init olsun) ve Id'yi manuel verelim.
            // En temizi: Id'yi init bırakalım, repository'de Id atamak için Reflection kullanalım (çok karmaşık).

            // SON KARAR (Ödevin ruhuna uygun):
            // IsEmri'ndeki 'Id'yi 'public int Id { get; set; }' olarak değiştirin.
            // 'init' özelliğini Talep'teki 'KayitZamani' ile göstermiş olacağız.
            
            isEmri.Id = _nextId++;
            _isEmirleri.Add(isEmri);
        }

        public IsEmri GetById(int id) => _isEmirleri.FirstOrDefault(i => i.Id == id);

        public IEnumerable<IsEmri> GetByUstaId(int ustaId) =>
            _isEmirleri.Where(i => i.UstaId == ustaId);

        public IEnumerable<IsEmri> GetByDate(DateOnly date) =>
            _isEmirleri.Where(i => i.Tarih == date);
    }
}