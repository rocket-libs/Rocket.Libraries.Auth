using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;

namespace Rocket.Libraries.Auth
{
    public interface IRocketJwtTokenDecoder
    {
        Task<IDictionary<string, object>> DecodeTokenAsync(string token);
    }

    public class RocketJwtTokenDecoder : IRocketJwtTokenDecoder
    {
        private readonly IRocketJwtSecretProvider secretProvider;



        public RocketJwtTokenDecoder(
            IRocketJwtSecretProvider secretProvider
        )
        {
            this.secretProvider = secretProvider;
        }

        public TokenDescription GetTokenDescription(string token)
        {
            var payload = JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .Decode<IDictionary<string, object>>(token);
            var expiryEpoch = default(long);
            if (payload.ContainsKey("exp"))
            {
                expiryEpoch = (long)payload["exp"];
                var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(expiryEpoch);
                return new TokenDescription
                {
                    Expires = dateTimeOffset,
                };
            }
            else
            {
                throw new RocketJwtException("Token does not contain expiry information", RocketJwtException.NoExpiry);
            }
        }

        public async Task<IDictionary<string, object>> DecodeTokenAsync(string token)
        {
            try
            {
                var secret = await secretProvider.GetSecretAsync();
                var payload = JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(secret)
                    .MustVerifySignature()
                    .Decode<IDictionary<string, object>>(token);
                return payload;
            }
            catch (TokenExpiredException)
            {
                throw new RocketJwtException("Token has expired. Please authenticate again", RocketJwtException.TokenExpired);
            }
            catch (SignatureVerificationException)
            {
                throw new RocketJwtException("Invalid signature", RocketJwtException.InvalidSignature);
            }
        }
    }
}