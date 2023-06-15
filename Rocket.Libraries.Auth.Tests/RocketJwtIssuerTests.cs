using System;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Rocket.Libraries.Auth.Tests
{
    public class RocketJwtIssuerTests
    {
        private const string Secret = "oh wow";
        private const int FiftyYearSeconds = 60 * 60 * 24 * 365 * 50;

        private DateTimeOffset UtcNow => new DateTimeOffset(2000, 01, 01, 0, 0, 0, 0, new TimeSpan(3, 0, 0));

        private const string TestClaimKey = "UserGroup";

        private const string TestClaimValue = "Human Resources";

        [Fact]
        public async Task TokenIsGenerated()
        {
            var secretProvider = new Mock<IRocketJwtSecretProvider>();
            var dateTimeUtility = new Mock<IRocketJwtDateTimeUtility>();
            dateTimeUtility.Setup(a => a.UtcNow)
                .Returns(UtcNow);

            var rocketJwtIssuer = new RocketJwtIssuer(
                secretProvider.Object,
                dateTimeUtility.Object
            );

            var nativeResult = new NativeAuthenticationResult
            {
                Authenticated = true,
                LifetimeSeconds = FiftyYearSeconds,
            };
            nativeResult.Claims.Add(TestClaimKey, TestClaimValue);


            secretProvider.Setup(a => a.GetSecretAsync())
                .ReturnsAsync(Secret);
            var token = await rocketJwtIssuer.GetTokenAsync(nativeResult);
            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact]
        public async Task TokenCanBeDecoded()
        {
            const string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjI1MjM0NzQwMDAsIlVzZXJHcm91cCI6Ikh1bWFuIFJlc291cmNlcyJ9.8AoyMFSAHwr7Klcmg3crZ32UMIGE3Lj5fN5HzsdIGrE";
            var secretProvider = new Mock<IRocketJwtSecretProvider>();

            secretProvider.Setup(a => a.GetSecretAsync())
                .ReturnsAsync(Secret);
            var rocketJwtTokenDecoder = new RocketJwtTokenDecoder(
                secretProvider.Object
            );

            var response = await rocketJwtTokenDecoder.DecodeTokenAsync(token);
            Assert.True(response.ContainsKey("exp"));
            Assert.Equal(TestClaimValue, response[TestClaimKey]);

        }
    }
}