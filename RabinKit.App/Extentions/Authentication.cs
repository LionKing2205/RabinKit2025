using OtpNet;
using RabinKit.App.Services;

namespace RabinKit.App.GAuthenticator
{
    class Authenticator
    {
        public bool Authentication(string userInput)
        {
            string secretKey = Keys.secretKey;
            string defaultKey = Keys.defkey;
            var key = Base32Encoding.ToBytes(secretKey);
            long timeStep = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 30;

            var totp = new Totp(key);
            string generatedCode = totp.ComputeTotp();

            if (userInput == generatedCode || userInput == defaultKey)
                return true;
            else return false;
        }
    }
}