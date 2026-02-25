using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;


public class TelegramBotService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TelegramBotClient _botClient;
    private readonly string _botToken = "8593469578:AAEfY-LCjp5gxn911MmQFvNlnR5QcFh7Sgw"; 

    public TelegramBotService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _botClient = new TelegramBotClient(_botToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var receiverOptions = new ReceiverOptions { AllowedUpdates = { } };
        _botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, stoppingToken);
        
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", "GirisimciBatuhan_2026_OzelAnahtar");
        

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        if (update.Message is not { Text: { } messageText } message) return;
        var chatId = message.Chat.Id;

        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (messageText == "/kasa")
            {
                var kasa = context.Kasalar.FirstOrDefault();
                await botClient.SendMessage(chatId, $" Ciro: {kasa?.ToplamCiro} TL\nSon İşlem: {kasa?.SonIslemTarihi}");
            }
            else if (messageText == "/stok")
            {
                var kritikUrunler = context.Hammaddeler.Where(h => h.MevcutStok <= h.KritikSeviye).ToList();
                var rapor = kritikUrunler.Any()
                    ? "Stoklar:\n" + string.Join("\n", kritikUrunler.Select(h => $"{h.Ad}: {h.MevcutStok} {h.Birim}"))
                    : "Stoklar seviyesi";
                await botClient.SendMessage(chatId, rapor);
            }
            else if (messageText == "/eniyi")
            {
                var enIyiMusteri = context.MusteriSiparisleri
                    .GroupBy(s => s.MusteriId)
                    .OrderByDescending(g => g.Sum(s => s.Miktar))
                    .Select(g => new { Id = g.Key, Toplam = g.Sum(s => s.Miktar) })
                    .FirstOrDefault();

                var musteri = context.Musteriler.Find(enIyiMusteri?.Id);
                await botClient.SendMessage(chatId, $"Sadık Müşteri: {musteri?.Ad}\nToplam Sipariş: {enIyiMusteri?.Toplam} adet");
            }
            else if (messageText == "/sonsiparis")
            {
                var Sonsiparis = context.MusteriSiparisleri.OrderByDescending(s => s.SiparisTarihi).FirstOrDefault();
                var musteri = context.Musteriler.Find(Sonsiparis?.MusteriId);
                await botClient.SendMessage(chatId, $"Son Sipariş: {Sonsiparis?.SiparisTarihi}\nMüşteri: {musteri?.Ad}\nMiktar: {Sonsiparis?.Miktar} ");
            }
            else if(messageText == "/musteriler")
            {
                var musteriler = context.Musteriler.ToList();
                var rapor = musteriler.Any()
                    ? "Müşteriler:\n" + string.Join("\n", musteriler.Select(m => $"{m.Ad}  {m.Telefon} {m.KayitTarihi}"))
                    : "Kayıtlı müşteri bulunmamaktadır.";
                await botClient.SendMessage(chatId, rapor);
            }
            else if (!string.IsNullOrEmpty(messageText))
            {
                await botClient.SendMessage(chatId, "Bilinmeyen komut. Lütfen geçerli bir komut girin.");
            }
          
            
                
            
        }
    }

    Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ct)
    {
        Console.WriteLine("Bot Hatası: " + exception.Message);
        return Task.CompletedTask;
    }
}