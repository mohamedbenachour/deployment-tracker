using System.Collections.Generic;
using System;

using Microsoft.Extensions.Configuration;

namespace deployment_tracker.Services.Token {
    public class TokenVerifier : ITokenVerifier {
        private ISet<String> Tokens { get; } = new HashSet<String>();

        public TokenVerifier(IConfiguration configuration) {
            PopulateTokens(configuration);
        }

        private void PopulateTokens(IConfiguration configuration) {
            var token = configuration["ExternalToken"];

            Tokens.Add(token);
        }

        public bool IsValid(string token) {
            return Tokens.Contains(token);
        }
    }
}