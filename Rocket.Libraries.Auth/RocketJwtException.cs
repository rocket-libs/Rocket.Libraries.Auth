using System;

namespace Rocket.Libraries.Auth
{
    public class RocketJwtException : Exception
    {
        public const string NativeAuthenticationFailed = "NativeAuthenticationFailed";

        public const string TokenExpired = "TokenExpired";

        public const string InvalidSignature = "InvalidSignature";

        public const string NoExpiry = "NoExpiry";

        public RocketJwtException(string message, string errorKey) : base(message)
        {
            ErrorKey = errorKey;
        }


        public string ErrorKey { get; }
    }
}