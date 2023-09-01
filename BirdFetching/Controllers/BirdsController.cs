using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BirdFetching.Models;
using Microsoft.Extensions.Configuration;

namespace BirdFetching.Controllers
{
    public class BirdsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string? _apiBaseUrl;

        public BirdsController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
        }

        // Get all birds
        public async Task<IActionResult> AllBirds()
        {

            if (string.IsNullOrWhiteSpace(_apiBaseUrl))
            {
                return View("Error");
            }

            string apiUrl = $"{_apiBaseUrl}/birds";

            try
            {

                var response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    var birdsData = JsonSerializer.Deserialize<BirdApiResponse>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

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

            if (string.IsNullOrWhiteSpace(_apiBaseUrl))
            {
                return View("Error");
            }

            string apiUrl = $"{_apiBaseUrl}/birds/generate";

            try
            {

                var response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    var birdData = JsonSerializer.Deserialize<SingleBirdResponse>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


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

        // Get all birds and random bird
        public async Task<IActionResult> AllBirdsAndRandom()
        {
            if (string.IsNullOrWhiteSpace(_apiBaseUrl))
            {
                return View("Error");
            }

            string allBirdsApiUrl = $"{_apiBaseUrl}/birds";
            string randomBirdApiUrl = $"{_apiBaseUrl}/birds/generate";

            IEnumerable<BirdModel>? allBirds = null;
            BirdModel? randomBird = null;

            try 
            {
                var allBirdsResponse = await _httpClient.GetAsync(allBirdsApiUrl);
                if (allBirdsResponse.IsSuccessStatusCode) 
                {
                    var allBirdsJsonResponse = await allBirdsResponse.Content.ReadAsStringAsync();

                    var allBirdsData = JsonSerializer.Deserialize<BirdApiResponse>(allBirdsJsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    Console.WriteLine($"All Bird Response: {allBirdsJsonResponse}");

                    allBirds = allBirdsData?.Data;
                }

                var randomBirdResponse = await _httpClient.GetAsync(randomBirdApiUrl);
                if (randomBirdResponse.IsSuccessStatusCode) 
                {
                    var randomBirdJsonResponse = await randomBirdResponse.Content.ReadAsStringAsync();

                    var randomBirdData = JsonSerializer.Deserialize<SingleBirdResponse>(randomBirdJsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    Console.WriteLine($"Random Bird Response: {randomBirdJsonResponse}");

                    randomBird = randomBirdData?.Data;
                }

                if (allBirds == null || randomBird == null)
                {
                    return View("Error");
                }

                CombinedBirdsModel combineModel = new CombinedBirdsModel
                {
                    AllBirds = allBirds,
                    RandomBird = randomBird
                };

                return View("AllBirdsAndRandom", combineModel);
            }

            catch (HttpRequestException ex) 
            {
                Console.Write(ex.Message);
                return View("Error");
            }

        }
    }
}