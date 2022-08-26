using System.Text.RegularExpressions;

namespace SalesMine.Core.DomainObjects
{
    public class Email
    {
        public const int AddressMaxLength = 254;
        public const int AddressMinLength = 5;

        public string Address { get; set; }

        protected Email()
        {

        }

        public Email(string address)
        {
            if (!Validate(address)) throw new DomainException("Invalid email");
            Address = address;
        }

        public static bool Validate(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }
    }
}
