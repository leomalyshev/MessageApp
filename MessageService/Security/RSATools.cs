using System.Security.Cryptography;

namespace MessageService.Security
{
    public static class RSATools
    {
        public static RSA GetPrivateKey(IConfiguration configuration)
        {
            var privateKeyPem = configuration["Keys:PrivateKey"];
            var rsa = RSA.Create();
            rsa.ImportFromPem(privateKeyPem);
            return rsa;
        }

        public static RSA GetPublicKey(IConfiguration configuration)
        {
            var publicKeyPem = configuration["Keys:PublicKey"];
            var rsa = RSA.Create();
            rsa.ImportFromPem(publicKeyPem);
            return rsa;
        }
    }
}
