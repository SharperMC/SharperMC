using System.Security.Cryptography;

namespace SharperMC.Core.Utils.Misc
{
    public class Hex
    {
        public static string JavaHexDigest(byte[] input)
        {
            var sha1 = SHA1.Create();
            byte[] hash = sha1.ComputeHash(input);
            bool negative = (hash[0] & 0x80) == 0x80;
            if (negative) // check for negative hashes
                hash = TwosCompliment(hash);
            // Create the string and trim away the zeroes
            string digest = GetHexString(hash).TrimStart('0');
            if (negative)
                digest = "-" + digest;
            return digest;
        }

        private static string GetHexString(byte[] p)
        {
            string result = string.Empty;
            foreach (var t in p)
                result += t.ToString("x2"); // Converts to hex string

            return result;
        }

        private static byte[] TwosCompliment(byte[] p) // little endian
        {
            int i;
            bool carry = true;
            for (i = p.Length - 1; i >= 0; i--)
            {
                p[i] = (byte)~p[i];
                if (carry)
                {
                    carry = p[i] == 0xFF;
                    p[i]++;
                }
            }
            return p;
        }
    }
}