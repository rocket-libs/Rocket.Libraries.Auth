using System.Threading.Tasks;

namespace Rocket.Libraries.Auth
{
    public interface IRocketJwtSecretProvider
    {
        Task<string> GetSecretAsync ();
    }
}