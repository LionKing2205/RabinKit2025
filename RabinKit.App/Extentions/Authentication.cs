using OtpNet;
using RabinKit.App.Services;

namespace RabinKit.App.GAuthenticator
{
    class Authenticator
    {
        public bool Authentication(string userInput)
        {
            string secretKey = Keys.secretKey;
            var key = Base32Encoding.ToBytes(secretKey);
            long timeStep = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 30;

            var totp = new Totp(key);
            string generatedCode = totp.ComputeTotp();

            return userInput == generatedCode;
        }
    }
}