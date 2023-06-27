using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace API.Utilities
{
    public class PasswordPolicyAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var password = (string)value;
            if (password is null) return false;

            var hasMinimum6Chars = new Regex(@".{6,}");
            var hasNumber = new Regex(@"[0-9]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            var hasUpperCase = new Regex(@"[A-Z]+");
            var hasLowerCase = new Regex(@"[a-z]+");

            var isValidated = hasMinimum6Chars.IsMatch(password) &&
                hasNumber.IsMatch(password) &&
                hasSymbols.IsMatch(password) &&
                hasUpperCase.IsMatch(password) &&
                hasLowerCase.IsMatch(password);

            return isValidated;
        }
    }
}
