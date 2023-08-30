using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BirdFetching.Models;

namespace BirdFetching.Controllers
{   
    public class BirdsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public BirdsController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
        }

        // Get all birds
        public async Task<IActionResult> AllBirds()
        {
            Console.WriteLine("Entered AllBirds action.");
            string apiUrl = $"{_apiBaseUrl}/birds";

            try
            {
                Console.WriteLine("Making HTTP GET request to fetch all birds.");
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    Console.WriteLine($"Received birds data: {jsonResponse}"); // Log raw JSON response here

                    var birdsData = JsonSerializer.Deserialize<BirdApiResponse>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    Console.WriteLine($"Birds data parsed?: {birdsData}");

                    Console.WriteLine("Successfully parsed birds data.");

                    return View(birdsData?.Data);
                }

            }

            catch (HttpRequestException ex) 
            {
                Console.WriteLine($"Request error: {ex.Message}");
                return View("Error");
            }

            return View("Error");
        }
        
        // Get random bird
        public async Task<IActionResult> GenerateBird() 
        {
            Console.WriteLine("Entered RandomBird action.");
            string apiUrl = $"{_apiBaseUrl}/birds/generate";

           try 
           {
                Console.WriteLine("Making HTTP GET request to fetch random bird.");
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    Console.WriteLine($"Received bird data: {jsonResponse}"); // Log raw JSON response here

                    var birdData = JsonSerializer.Deserialize<SingleBirdResponse>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    Console.WriteLine($"Birds data parsed?: {birdData}");

                    Console.WriteLine("Successfully parsed random bird data.");

                    return View(birdData?.Data);
                }
           }

           catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                return View("Error");
            }

            return View("Error");
        }
    }
}