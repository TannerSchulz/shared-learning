using FLP.Shared.Models;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace FLP.Client.Services.UserAccount
{
    internal sealed class ClientUserAccountService(HttpClient _httpClient, IJSRuntime _jsRuntime) : IUserAccountService
    {
        public async Task<Status> Register(RegisterModel model)
        {
            var response = await _httpClient.PostAsJsonAsync<RegisterModel>($"api/UserAccount/Register", model);
            return await response.Content.ReadFromJsonAsync<Status>();
        }

        public async Task<Status> Login(LoginModel model)
        {
            var response = await _httpClient.PostAsJsonAsync<LoginModel>($"api/UserAccount/Login", model);
            if(response.IsSuccessStatusCode)
            {
                var responseStatus = await response.Content.ReadFromJsonAsync<Status>();
                await SaveToken(responseStatus.Message);
                return new Status { Code = "200", Message = "Successful Login" };
            }
            return new Status { Code = "400", Message = "Bad Request" };
        }

        private async Task SaveToken(string token)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        }
    }
}

public class RegisterModel
{
    [Required]
    [EmailAddress]
    public string email { get; set; }
    [Required, MinLength(8)]
    public string password { get; set; }
    [Compare(nameof(RegisterModel.password))]
    public string confirmPassword { get; set; }
}

public class LoginModel
{
    [Required]
    public string email { get; set; }
    [Required]
    public string password { get; set; }
}
