﻿using ServiceStack.DataAnnotations;
using System;

namespace Factors.Models.UserAccount
{
    [Alias("Factors_Credential")]
    public class FactorsCredential
    {
        [PrimaryKey]
        [AutoIncrement]
        public long Id { get; set; }

        [Index]
        public string UserAccountId { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public DateTime ModifiedDateUtc { get; set; }

        [Index]
        public string FeatureTypeGuid { get; set; }

        public string CredentialKey { get; set; }

        public bool CredentialIsValidated { get; set; }
    }
}
