using System.Collections.Generic;

namespace Rocket.Libraries.Auth
{
    /// <summary>
    /// Information regarding the result of authentication on the native system as well as any claims to be encoded in the token.
    /// </summary>
    public class NativeAuthenticationResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether authentication succeeded on the native system.
        /// </summary>
        public bool Authenticated { get; set; }

        /// <summary>
        /// Gets any claims to be encoded in the token
        /// </summary>
        /// <typeparam name="string">Gets the unique name or key of the claim</typeparam>
        /// <typeparam name="object">Gets the value of the claim</typeparam>
        public Dictionary<string, object> Claims { get; } = new Dictionary<string, object> ();

        /// <summary>
        /// Gets or sets the value indicating how long the token is valid for.
        /// </summary>
        public int LifetimeSeconds { get; set; }
    }
}