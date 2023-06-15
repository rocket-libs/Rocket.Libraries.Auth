using System;

namespace Rocket.Libraries.Auth
{
    public class TokenDescription
    {
        public DateTimeOffset Expires { get; set; }

        public bool IsExpired => DateTime.UtcNow > Expires;


    }
}