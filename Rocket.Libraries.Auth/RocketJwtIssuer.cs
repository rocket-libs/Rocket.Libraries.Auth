using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JWT.Algorithms;
using JWT.Builder;

namespace Rocket.Libraries.Auth
{
    public interface IRocketJwtIssuer
    {
        Task<string> GetTokenAsync(NativeAuthenticationResult nativeAuthenticationResult);
    }

    public class RocketJwtIssuer : IRocketJwtIssuer
    {


        private readonly IRocketJwtSecretProvider secretProvider;
        private readonly IRocketJwtDateTimeUtility dateTimeUtility;

        public RocketJwtIssuer(
            IRocketJwtSecretProvider secretProvider,
            IRocketJwtDateTimeUtility dateTimeUtility
        )
        {
            this.secretProvider = secretProvider;
            this.dateTimeUtility = dateTimeUtility;
        }

        /// <summary>
        /// Pass ther result of authentication on your native system. If authentication succeeded on the native system 
        /// then a token string is returned.
        /// If authentication failed on the native system, then an exception is thrown.
        /// </summary>
        /// <param name="nativeAuthenticationResult">Information regarding the result of authentication on the native system as well as any claims to be encoded in the token</param>
        /// <returns></returns>
        public async Task<string> GetTokenAsync(NativeAuthenticationResult nativeAuthenticationResult)
        {
            if (nativeAuthenticationResult.Authenticated == false)
            {
                throw new RocketJwtException("Authentication failed.", RocketJwtException.NativeAuthenticationFailed);
            }
            else
            {
                var secret = await secretProvider.GetSecretAsync();
                var jwtBuilder = JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(secret)
                    .AddClaim("exp", dateTimeUtility.UtcNow.AddSeconds(nativeAuthenticationResult.LifetimeSeconds).ToUnixTimeSeconds());
                AddClaims(jwtBuilder, nativeAuthenticationResult.Claims);
                return jwtBuilder.Encode();
            }
        }

        private void AddClaims(JwtBuilder jwtBuilder, Dictionary<string, object> claims)
        {
            if (claims != null)
            {
                foreach (var item in claims)
                {
                    jwtBuilder.AddClaim(item.Key, item.Value);
                }
            }
        }
    }
}