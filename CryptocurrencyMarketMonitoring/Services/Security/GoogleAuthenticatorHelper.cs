using AspNetCore.Totp;
using AspNetCore.Totp.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Services
{
    public static class GoogleAuthenticationHelper
    {
        public static bool CheckCode(int code, string userKey)
        {
            if (userKey == null) return false;

            var generator = new TotpGenerator();
            var tfaValidator = new TotpValidator(generator);

            return tfaValidator.Validate(userKey, code, 300);
        }

        public static TotpSetup GenerateUserCode(string userName, string userKey)
        {
            var setupGenerator = new TotpSetupGenerator();

            var generatedCode = setupGenerator.Generate("DVCRYPTO", userName, userKey);

            return generatedCode;
        }

    }
}
