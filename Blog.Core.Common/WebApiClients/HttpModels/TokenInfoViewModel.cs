using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiClient.DataAnnotations;
namespace Blog.Core.Common.WebApiClients
{
    public class TokenInfoViewModel
    {
        [AliasAs("success")]
        public bool Success { get; set; }

        [AliasAs("token")]
        public string Token { get; set; }

        [AliasAs("expires_in")]
        public double Expires_in { get; set; }

        [AliasAs("token_type")]
        public string Token_type { get; set; }

    }
}