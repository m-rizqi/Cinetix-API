using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Cinetix_Api.Utility
{
    static class Utility
    {
        public static bool IsValidEmail(string email)
        {
            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }
        public static bool isValidPassword(string password)
        {
            var numberRegex = new Regex(@"[0-9]+");
            var capsRegex = new Regex(@"[A-Z]+");
            var lengthRegex = new Regex(@".{8,}");
            return numberRegex.IsMatch(password) && capsRegex.IsMatch(password) && lengthRegex.IsMatch(password);
        }
    }
}
