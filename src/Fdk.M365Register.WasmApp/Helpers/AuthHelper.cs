using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using Fdk.M365Register.WasmApp.Models;

namespace Fdk.M365Register.WasmApp.Helpers
{
    public interface IAuthHelper
    {
        Task<ClientPrincipal> GetAuthenticationDetailsAsync();
    }

    public class AuthHelper : IAuthHelper
    {
        private readonly HttpClient _http;

        public AuthHelper(HttpClient httpClient)
        {
            this._http = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<ClientPrincipal> GetAuthenticationDetailsAsync()
        {
            var json = await this._http.GetStringAsync("/.auth/me").ConfigureAwait(false);
            var details = JsonSerializer.Deserialize<AuthenticationDetails>(json);

            return details?.ClientPrincipal;
        }
    }
}
