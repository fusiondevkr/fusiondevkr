using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using Fdk.M365Register.WasmApp.Models;

namespace Fdk.M365Register.WasmApp.Helpers
{
    /// <summary>
    /// This provides interfaces to the <see cref="AuthHelper"/> class.
    /// </summary>
    public interface IAuthHelper
    {
        /// <summary>
        /// Gets the authentication details from the token.
        /// </summary>
        Task<AuthenticationDetails> GetAuthenticationDetailsAsync();

        /// <summary>
        /// Gets the logged-in user details from Azure AD.
        /// </summary>
        Task<LoggedInUserDetails> GetLoggedInUserDetailsAsync();
    }

    /// <summary>
    /// This represents the helper entity for authentication.
    /// </summary>
    public class AuthHelper : IAuthHelper
    {
        private readonly HttpClient _http;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthHelper"/> class.
        /// </summary>
        /// <param name="httpClient"></param>
        public AuthHelper(HttpClient httpClient)
        {
            this._http = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <inheritdoc />
        public async Task<AuthenticationDetails> GetAuthenticationDetailsAsync()
        {
            var json = await this._http.GetStringAsync("/.auth/me").ConfigureAwait(false);
            var details = JsonSerializer.Deserialize<AuthenticationDetails>(json);

            return details;
        }

        /// <inheritdoc />
        public async Task<LoggedInUserDetails> GetLoggedInUserDetailsAsync()
        {
            var details = default(LoggedInUserDetails);
            try
            {
                using (var response = await this._http.GetAsync("/api/users/get").ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    details = JsonSerializer.Deserialize<LoggedInUserDetails>(json);
                }
            }
            catch
            {
            }

            return details;
        }
    }
}
