using ElPortalApp.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Shared.DTOs;
using System.Net.Http;
using System.Text;
using ElPortalApp.ViewModels;
using ElPortalApp.Session;

namespace ElPortalApp.Servicios
{
    public class AccountService: IAccountService
    {
        private readonly IHttpClientFactory _httpClient;        
        private static string? _token;

        public AccountService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<SessionUser> Login(LoginViewModel credencialesLogin)
        {
            var httpClient = _httpClient.CreateClient("ElPortalAPI");
            var credenciales = new UserDto() { Username = credencialesLogin.UserName, Password = credencialesLogin.Password };

            var content = new StringContent(JsonConvert.SerializeObject(credenciales), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/api/Auth/login", content);
            var json_respuesta = await response.Content.ReadAsStringAsync();

            var resultado = JsonConvert.DeserializeObject<JwtToken>(json_respuesta);
            _token = resultado.Token;

            if(_token == null)
            {
                return null;
            }
            var userLoggedIn = new SessionUser()
            {
                Token = _token,
                Password = credencialesLogin.Password,
                UserName = credencialesLogin.UserName,
            };
            return userLoggedIn;
        }
    }
}
