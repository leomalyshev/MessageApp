using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MessageService.Security
{
    public class JwtConfiguration
    {
        public required string Key { get; init; }
        public required string Issuer { get; init; }
        public required string Audience { get; init; }
        public JwtConfiguration()
        {
            Key = "defaultSecretKey";
            Issuer = "defaultIssuer";
            Audience = "defaultAudience";
        }
        internal SymmetricSecurityKey GetSigningKey()
        {
            return new(Encoding.UTF8.GetBytes(Key));
        }
    }
}
