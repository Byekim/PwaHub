using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkLibrary
{
    public sealed class TelegramService
    {
        private readonly string _botToken = "7819282344:AAGv2w9_INNjmXav5dLBDZj1xO1bqcflvXY";
        private readonly HttpClient _httpClient = new HttpClient();
        private static readonly Lazy<TelegramService> _instance = new(() => new TelegramService());
        public static TelegramService Instance => _instance.Value;

        public TelegramService()
        {
            
        }

        public async Task SendMessageAsync(long chatId, string message)
        {
            var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";

            var payload = new
            {
                chat_id = chatId,
                text = message
            };
            var content = new StringContent(payload.CreateJson(), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to send message: {response.ReasonPhrase}");
            }
        }
    }

}
