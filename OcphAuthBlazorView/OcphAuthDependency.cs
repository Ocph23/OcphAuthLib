using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using OcphAuthBlazorView.Services;

namespace OcphAuthBlazorView
{
    public static class OcphAuthDependency
    {
        public static IServiceCollection AddOcphAuthView(this IServiceCollection service)
        {
            service.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            service.AddScoped<IAccountService, AccountService>();
            service.AddScoped<LocalStorageAccessor>();
            service.AddAuthorizationCore();
            return service;
        }

    }
}
