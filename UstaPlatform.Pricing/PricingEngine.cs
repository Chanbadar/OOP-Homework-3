using UstaPlatform.Domain.Entities;
using UstaPlatform.Domain.Interfaces;
using UstaPlatform.Pricing.Rules;
using System.Reflection; // Reflection (DLL yükleme) için bu şart!

namespace UstaPlatform.Pricing
{
    public class PricingEngine
    {
        private readonly List<IPricingRule> _rules = new List<IPricingRule>();

        public PricingEngine()
        {
            Console.WriteLine("[PricingEngine] Fiyat motoru başlatılıyor...");

            // 1. Varsayılan (built-in) kuralları yükle
            _rules.Add(new HaftasonuEkUcretiKurali());
            Console.WriteLine($"[PricingEngine] Dahili kural yüklendi: {nameof(HaftasonuEkUcretiKurali)}");

            // 2. DLL'den (plug-in) kuralları yükle
            LoadPluginRules();
        }

        private void LoadPluginRules()
        {
            // Uygulamanın çalıştığı dizindeki "Plugins" klasörünü ara
            // (UstaPlatform.App/bin/Debug/netX.X/Plugins)
            string pluginPath = Path.Combine(AppContext.BaseDirectory, "Plugins");

            if (!Directory.Exists(pluginPath))
            {
                Directory.CreateDirectory(pluginPath);
                Console.WriteLine($"[PricingEngine] 'Plugins' klasörü oluşturuldu: {pluginPath}");
                return;
            }

            Console.WriteLine($"[PricingEngine] Harici kurallar (plugin) taranıyor: {pluginPath}");

            // Bu klasördeki tüm .dll dosyalarını bul
            foreach (var dllFile in Directory.GetFiles(pluginPath, "*.dll"))
            {
                try
                {
                    // DLL'i hafızaya yükle
                    var assembly = Assembly.LoadFrom(dllFile);

                    // Bu DLL içindeki IPricingRule arayüzünü implement eden tipleri bul
                    var ruleTypes = assembly.GetTypes()
                        .Where(t => typeof(IPricingRule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                    foreach (var type in ruleTypes)
                    {
                        // Tipi oluştur (instance al)
                        var ruleInstance = Activator.CreateInstance(type) as IPricingRule;
                        if (ruleInstance != null)
                        {
                            _rules.Add(ruleInstance);
                            Console.WriteLine($"[PricingEngine] -> BAŞARILI: Harici kural yüklendi: {ruleInstance.RuleName} ({Path.GetFileName(dllFile)})");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[PricingEngine] -> HATA: {dllFile} yüklenemedi. {ex.Message}");
                }
            }
            Console.WriteLine("[PricingEngine] Plugin taraması tamamlandı.");
        }

        // Fiyat hesaplaması (Composition)
        public decimal CalculatePrice(IsEmri isEmri)
        {
            // SRP: Motor sadece kuralları sırayla uygular.
            decimal finalPrice = isEmri.TemelUcret;

            Console.WriteLine($"[PricingEngine] Hesaplama başlıyor. Temel Ücret: {finalPrice:C}");

            foreach (var rule in _rules)
            {
                decimal originalPrice = finalPrice;
                finalPrice = rule.CalculatePrice(finalPrice, isEmri);

                if (originalPrice != finalPrice)
                {
                    Console.WriteLine($"[PricingEngine] -> Kural uygulandı: {rule.RuleName} | Fiyat değişti: {originalPrice:C} -> {finalPrice:C}");
                }
            }

            isEmri.NihaiUcret = finalPrice;
            Console.WriteLine($"[PricingEngine] Hesaplama bitti. Nihai Ücret: {finalPrice:C}");
            return finalPrice;
        }
    }
}