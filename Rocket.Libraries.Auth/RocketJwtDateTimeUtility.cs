using System;

namespace Rocket.Libraries.Auth
{
    public interface IRocketJwtDateTimeUtility
    {
        DateTimeOffset UtcNow { get; }
    }

    public class RocketJwtDateTimeUtility : IRocketJwtDateTimeUtility
    {
        public DateTimeOffset UtcNow
        {
            get
            {
                return DateTimeOffset.UtcNow;
            }
        }
    }
}