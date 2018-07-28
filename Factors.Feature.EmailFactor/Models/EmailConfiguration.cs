using System;

namespace Factors.Feature.Email.Models
{
    public class EmailConfiguration
    {
        public string HostName { get; set; }

        public int Port { get; set; }

        public bool UseSSL { get; set; }

        public string FromName { get; set; }

        public string FromAddress { get; set; }

        /// <summary>
        /// The amount of time before an email token
        /// will expire and become unuseable
        /// </summary>
        public TimeSpan TokenExpirationTime { get; set; }
    }
}
