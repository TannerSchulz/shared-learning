using FLP.Shared.Models;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Reflection;

namespace FLP.Client.Services.Profile
{
    internal sealed class ClientProfileService(HttpClient _httpClient, IJSRuntime _jsRuntime) : IProfileService
    {
        public async Task<Status> CreateProfile(ProfileDto profileDto)
        {
            var response = await _httpClient.PostAsJsonAsync<ProfileDto>($"api/Profile/CreateProfile", profileDto);
            return await response.Content.ReadFromJsonAsync<Status>();
        }

        public async Task<ProfileDto> GetUserProfile(string email = "")
        {
            var result = new ProfileDto();
            try
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                result = await _httpClient.GetFromJsonAsync<ProfileDto>($"api/Profile/GetUserProfile");
            }
            catch(Exception ex)
            {
                var errorMessage = ex.Message;
            }
            return result;
        }

        public async Task SaveProfile(ProfileDto profileDto, string email = "")
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            await _httpClient.PostAsJsonAsync<ProfileDto>($"api/Profile/SaveProfile", profileDto);
        }
    }
}
