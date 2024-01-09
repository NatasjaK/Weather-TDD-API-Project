namespace Weather_TDD_API
{
    public class Program
    {
        private static int _apiCallCount = 0;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                _apiCallCount++; // Increment the counter for each API call
                Console.WriteLine($"API Call {_apiCallCount} made");
                await next.Invoke();
            });

            app.MapGet("/statistics", async (HttpContext context) =>
            {
                await context.Response.WriteAsync($"Total API calls since start: {_apiCallCount}");
            });

            app.MapGet("/healthcheck", () =>
            {
                return "OK";
            });

            string API_key = "fb32338da628928837d8f4bf95cdc4c6";

            app.MapGet("/weatherdata", () =>
            {
                var cityName = "Stockholm"; // Default city name
                var weatherData = GetWeatherData(cityName);
                return Results.Json(weatherData);
            });

            app.MapGet("/weatherdata/{cityName}", (string cityName) =>
            {
                var weatherData = GetWeatherData(cityName);
                return Results.Json(weatherData);
            });

            WeatherData GetWeatherData(string cityName)
            {
                var baseURL = "https://api.openweathermap.org/data/2.5/weather";
                var queryParams = $"?q={cityName}&appid={API_key}&units=metric"; // Requesting metric units for temperature

                using var client = new HttpClient();
                var response = client.GetAsync($"{baseURL}{queryParams}").Result;
                var content = response.Content.ReadAsStringAsync().Result;

                dynamic jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                // Extracting required fields
                var cityNameFromAPI = jsonData.name;
                var temperature = jsonData.main.temp;
                var humidity = jsonData.main.humidity;
                var windSpeed = jsonData.wind.speed;

                // Creating a WeatherData object with the extracted fields
                var weatherData = new WeatherData
                {
                    CityName = cityNameFromAPI,
                    Temperature = temperature,
                    Humidity = humidity,
                    WindSpeed = windSpeed
                };

                return weatherData;
            }

            app.Run();
        }
    }
}