using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Rocket.Libraries.Auth.Tests
{
    public class RocketJwtTokenDecoderTests
    {
        [Fact]
        public void ExpiryCanBeObtained()
        {
            const string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjI1MjM0NzQwMDAsIlVzZXJHcm91cCI6Ikh1bWFuIFJlc291cmNlcyJ9.8AoyMFSAHwr7Klcmg3crZ32UMIGE3Lj5fN5HzsdIGrE";
            var secretProvider = new Mock<IRocketJwtSecretProvider>();

            secretProvider.Setup(a => a.GetSecretAsync())
                .ReturnsAsync(string.Empty);

            var rocketJwtTokenDecoder = new RocketJwtTokenDecoder(
                secretProvider.Object
            );

            var response = rocketJwtTokenDecoder.GetTokenDescription(token);

            Assert.NotEqual(default(DateTimeOffset), response.Expires);

        }
    }
}