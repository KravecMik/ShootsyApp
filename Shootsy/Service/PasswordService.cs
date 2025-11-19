using System.Security.Cryptography;
using System.Text;

namespace Shootsy.Service
{
    public class PasswordService : IPasswordService
    {
        private const string initVector = "pemgail9uzpgzl88";
        private const int keysize = 256;

        public bool VerifyPassword(string pass, string passPhrase, byte[] correctPasswordHash)
        {
            byte[] hashedPassword = EncryptString(pass, passPhrase);
            return hashedPassword.SequenceEqual(correctPasswordHash);
        }

        public byte[] EncryptString(string pass, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(pass);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return cipherTextBytes;
        }
    }
}