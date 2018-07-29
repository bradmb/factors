using Factors.Models.Interfaces;
using System;
using System.Net.Mail;

namespace Factors.Feature.Email
{
    public partial class EmailInstance : IFactorFeature
    {
        /// <summary>
        /// Verifies that the initial configuration is correct
        /// </summary>
        private void ValidateConfiguration()
        {
            if (String.IsNullOrWhiteSpace(_configuration.FromAddress))
            {
                throw new ArgumentException("EmailFactor: FromAddress must be set at initialization");
            }

            new MailAddress(_configuration.FromAddress);

            if (String.IsNullOrWhiteSpace(_configuration.FromName))
            {
                throw new ArgumentException("EmailFactor: FromName must be set at initialization");
            }

            if (_configuration.MailProvider == null)
            {
                throw new ArgumentException("EmailFactor: MailProvider must be configured at initialization");
            }

            if (_configuration.TokenExpirationTime.TotalSeconds <= 0)
            {
                throw new ArgumentException("EmailFactor: TokenExpirationTime must be a positive number greater than zero seconds");
            }
        }
    }
}
