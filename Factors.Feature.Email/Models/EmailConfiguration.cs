using Factors.Feature.Email.Interfaces;
using System;

namespace Factors.Feature.Email.Models
{
    public class EmailConfiguration
    {
        /// <summary>
        /// The display name you want to show two-factor
        /// emails as coming from
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// The email address you want two-factor emails to come from
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// The subject line you want to use for two-factor
        /// emails. If not populated, will use: "Your <c>FromName</c> Verification Code"
        /// </summary>
        public string SubjectLineTemplate { get; set; }

        /// <summary>
        /// The email message body template you want to use for
        /// two-factor authentication. Make sure you add "<c>{{verification-code}}</c>"
        /// into your email, which will get replaced with the two-factor verification
        /// code. If not populated, will use: "Your verification code is: {{verification-code}}"
        /// </summary>
        public string MessageBodyTemplate { get; set; }

        /// <summary>
        /// If set to <c>true</c>, the outgoing email message body will
        /// be marked as HTML content (for correct email client parsing)
        /// </summary>
        public bool MessageBodyTemplateIsHTML { get; set; }

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
