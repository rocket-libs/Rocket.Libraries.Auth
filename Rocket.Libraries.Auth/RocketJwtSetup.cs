using Microsoft.Extensions.DependencyInjection;

namespace Rocket.Libraries.Auth
{
    public static class RocketJwtSetup
    {
        
        /// <summary>
        /// Register services required for Rocket.Libraries.Auth to work.
        /// </summary>
        /// <param name="services">The global services collection for DI service registration</param>
        /// <typeparam name="TSecretProvider">Type of the class that will provide the secret used to encode and decode tokens</typeparam>
        /// <returns>The global services collection for DI service registration</returns>
        public static IServiceCollection SetupRocketJwtAuth<TSecretProvider> (this IServiceCollection services)
        where TSecretProvider : class, IRocketJwtSecretProvider, new ()
        {
            services
                .AddScoped<IRocketJwtDateTimeUtility, RocketJwtDateTimeUtility> ()
                .AddScoped<IRocketJwtSecretProvider, TSecretProvider> ()
                .AddScoped<IRocketJwtIssuer, RocketJwtIssuer> ()
                .AddScoped<IRocketJwtTokenDecoder, RocketJwtTokenDecoder> ();
            return services;
        }
    }
}