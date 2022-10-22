global using System.Net.Http.Json;
using OcphAuthBlazorView.Models;
using System.Diagnostics;
using System.Text.Json;
using OcphAuthBlazorView.Extentions;

using static System.Net.WebRequestMethods;
using System.Xml.Linq;
using System;
using System.Reflection.Metadata;
using SharedModel.Models;

namespace OcphAuthBlazorView.Services
{

    public class AccountService : IAccountService
    {

        private LocalStorageAccessor _localStorage;
        private readonly HttpClient _httpClient;
        public AccountService(HttpClient httpClient, LocalStorageAccessor localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }


        public async Task<bool> Login(string userName, string password)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync("/api/account/login", new { userName, password });
                var stringData = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    var resultData = JsonSerializer.Deserialize<LoginResponse>(stringData, Helper.JsonOption);
                    await _localStorage.SetValueAsync("token", resultData.Token);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
        }


        public async Task<bool> Logout()
        {

            try
            {
                await _localStorage.RemoveAsync("token");
                return true;
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }


        public async Task<bool> Register(RegisterRequest model)
        {

            try
            {
                var result = await _httpClient.PostAsJsonAsync("/api/account/register", _httpClient.GenerateContentToJson(model));
                var stringData = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    var resultData = JsonSerializer.Deserialize<RegisterResult>(stringData, Helper.JsonOption);
                    await _localStorage.SetValueAsync("token", resultData.Token);
                    return true;
                }
                else
                {
                    var error = JsonSerializer.Deserialize<ErrorResult>(stringData, Helper.JsonOption);
                    error.GetErrors();
                    throw new OcphAuthClientException(error.Title, error);

                }
            }
            catch (OcphAuthClientException ex)
            {
               
                throw ex;
            }
        }




    }
}
