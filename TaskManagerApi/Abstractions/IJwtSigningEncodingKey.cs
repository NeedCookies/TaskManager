using Microsoft.IdentityModel.Tokens;

namespace TaskManagerApi.Abstractions
{
    public interface IJwtSigningEncodingKey
    {
        string SigningAlgoritm { get; }
        SecurityKey GetKey();
    }
}
