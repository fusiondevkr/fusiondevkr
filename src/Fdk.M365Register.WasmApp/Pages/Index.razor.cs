using System.Threading.Tasks;

using Fdk.M365Register.WasmApp.Helpers;

using Microsoft.AspNetCore.Components;

namespace Fdk.M365Register.WasmApp.Pages
{
    /// <summary>
    /// This represents the entity of the index component.
    /// </summary>
    public partial class Index : ComponentBase
    {
        /// <summary>
        /// Gets or sets the <see cref="IAuthHelper"/> instance.
        /// </summary>
        [Inject]
        public IAuthHelper Helper { get; set; }

        /// <summary>
        /// Gets the value indicating whether the component should be hidden or not.
        /// </summary>
        protected virtual bool IsHidden { get; private set;}

        /// <summary>
        /// Gets the display name of the logged-in user.
        /// </summary>
        protected virtual string DisplayName { get; private set; }

        /// <summary>
        /// Gets the email of the logged-in user.
        /// </summary>
        protected virtual string UserEmail { get; private set; }

        /// <inheritdoc />
        protected override async Task OnInitializedAsync()
        {
            var loggedInUser = await this.Helper.GetLoggedInUserDetailsAsync().ConfigureAwait(false);

            this.IsHidden = loggedInUser == null;

            this.DisplayName = loggedInUser?.DisplayName;
            this.UserEmail = loggedInUser?.Email;
        }
    }
}
