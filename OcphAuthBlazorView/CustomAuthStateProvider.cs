using Microsoft.AspNetCore.Components.Authorization;
using OcphAuthBlazorView.Services;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;

namespace OcphAuthBlazorView
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private LocalStorageAccessor _protectedLocalStorage;
        public CustomAuthStateProvider(LocalStorageAccessor protectedLocalStorage)
        {
            _protectedLocalStorage = protectedLocalStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            try
            {

                var token = await _protectedLocalStorage.GetValueAsync<string>("token");
                if (string.IsNullOrEmpty(token))
                    throw new SystemException();
                
                var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                var user = new ClaimsPrincipal(identity);
                var state = new AuthenticationState(user);
                NotifyAuthenticationStateChanged(Task.FromResult(state));
                return state;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new AuthenticationState(new ClaimsPrincipal());
            }

        }


        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithOutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            var xxx=  keyValuePairs.Select(x => new Claim(x.Key, x.Value.ToString()));
            return xxx;
        }

        private static byte[] ParseBase64WithOutPadding(string payload)
        {
            switch (payload.Length % 4)
            {
                case 2: payload += "==";
                    break;

                case 3:
                    payload += "=";
                    break;
            }

            return Convert.FromBase64String(payload);
        }
    }
}
