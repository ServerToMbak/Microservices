using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;

namespace PlatformService.SyncDatServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private HttpClient _httpClient;
        private IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task SendPlatformToCommand(PlatformReadDto plat)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(plat),
                System.Text.Encoding.UTF8,
                "application/json");

            var response =  await _httpClient.PostAsync($"{_configuration["CommandService"]}", httpContent);


            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to CommandService was OK!!");
            } 
            else
            {
                Console.WriteLine("Sync POST to CommandService was NOT OK!!!");
            }   
        }
    }
}