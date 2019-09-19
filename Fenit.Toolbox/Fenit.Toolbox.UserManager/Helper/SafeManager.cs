using System;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web.Security;
using Fenit.Toolbox.UserManager.ViewModel;

namespace Fenit.Toolbox.UserManager.Helper
{
    public static class SafeManager
    {
        private const string FixedSalt = "X2#a78asKD!9!FoJZ3$sdfld5dfd5294yboCJK(I#!(CIIJS";

        public static UserData GetUsetData(this IPrincipal principal)
        {
            var result = new UserData();
            if (principal.Identity is FormsIdentity identity) result = new UserData(identity.Ticket.UserData);
            return result;
        }

        public static int GetUsetId(this IPrincipal principal)
        {
            return principal.GetUsetData().Id;
        }

        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var res = new StringBuilder();
            var rnd = new Random();
            while (0 < length--) res.Append(valid[rnd.Next(valid.Length)]);
            return res.ToString();
        }

        public static string GetHashString(string password, string salt)

        {
            var s = salt + password + FixedSalt;

            return StringToSha256String(s);
        }

        public static void HashString(string password, out string hash, out string salt)
        {
            var uniqueSalt = Guid.NewGuid().ToString();
            var s = uniqueSalt + password + FixedSalt;
            hash = StringToSha256String(s);
            salt = uniqueSalt;
        }

        private static string DecodeFrom64(string encodedData)
        {
            var base64 = Convert.FromBase64String(encodedData);
            var bytValue = Encoding.UTF8.GetString(base64);
            return bytValue;
        }

        private static string EncodeTo64(string toEncode)
        {
            var bytValue = Encoding.UTF8.GetBytes(toEncode);
            var base64 = Convert.ToBase64String(bytValue);
            return base64;
        }

        private static string StringToSha256String(string s)
        {
            var sb = new StringBuilder();
            using (var hash = SHA256.Create())
            {
                var enc = Encoding.Unicode;
                var result = hash.ComputeHash(enc.GetBytes(s));
                foreach (var b in result) sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}