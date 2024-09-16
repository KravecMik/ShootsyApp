using System.Security.Cryptography;
using System.Text;

namespace Shootsy.Security
{
    public static class OtherMethods
    {
        private const string initVector = "pemgail9uzpgzl88";
        private const int keysize = 256;

        public static byte[] EncryptString(this string pass, string passPhrase)
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

        public static bool IsPasswordValid(this string pass, string passPhrase, byte[] correctPasswordHash)
        {
            byte[] hashedPassword = EncryptString(pass, passPhrase);
            return hashedPassword.SequenceEqual(correctPasswordHash);
        }

        //public static string DecryptString(byte[] cipherText, string passPhrase)
        //{
        //    byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
        //    PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
        //    byte[] keyBytes = password.GetBytes(keysize / 8);
        //    RijndaelManaged symmetricKey = new RijndaelManaged();
        //    symmetricKey.Mode = CipherMode.CBC;
        //    ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
        //    MemoryStream memoryStream = new MemoryStream(cipherText);
        //    CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        //    byte[] plainTextBytes = new byte[cipherText.Length];
        //    int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
        //    memoryStream.Close();
        //    cryptoStream.Close();
        //    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        //}
    }
}
