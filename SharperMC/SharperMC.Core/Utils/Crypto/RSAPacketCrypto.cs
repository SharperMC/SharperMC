using System;
using System.Security.Cryptography;
using SharperMC.Core.Utils.Encryption;

namespace SharperMC.Core.Utils.Crypto
{
    public class RSAPacketCrypto
    {
        private readonly RSAParameters _privateKey;
        private readonly RSAParameters _publicKey;

        public byte[] PublicKey;

        public RSAPacketCrypto()
        {
            var rsaProvider = new RSACryptoServiceProvider(1024);

            _privateKey = rsaProvider.ExportParameters(true);
            _publicKey = rsaProvider.ExportParameters(false);

            PublicKey = AsnKeyBuilder.PublicKeyToX509(_publicKey).GetBytes();
        }

        public byte[] GenerateByteToken(int length)
        {
            var token = new byte[length];
            var provider = new RNGCryptoServiceProvider();
            provider.GetBytes(token);
            return token;
        }
        
        public RijndaelManaged GenerateAes(byte[] key)
        {
            var cipher = new RijndaelManaged
            {
                            Mode = CipherMode.CBC,
                            Padding = PaddingMode.None,
                            KeySize = 128,
                            FeedbackSize = 8,
                            Key = key,
                            IV = (byte[]) key.Clone()
            };
            return cipher;
        }

        public byte[] Decrypt(byte[] data)
        {
            return RsaDecrypt(data, _privateKey, false);
        }

        public byte[] RsaDecrypt(byte[] data, RSAParameters rsaPrivateParameter, bool oaepPadding)
        {
            try
            {
                using RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(rsaPrivateParameter);
                return rsa.Decrypt(data, oaepPadding);
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine(ex + " | " + ex.Message);
                return null;
            }
        }

        public byte[] RsaEncrypt(byte[] data, RSAParameters rsaPublicParameter, bool oaepPadding)
        {
            try
            {
                using RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(rsaPublicParameter);
                return rsa.Encrypt(data, oaepPadding);
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine(ex + " | " + ex.Message);
                return null;
            }
        }
    }
}