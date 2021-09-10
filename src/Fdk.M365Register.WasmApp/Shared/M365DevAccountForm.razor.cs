using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Fdk.M365Register.WasmApp.Models;

using Microsoft.AspNetCore.Components;

namespace Fdk.M365Register.WasmApp.Shared
{
    /// <summary>
    /// This represents the entity of the M365 dev account registration form.
    /// </summary>
    public partial class M365DevAccountForm : ComponentBase
    {
        /// <summary>
        /// Gets or sets the <see cref="HttpClient"/> instance.
        /// </summary>
        [Inject]
        public HttpClient Http { get; set; }

        /// <summary>
        /// Gets or sets the user's name.
        /// </summary>
        [Parameter]
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        [Parameter]
        public virtual string UserEmail { get; set; }

        /// <summary>
        /// Gets or sets the user's M365 dev account name.
        /// </summary>
        protected virtual string M365Email { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether to show the transaction message or not.
        /// </summary>
        protected virtual bool IsHidden { get; set; } = true;

        /// <summary>
        /// Get or sets the transaction result message.
        /// </summary>
        protected virtual string Result { get; set; }

        /// <summary>
        /// Resets the form fields.
        /// </summary>
        protected void Reset()
        {
            this.M365Email = string.Empty;
            this.IsHidden = true;
            this.Result = string.Empty;
        }

        /// <summary>
        /// Registers the M365 dev account to Microsoft Lists.
        /// </summary>
        protected async Task RegisterM365AccountAsync()
        {
            var payload = new M365EmailRegistrationRequest() { UserEmail = this.UserEmail, M365Email = this.M365Email };

            try
            {
                using(var result = await this.Http.PostAsJsonAsync<M365EmailRegistrationRequest>("api/m365/register", payload).ConfigureAwait(false))
                {
                    result.EnsureSuccessStatusCode();

                    var response = await result.Content.ReadFromJsonAsync<M365EmailRegistrationResponse>().ConfigureAwait(false);
                    this.Result = response.Message;
                    this.IsHidden = false;
                }
            }
            catch (HttpRequestException ex)
            {
                this.Result = ex.Message;
                this.IsHidden = false;
            }
        }
    }
}
