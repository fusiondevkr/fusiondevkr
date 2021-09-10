using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Fdk.M365Register.WasmApp.Models;

using Microsoft.AspNetCore.Components;

namespace Fdk.M365Register.WasmApp.Shared
{
    public partial class M365DevAccountForm : ComponentBase
    {
        [Inject]
        public HttpClient Http { get; set; }

        [Parameter]
        public virtual string Name { get; set; }

        [Parameter]
        public virtual string UserEmail { get; set; }

        protected virtual string M365Email { get; set; }

        protected virtual bool IsHidden { get; set; } = true;

        protected virtual string Result { get; set; }

        protected void Reset()
        {
            this.M365Email = string.Empty;
            this.IsHidden = true;
            this.Result = string.Empty;
        }

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
