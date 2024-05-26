using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UserService.Abstraction;
using UserService.DTO;
using UserService.Model;
using UserService.Security;

namespace UserService.Services
{
    public class TokenService(JwtConfiguration jwt):ITokenService
    {
        public string GenerateToken(LoginViewModel loginViewModel)
        {
            var securityKey = new RsaSecurityKey(RSATools.GetPrivateKey());
            var credentilas = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256Signature);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, loginViewModel.Email),
                new Claim(ClaimTypes.Role, loginViewModel.UserRole.ToString())
            };
            var token = new JwtSecurityToken
            (
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentilas
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
