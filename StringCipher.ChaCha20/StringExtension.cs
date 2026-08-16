namespace StringCipher.ChaCha20;

public static class StringExtension
{
    public static string EncryptString(this string str, string password) => ChaChaCipher.EncryptStringToString(str, password);
    public static string EncryptString(this string str, byte[] key) => ChaChaCipher.EncryptStringToString(str, key);

    public static string DecryptString(this string str, string password) => ChaChaCipher.DecryptToString(str, password);
    public static string DecryptString(this string str, byte[] key) => ChaChaCipher.DecryptToString(str, key);


}
