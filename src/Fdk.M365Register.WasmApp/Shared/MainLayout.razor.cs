using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Fdk.M365Register.WasmApp.Models;
using Microsoft.AspNetCore.Components;

namespace Fdk.M365Register.WasmApp.Shared
{
    public class MainLayoutBase : LayoutComponentBase
    {
        [Inject]
        public NavigationManager Nav {get; set;}

        [Inject]
        public HttpClient Http { get; set; }

        protected virtual string CurrentPage { get; private set; }

        protected virtual bool IsAuthenticated { get; private set; }
        protected virtual bool IsLoginHidden { get; private set; }
        protected virtual bool IsLogoutHidden { get; private set; }

        protected virtual string LogInDetails { get; private set; }

        /// <inheritdoc />
        protected override async Task OnInitializedAsync()
        {
            this.CurrentPage = this.Nav.Uri;

            await this.GetLogInDetailsAsync().ConfigureAwait(false);
        }

        protected async Task OnLoggedInAsync()
        {
            await this.GetLogInDetailsAsync().ConfigureAwait(false);
        }

        protected async Task OnLoggedOutAsync()
        {
            await this.GetLogInDetailsAsync().ConfigureAwait(false);
        }

        private async Task GetLogInDetailsAsync()
        {
            var authDetails = await this.Http.GetStringAsync("/.auth/me").ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<AuthenticationDetails>(authDetails);

            this.LogInDetails += JsonSerializer.Serialize(result);

            this.IsAuthenticated = result.ClientPrincipal != null;
            this.IsLoginHidden = this.IsAuthenticated;
            this.IsLogoutHidden = !this.IsAuthenticated;

            this.LogInDetails += "<br/>";
            this.LogInDetails += authDetails;
        }
    }
}
