using Vending.Domain.Exceptions;
using Domain.VO;
using System.Text.RegularExpressions;

namespace Vending.Domain.VO
{
    /// <summary>
    /// ValueObject that encapsulates an email message. Responsible for ensuring the integrity of your data.
    /// </summary>
    public class EmailVO : ValueObject
    {
        public string To { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }

        public EmailVO(string to, string subject, string body)
        {
            if (!Regex.IsMatch(to,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase))
            {
                throw new WrongEmailAddressException($"The email address {To} is not correct");
            }

            To = to;
            Subject = subject;
            Body = body;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // Using a yield return statement to return each element one at a time
            yield return To;
            yield return Subject;
            yield return Body;
        }
    }
}
