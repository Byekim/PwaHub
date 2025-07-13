using Hub.Shared;
using NetworkLibrary;
using static System.Net.Mime.MediaTypeNames;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Text;
using Microsoft.JSInterop;
using Hub.Shared.Voice;

namespace Hub.Client
{
    public class TtsApi
    {

        private readonly string naverTtsApiAddress = "yourAddress";
        private readonly string apiKeyid = "yourApiKeyId";
        private readonly string apiKeyPw = "youyrPw";

        private readonly int maxTtsBodyLength = 5000;
        private readonly HttpClient _client ;
        public TtsApi(HttpClient client)
        {
            this._client = client;
        }

        public async Task<byte[]> ProcessTts(VoiceBroadCast voice)
        {
            try
            {
                _client.DefaultRequestHeaders.Add("X-NCP-APIGW-API-KEY-ID", apiKeyid);
                _client.DefaultRequestHeaders.Add("X-NCP-APIGW-API-KEY", apiKeyPw);

                string? speaker = voice.speaker == Speaker.dara_danna.ToString() ? voice.speaker.Replace("_", "-") : voice.speaker;
                if (string.IsNullOrEmpty(speaker))
                    return null;
                var postData = new Dictionary<string, string>
        {
            { "speaker", speaker },
            { "volume", "5" },
            { "speed", string.IsNullOrEmpty(voice.voiceSpeed) ? "5" : voice.voiceSpeed },
            { "pitch", ""},
            { "emotion", ""},
            { "format", "mp3" },
            { "text", voice.body }
        };

                var content = new FormUrlEncodedContent(postData);
                HttpResponseMessage response = await _client.PostAsync(this.naverTtsApiAddress, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("TTS API 호출 실패");
                }
                return await response.Content.ReadAsByteArrayAsync();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}

