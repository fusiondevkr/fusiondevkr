using System.Text.RegularExpressions;

using Fdk.FlowHelper.FunctionApp.Models;

namespace Fdk.FlowHelper.FunctionApp.Extensions
{
    /// <summary>
    /// This represents the extensions entity for <see cref="NameReorderRequest"/>.
    /// </summary>
    public static class NameReorderRequestExtensions
    {
        /// <summary>
        /// Reorders the first name and surname depending on the language identified.
        /// </summary>
        /// <param name="request"><see cref="NameReorderRequest"/> instance.</param>
        /// <returns>Returns the reordered value.</returns>
        public static string Reorder(this NameReorderRequest request, Regex regex)
        {
            var firstName = request.FirstName;
            var lastName = request.LastName;

            var isFirstNameKorean = regex.IsMatch(firstName);
            var isLastNameKorean = regex.IsMatch(lastName);

            var result = (isFirstNameKorean && isLastNameKorean) ? $"{lastName}{firstName}" : $"{firstName} {lastName}";

            return result;
        }
    }
}
