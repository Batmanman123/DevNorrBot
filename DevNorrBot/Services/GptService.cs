using DevNorrBot.Models;
using DevNorrBot.Payload;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DevNorrBot.Services
{
    public class Gpt3Service
    {
        private readonly string ApiKey = string.Empty;
        private readonly HttpClient _httpClient;
        public Gpt3Service(string apiKey)
        {
            ApiKey = apiKey;
            _httpClient = new HttpClient();
        }

        public async Task<GptResult> GenerateChatResponse(string message, List<Payload.Message> chatHistory)
        {
            var apiKey = ApiKey;
            var apiUrl = "https://api.openai.com/v1/chat/completions";

            chatHistory.Add(new Payload.Message
            {
                Role = "user",
                Content = message,
            });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var payload = new ChatCompletionRequest
            {
                Model = "gpt-3.5-turbo",
                Messages = chatHistory,
                Temperature = 0.7
            };

            var jsonString = JsonConvert.SerializeObject(payload, Formatting.None, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);

            GptResult result = null;
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<GptResult>(responseJson);
            }
            Program.Messages.Add(new Payload.Message
            {
                Role = "assistant",
                Content = result.choices.First().message.content
            });

            return result;
        }

        public async Task<List<string>> GenerateImageResponse(int numberOfPics, string message)
        {

            var apiUrl = "https://api.openai.com/v1/images/generations";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

            var payload = new ImagePrompt
            {
                prompt = message,
                n = numberOfPics,
                size = "1024x1024"
            };
            var jsonString = JsonConvert.SerializeObject(payload, Formatting.None, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });

            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            //response is a list of image objects
            var response = await _httpClient.PostAsync(apiUrl, content);

            ImageResponse imageResponse = JsonConvert.DeserializeObject<ImageResponse>(response.Content.ReadAsStringAsync().Result);

            var result = imageResponse.data.Select(x => x.url).ToList();
            //var result = imageResponse.data.First().url;


            return result;


        }
    }
}
