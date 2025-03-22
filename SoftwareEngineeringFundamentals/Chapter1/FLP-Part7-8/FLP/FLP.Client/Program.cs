using FLP.Client;
using FLP.Client.Services.Profile;
using FLP.Client.Services.UserAccount;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddHttpClient<IUserAccountService, ClientUserAccountService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

builder.Services.AddHttpClient<IProfileService, ClientProfileService>(httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

await builder.Build().RunAsync();
