using System.Net;
using System.Text.RegularExpressions;
using Weather_TDD_API;

namespace ApiWeatherTests

{
    public class WeatherServiceTests : IDisposable
    {

        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri("https://localhost:7186/")
        };
        public void Dispose()
        {
            _httpClient.Dispose();
        }

        
        [Fact]
        public async Task HealthCheckShouldReturnSuccess()
        {
            try
            {
                // Act
                var response = await _httpClient.GetAsync("/healthcheck");

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"InnerException: {ex.InnerException.Message}");
                }
                throw;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"IOException: {ex.Message}");
                throw;
            }
        }


        [Fact]
        public async Task Healthcheck_ExpectedOk()
        {
            string expectedStatusCode = "OK";

            var response = await _httpClient.GetAsync("/healthcheck");
            string actual = await response.Content.ReadAsStringAsync();

            Assert.Equal(expectedStatusCode, actual);
        }

        [Fact]
        public async Task GetWeatherDataForStockholm_ShouldReturnCorrectCityName()
        {
            // Arrange
            var expectedCityName = "Stockholm";

            // Act
            var response = await _httpClient.GetAsync($"/weatherdata");
            var content = await response.Content.ReadAsStringAsync();
            var weatherData = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherData>(content);

            // Assert
            Assert.Equal(expectedCityName, weatherData.CityName);
        }

        [Theory]
        [InlineData("Stockholm")]
        [InlineData("Oslo")]
        [InlineData("Berlin")]
        public async Task GetWeatherData_ShouldReturnValidHumidity(string cityName)
        {
            // Act
            var response = await _httpClient.GetAsync($"/weatherdata/{cityName}");
            var content = await response.Content.ReadAsStringAsync();
            var weatherData = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherData>(content);

            // Assert
            Assert.InRange(weatherData.Humidity, 0, 100); // Assuming humidity ranges from 0 to 100
        }

        [Theory]
        [InlineData("Stockholm")]
        [InlineData("Oslo")]
        [InlineData("Berlin")]
        public async Task GetWeatherData_ShouldReturnValidTemperature(string cityName)
        {
            // Act
            var response = await _httpClient.GetAsync($"/weatherdata/{cityName}");
            var content = await response.Content.ReadAsStringAsync();
            var weatherData = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherData>(content);

            // Assert
            Assert.InRange(weatherData.Temperature, -100, 100); // Adjust temperature range as needed
        }
        [Theory]
        [InlineData("Stockholm")]
        [InlineData("Oslo")]
        [InlineData("Berlin")]
        public async Task GetWeatherData_ShouldReturnValidWindSpeed(string cityName)
        {
            // Act
            var response = await _httpClient.GetAsync($"/weatherdata/{cityName}");
            var content = await response.Content.ReadAsStringAsync();
            var weatherData = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherData>(content);

            // Assert
            Assert.InRange(weatherData.WindSpeed, 0, double.MaxValue); // Assuming wind speed is non-negative
        }

        [Fact]
        public async Task GetWeatherData_ShouldReturnValidApiCallCount()
        {
            // Arrange
            int expectedApiCallCount = 4;

            // Reset API call count to 0 using the class name
            Weather_TDD_API.Program.ResetApiCallCount();
            Console.WriteLine("API Call count reset to 0." + Weather_TDD_API.Program._apiCallCount);

            // Act: Make several API calls
            await _httpClient.GetAsync("/weatherdata/Stockholm");
            await _httpClient.GetAsync("/weatherdata/Oslo");
            await _httpClient.GetAsync("/weatherdata/Berlin");

            // Now, make a call to retrieve statistics
            var statisticsResponse = await _httpClient.GetAsync("/statistics");
            statisticsResponse.EnsureSuccessStatusCode();

            var statisticsContent = await statisticsResponse.Content.ReadAsStringAsync();
            var apiCallCount = ExtractApiCallCount(statisticsContent);

            // Assert: Check if the count matches the expected number of calls
            Assert.Equal(expectedApiCallCount, apiCallCount);
        }



        private int ExtractApiCallCount(string statisticsContent)
        {
            const string pattern = @"Total API calls since start:\s*(\d+)";
            var match = Regex.Match(statisticsContent, pattern);

            if (match.Success)
            {
                if (int.TryParse(match.Groups[1].Value, out int extractedCount))
                {
                    return extractedCount;
                }
                else
                {
                    throw new InvalidOperationException("Failed to parse API call count.");
                }
            }
            else
            {
                Console.WriteLine("Prefix not found in the statistics content.");
                return 0;
            }
        }
    }
}