Bu proje, "Nesne Yönelimli Programlama ve İleri C#" dersi kapsamında, Arcadia şehrindeki uzmanlar (usta) ile vatandaş taleplerini eşleştiren bir platform simülasyonudur.

Projenin temel amacı, SOLID prensiplerine (özellikle Açık/Kapalı Prensibi) uygun, genişletilebilir (plug-in tabanlı) bir mimari kurmaktır.

## Mimari ve Tasarım Kararları

Proje, sorumlulukların net bir şekilde ayrılması için Çok Katmanlı (Layered) bir mimari ve .NET proje yapısı kullanır:

1.  **`UstaPlatform.Domain`**:
    * **Sorumluluk**: Projenin kalbidir. İşin temel varlıklarını (Entities: `Usta`, `IsEmri` vb.) ve temel kontratları (Interfaces: `IPricingRule`, `IWorkOrderRepository`) içerir.
    * **Tasarım Kararı**: Bu katman, hiçbir katmana bağımlı değildir (Dependency Inversion Principle - DIP). Diğer tüm katmanlar bu katmana bağımlıdır.

2.  **`UstaPlatform.Infrastructure`**:
    * **Sorumluluk**: Dış dünya ile ilgili endişeleri (veritabanı, dosya sistemi, yardımcı araçlar) barındırır.
    * **Tasarım Kararı**: `Domain` katmanındaki `IWorkOrderRepository` gibi arayüzleri uygular (`InMemoryWorkOrderRepository`). Ayrıca `Guard`, `GeoHelper` gibi statik yardımcı sınıfları içerir (Single Responsibility Principle - SRP).

3.  **`UstaPlatform.Pricing`**:
    * **Sorumluluk**: Fiyatlandırma ile ilgili tüm iş mantığını içerir.
    * **Tasarım Kararı**: Bu katman, `PricingEngine` ve varsayılan (`HaftasonuEkUcretiKurali`) kuralları içerir. En kritik sorumluluğu, plug-in mimarisini yönetmektir.

4.  **`UstaPlatform.App`**:
    * **Sorumluluk**: Uygulamanın giriş noktası (Entry Point). Bağımlılıkları birbirine bağlar (DI) ve demo senaryosunu çalıştırır.

## Kritik C# Özelliklerinin Kullanımı

* **`init-only` Özelliği**: `Talep` sınıfındaki `KayitZamani` ve varlıklardaki `Id` (bazı senaryolarda) gibi alanlar, nesne oluşturulduktan sonra değiştirilemesin diye `init` olarak ayarlanmıştır.
* **Dizinleyici (Indexer)**: `Cizelge` sınıfı, `cizelge[DateOnly gun]` sözdizimini destekler. Bu, o güne ait iş emirlerine koleksiyon gibi erişmeyi sağlar.
* **Özel `IEnumerable<T>` Koleksiyonu**: `Rota` sınıfı, `IEnumerable<(int X, int Y)>` arayüzünü uygular ve `public void Add(int X, int Y)` metodu sayesinde *koleksiyon başlatıcı* (collection initializer) sözdizimini destekler (`new Rota { {1,2}, {3,4} }`).
* **Statik Sınıflar**: `Guard` ve `GeoHelper` gibi durum (state) tutmayan genel yardımcı fonksiyonlar `static` sınıf olarak tasarlanmıştır.

## Plug-in (Eklenti) Mimarisi ve OCP

Projenin en kritik gereksinimi olan Açık/Kapalı Prensibi (OCP), dinamik fiyatlandırma ile çözülmüştür.

**Nasıl Çalışır?**

1.  **Kontrat (`IPricingRule`)**: `UstaPlatform.Domain` katmanında, tüm fiyat kurallarının uyması gereken `IPricingRule` adında bir arayüz tanımlanmıştır.
2.  **Motor (`PricingEngine`)**: `UstaPlatform.Pricing` katmanındaki `PricingEngine`, başlatıldığı zaman (constructor'da) **Reflection** kullanarak `Plugins` adlı bir klasörü tarar.
3.  **Keşif (Discovery)**: Motor, bu klasördeki tüm `.dll` dosyalarını yükler ve içlerinde `IPricingRule` arayüzünü uygulayan sınıfları arar.
4.  **Yükleme (Loading)**: Bulduğu tüm sınıflardan `Activator.CreateInstance` ile yeni nesneler yaratır ve bunları kendi içindeki kural listesine (`_rules`) ekler.
5.  **Çalıştırma (Execution)**: `CalculatePrice` metodu çağrıldığında, motor hem *dahili* (Haftasonu kuralı) hem de *harici* (DLL'den yüklenen) tüm kuralları bir kompozisyon (composition) şeklinde sırayla çalıştırır.

Bu tasarım sayesinde, **ana uygulama (`UstaPlatform.App`) yeniden derlenmeden**, sadece `Plugins` klasörüne yeni bir `.dll` dosyası bırakarak (Örn: `BayramIndirimi.dll`) sisteme yeni fiyatlandırma kuralları eklenebilir. Bu, OCP'nin tam bir uygulamasıdır.

## Kurulum ve Çalıştırma

1.  Proje klonlanır.
2.  Visual Studio 2022 veya üzeri bir sürümde `UstaPlatform.sln` çözümü açılır.
3.  Çözüm derlenir (Build Solution).
4.  `UstaPlatform.App` projesi "Başlangıç Projesi" (Set as Startup Project) olarak ayarlanır.
5.  Proje çalıştırılır (Ctrl+F5).

### Demo Akışı (Plug-in Testi)

1.  `UstaPlatform.App` projesini çalıştırın. Çıktıda harici kuralın yüklenmediğini ve Senaryo 2 fiyatının (örneğin) **450 TL** olduğunu gözlemleyin.
2.  `LoyaltyDiscountRule\bin\Debug\netX.X\` klasöründeki `LoyaltyDiscountRule.dll` dosyasını kopyalayın.
3.  `UstaPlatform.App\bin\Debug\netX.X\Plugins\` klasörüne (yoksa oluşturun) yapıştırın.
4.  `UstaPlatform.App` projesini **yeniden derlemeden** tekrar çalıştırın.
5.  Çıktıda `Sadakat İndirimi (%10)` kuralının yüklendiğini ve Senaryo 2 fiyatının (örneğin) **405 TL**'ye düştüğünü gözlemleyin.
