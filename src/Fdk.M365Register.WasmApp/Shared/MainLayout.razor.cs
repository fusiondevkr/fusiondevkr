using System.Threading.Tasks;

using Fdk.M365Register.WasmApp.Helpers;
using Fdk.M365Register.WasmApp.Models;

using Microsoft.AspNetCore.Components;

namespace Fdk.M365Register.WasmApp.Shared
{
    /// <summary>
    /// This represents the main layout component.
    /// </summary>
    public class MainLayoutBase : LayoutComponentBase
    {
        /// <summary>
        /// Gets or sets the <see cref="IAuthHelper"/> instance.
        /// </summary>
        [Inject]
        public IAuthHelper Helper { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether the user is authenticated or not.
        /// </summary>
        protected virtual bool IsAuthenticated { get; private set; }

        /// <summary>
        /// Gets or sets the value indicating whether the login DOM is hidden or not.
        /// </summary>
        protected virtual bool IsLoginHidden { get; private set; }

        /// <summary>
        /// Gets or sets the value indicating whether the logout DOM is hidden or not.
        /// </summary>
        protected virtual bool IsLogoutHidden { get; private set; }

        /// <summary>
        /// Gets the <see cref="LoggedInUserDetails"/> instance.
        /// </summary>
        protected virtual LoggedInUserDetails LoggedInUser { get; private set; }

        /// <summary>
        /// Gets the logged-in user's display name.
        /// </summary>
        protected virtual string DisplayName { get; private set; }

        /// <inheritdoc />
        protected override async Task OnInitializedAsync()
        {
            await this.GetLogInDetailsAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Invokes right after the user logged-in.
        /// </summary>
        protected async Task OnLoggedInAsync()
        {
            await this.GetLogInDetailsAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Invokes right after the user logged-out.
        /// </summary>
        protected async Task OnLoggedOutAsync()
        {
            await this.GetLogInDetailsAsync().ConfigureAwait(false);
        }

        private async Task GetLogInDetailsAsync()
        {
            var authDetails = await this.Helper.GetAuthenticationDetailsAsync().ConfigureAwait(false);

            this.IsAuthenticated = authDetails.ClientPrincipal != null;
            this.IsLoginHidden = this.IsAuthenticated;
            this.IsLogoutHidden = !this.IsAuthenticated;

            this.LoggedInUser = await this.Helper.GetLoggedInUserDetailsAsync().ConfigureAwait(false);
            this.DisplayName = this.LoggedInUser?.DisplayName ?? "등록된 사용자 아님";
        }
    }
}
