﻿using ElPortalApp.Interfaces;
using Newtonsoft.Json;
using Shared.DTOs;
using System.Net.Http.Headers;
using System.Text;

namespace ElPortalApp.Servicios
{
    public class ClienteService : IClienteService
    {
        private readonly IHttpClientFactory _httpClient;
        private static string _usuario;
        private static string _clave;
        //private static string _baseUrl;
        private static string? _token;

        public ClienteService(IHttpClientFactory httpClient)
        {

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            _usuario = builder.GetSection("ApiSetting:usuario").Value;
            _clave = builder.GetSection("ApiSetting:clave").Value;
            //_baseUrl = builder.GetSection("ApiSetting:baseUrl").Value;
            _httpClient = httpClient;
        }

        //USAR REFERENCIAS 
        public async Task Autenticar()
            {

            //var cliente = new HttpClient();
            //cliente.BaseAddress = new Uri(_baseUrl);
            var httpClient = _httpClient.CreateClient("ElPortalAPI");
            

            var credenciales = new UserDto() { Username = _usuario, Password = _clave };

            var content = new StringContent(JsonConvert.SerializeObject(credenciales), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/api/Auth/login", content);
            var json_respuesta = await response.Content.ReadAsStringAsync();

            var resultado = JsonConvert.DeserializeObject<JwtToken>(json_respuesta);
            _token = resultado.Token;
        }

       
        public async Task<ICollection<ClienteDto>?> GetClientesAsync()
        {
            List<ClienteDto> lista = new List<ClienteDto>();

            await Autenticar();
            var httpClient = _httpClient.CreateClient("ElPortalAPI");

            //var cliente = new HttpClient();
            //cliente.BaseAddress = new Uri(_baseUrl);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await httpClient.GetAsync("/api/Cliente/ListadoClientes");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<ClienteDto>>(json_respuesta);
                lista = resultado;
            }

            return lista;
        }

        public Task<ClienteDto?> GetClienteAsync(int id)
        {
            throw new NotImplementedException();
        }


        public Task<ClienteDto> CreateClienteAsync(AgregarClienteDto cliente)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteClienteAsync(int id)
        {
            bool respuesta = false;

            await Autenticar();


            var httpClient = _httpClient.CreateClient("ElPortalAPI");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);


            var response = await httpClient.DeleteAsync($"/api/Cliente/{id}");

            if (response.IsSuccessStatusCode)
            {
                respuesta = true;
            }

            return respuesta;
        }

        public Task<bool> ExisteClienteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExisteClienteByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

       
        public Task<bool> UpdateClienteAsync(ClienteDto cliente)
        {
            throw new NotImplementedException();
        }
    }
}
