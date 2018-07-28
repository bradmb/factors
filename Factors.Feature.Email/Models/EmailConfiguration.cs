using Factors.Feature.Email.Interfaces;
using System;

namespace Factors.Feature.Email.Models
{
    public class EmailConfiguration
    {
        public string FromName { get; set; }

        public string FromAddress { get; set; }

        /// <summary>
        /// The message delivery service you want to use
        /// for sending out emails
        /// </summary>
        public IMailProvider MailProvider { get; set; }

        /// <summary>
        /// The amount of time before an email token
        /// will expire and become unuseable
        /// </summary>
        public TimeSpan TokenExpirationTime { get; set; }
    }
}
