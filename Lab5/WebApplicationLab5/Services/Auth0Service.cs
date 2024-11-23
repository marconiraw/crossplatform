using WebApplicationLab5.Models;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace WebApplicationLab5.Services
{
    public class Auth0Service : IAuth0Service
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public Auth0Service(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<bool> RegisterUserAsync(UserModel user)
        {
            var domain = _configuration["Auth0:Domain"];
            var clientId = _configuration["Auth0:ClientId"];
            var clientSecret = _configuration["Auth0:ClientSecret"];
            var connection = "Username-Password-Authentication"; // Змініть, якщо використовуєте іншу конекцію

            var url = $"https://{domain}/dbconnections/signup";

            var payload = new
            {
                client_id = clientId,
                email = user.Email,
                username = user.Username,
                password = user.Password,
                connection = connection,
                user_metadata = new
                {
                    username = user.Username,
                    fullname = user.FullName,
                    phone = user.Phone
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<(string AccessToken, string Username, string Fullname, string Phone, string Email)> LoginUserAsync(string email, string password)
        {
            var domain = _configuration["Auth0:Domain"];
            var clientId = _configuration["Auth0:ClientId"];
            var clientSecret = _configuration["Auth0:ClientSecret"];
            var audience = _configuration["Auth0:Audience"]; // Якщо використовується

            var url = $"https://{domain}/oauth/token";

            var payload = new
            {
                grant_type = "password",
                username = email, // Використовуємо email як username
                password = password,
                audience = audience, // Опціонально
                scope = "openid profile email",
                client_id = clientId,
                client_secret = clientSecret
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Login failed: {responseContent}");
            }

            dynamic result = JsonConvert.DeserializeObject(responseContent);
            string accessToken = result.access_token;
            string idToken = result.id_token;

            // Розшифровуємо id_token
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(idToken);

            // Витягуємо дані
            string username = jwtToken.Claims.FirstOrDefault(c => c.Type == "dev-h28dtqajsj70m8ah.us.auth0.comusername")?.Value;
            string fullname = jwtToken.Claims.FirstOrDefault(c => c.Type == "dev-h28dtqajsj70m8ah.us.auth0.comfullname")?.Value;
            string phone = jwtToken.Claims.FirstOrDefault(c => c.Type == "dev-h28dtqajsj70m8ah.us.auth0.comphone")?.Value;
            string userEmail = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

            return (AccessToken: accessToken, Username: username, Fullname: fullname, Phone: phone, Email: userEmail);
        }
    }
}
