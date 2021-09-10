using System.Threading.Tasks;

using Fdk.M365Register.WasmApp.Helpers;

using Microsoft.AspNetCore.Components;

namespace Fdk.M365Register.WasmApp.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public IAuthHelper Helper { get; set; }

        /// <inheritdoc />
        protected override async Task OnInitializedAsync()
        {
            var principal = await this.Helper.GetAuthenticationDetailsAsync().ConfigureAwait(false);
        }
    }
}
