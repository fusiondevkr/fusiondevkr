using Fdk.M365Register.ApiApp.Configurations;

using Microsoft.Identity.Client;

namespace Fdk.M365Register.ApiApp.Extensions
{
    /// <summary>
    /// This represents the extension entity for the <see cref="AppSettings"/> class.
    /// </summary>
    public static class AppSettingsExtensions
    {
        /// <summary>
        /// Converts the <see cref="MsGraphSettings"/> instance to the <see cref="ConfidentialClientApplicationOptions"/> instance.
        /// </summary>
        /// <param name="settings"><see cref="MsGraphSettings"/> instance.</param>
        /// <returns>Returns the <see cref="ConfidentialClientApplicationOptions"/> instance.</returns>
        public static ConfidentialClientApplicationOptions ToConfidentialClientApplicationOptions(this MsGraphSettings settings)
        {
            var options = new ConfidentialClientApplicationOptions()
            {
                Instance = settings.LoginUri,
                ClientId = settings.ClientId,
                ClientSecret = settings.ClientSecret,
                TenantId = settings.TenantId,
            };

            return options;
        }
    }
}
