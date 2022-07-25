using Catalog.Domain.VO;

namespace Catalog.Application.Contracts.Infrastructure
{
    /// <summary>
    /// Contracts related to mailing
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Method that sends an email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<bool> SendEmail(EmailVO email);
    }
}
