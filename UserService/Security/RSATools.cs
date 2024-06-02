using System.Security.Cryptography;

namespace UserService.Security
{
    public static class RSATools
    {
        public static RSA GetPrivateKey(IConfiguration configuration)
        {
            var privateKeyPem = configuration["Keys:PrivateKey"];
            Console.WriteLine($"Public Key: {privateKeyPem}"); // Временный вывод для отладки
            var rsa = RSA.Create();
            rsa.ImportFromPem(privateKeyPem);
            return rsa;
        }

        public static RSA GetPublicKey(IConfiguration configuration)
        {
            var publicKeyPem = configuration["Keys:PublicKey"];
            Console.WriteLine($"Public Key: {publicKeyPem}"); // Временный вывод для отладки
            var rsa = RSA.Create();
            rsa.ImportFromPem(publicKeyPem);
            return rsa;
        }
    }
}
