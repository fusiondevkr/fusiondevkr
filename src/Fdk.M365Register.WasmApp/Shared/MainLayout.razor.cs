using System.Threading.Tasks;

using Fdk.M365Register.WasmApp.Helpers;

using Microsoft.AspNetCore.Components;

namespace Fdk.M365Register.WasmApp.Shared
{
    public class MainLayoutBase : LayoutComponentBase
    {
        [Inject]
        public NavigationManager Nav {get; set;}

        [Inject]
        public IAuthHelper Helper { get; set; }

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
            var principal = await this.Helper.GetAuthenticationDetailsAsync().ConfigureAwait(false);

            this.IsAuthenticated = principal != null;
            this.IsLoginHidden = this.IsAuthenticated;
            this.IsLogoutHidden = !this.IsAuthenticated;

            this.LogInDetails += principal;
        }
    }
}
