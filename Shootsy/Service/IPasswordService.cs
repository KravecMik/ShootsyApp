namespace Shootsy.Service
{
    public interface IPasswordService
    {
        bool VerifyPassword(string pass, string passPhrase, byte[] correctPasswordHash);
        byte[] EncryptString(string pass, string passPhrase);
    }
}