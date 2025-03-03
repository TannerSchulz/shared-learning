using FLP.Shared.Models;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace FLP.Client.Services.UserAccount
{
    public class ClientUserAccountService(HttpClient _httpClient) : IUserAccountService
    {
        public async Task<Status> Register(RegisterModel model)
        {
            var response = await _httpClient.PostAsJsonAsync<RegisterModel>($"api/UserAccount/Register", model);
            return await response.Content.ReadFromJsonAsync<Status>();
        }
    }
}

public class RegisterModel
{
    public string email { get; set; }
    public string password { get; set; }
    public string confirmPassword { get; set; }
}
