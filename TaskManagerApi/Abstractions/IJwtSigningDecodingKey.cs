using Microsoft.IdentityModel.Tokens;

namespace TaskManagerApi.Abstractions
{
    public interface IJwtSigningDecodingKey
    {
        SecurityKey GetKey();
    }
}
