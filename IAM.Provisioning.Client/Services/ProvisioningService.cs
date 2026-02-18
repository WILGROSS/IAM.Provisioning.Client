using IAM.Provisioning.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IAM.Provisioning.Client.Services
{
    public class ProvisioningService
    {
        private readonly HttpClient _client;
        private readonly string _apiKey
        private readonly string _baseUrl;

        public ProvisioningService(string baseUrl, string apiKey)
        {
            _client = new HttpClient();
            _baseUrl = baseUrl;
            _apiKey = apiKey;
        }

        // --- G-NIVÅ METOD ---
        public async Task ProcessBasicAsync(string filePath)
        {
            Console.WriteLine("Mappar användare för G-provisionering");
            var users = LoadUsers(filePath);

            // (G): Mappa ALLA fält enligt specifikation. Saknas något va?
            var payload = users.Select(u => new {
                name = u.GetFormattedFullName(),
                email = u.GetFormattedEmail()
            }).ToList();

            await SendBatchRequestAsync(payload, "basic");
        }}

        // --- VG-NIVÅ METOD ---
        public async Task ProcessAdvancedAsync(string filePath)
        {
            Console.WriteLine("Mappar användare för VG-provisionering");
            var users = LoadUsers(filePath);

            // (VG): Här ska det vara samma mappning som för G, men det saknas något mer...
            // Det behövs ju något för att endast mappa aktiva användare
            var payload = users.Select(u => new {
                    name = u.GetFormattedFullName(),
                    email = u.GetFormattedEmail()
                }).ToList();

            await SendBatchRequestAsync(payload, "kalleAnka");
        }

        private async Task SendBatchRequestAsync(object payload, string endpoint)
        {
            Console.WriteLine($"Försöker skicka mappade användare till API-endpoint {endpoint}...");
            try
            {
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

                if (endpoint == "advanced")
                    _client.DefaultRequestHeaders.Add("Här behövs någon slags säkerhetsheader tror jag", "Eller var det här..?");

                var response = await _client.PostAsJsonAsync($"{_baseUrl}/{endpoint}", payload);
                string responseString = await response.Content.ReadAsStringAsync();

                Console.WriteLine("\nSvar från API:");
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var json = JsonSerializer.Deserialize<JsonElement>(responseString);
                        string msg = json.TryGetProperty("message", out var m) ? m.GetString() : responseString;

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode} - {msg}\n");
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode} - {responseString}\n");
                    }
                }
                else
                {
                    try
                    {
                        var json = JsonSerializer.Deserialize<JsonElement>(responseString);
                        string err = json.TryGetProperty("error", out var e) ? e.GetString() : responseString;

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode} - Error: {err}\n");
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode} - Error: {responseString}\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[CRITICAL] System Error: {ex.Message}\n");
            }

            Console.ResetColor();
        }

        private List<UserRecord> LoadUsers(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<UserRecord>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
