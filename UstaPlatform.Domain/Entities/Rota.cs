using System.Collections;

namespace UstaPlatform.Domain.Entities
{
    // IEnumerable<T> arayüzünü uygulayan özel koleksiyon
    public class Rota : IEnumerable<(int X, int Y)>
    {
        private readonly List<(int X, int Y)> _duraklar = new List<(int X, int Y)>();

        // KRİTİK: Koleksiyon başlatıcı (collection initializer) desteklemek için
        // 'Add' metodu public olmalı ve bu imzaya sahip olmalı.
        public void Add(int X, int Y)
        {
            _duraklar.Add((X, Y));
            Console.WriteLine($"[Rota] Yeni durak eklendi: ({X}, {Y})");
        }

        // IEnumerable<(int X, int Y)> implementasyonu
        public IEnumerator<(int X, int Y)> GetEnumerator()
        {
            // Ödevde basit bir rota sıralaması yeterli.
            // Burada gelişmiş optimizasyon (TSP) yapılabilir.
            // Şimdilik eklendiği sırayla dönüyoruz.
            return _duraklar.GetEnumerator();
        }

        // IEnumerable implementasyonu
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}