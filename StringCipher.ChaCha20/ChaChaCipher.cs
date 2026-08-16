using System.Security.Cryptography;
using System.Text;

namespace StringCipher.ChaCha20;

public static class ChaChaCipher
{
    // ChaCha20-Poly1305 requirements:
    // Key:   32 bytes
    // Nonce: 12 bytes
    // Tag:   16 bytes

    private static readonly Encoding StringEncoding = Encoding.Unicode;
    private static readonly int NonceLength = 12;
    private static readonly int TagLength = 16;
    private static readonly int KeyLength = 32;

    public static string EncryptStringToString(string plaintext, string key) => Convert.ToBase64String(EncryptString(plaintext, key));
    public static string EncryptStringToString(string plaintext, byte[] key) => Convert.ToBase64String(EncryptString(plaintext, key));
    public static string EncryptStringToString(byte[] plaintext, byte[] key) => Convert.ToBase64String(Encrypt(plaintext, key));
    public static byte[] EncryptString(string plaintext, string key) => EncryptString(plaintext, GetKeyForPassword(key));
    public static byte[] EncryptString(string plaintext, byte[] key) => Encrypt(StringEncoding.GetBytes(plaintext), key);
    public static byte[] Encrypt(byte[] plaintext, string key) => Encrypt(plaintext, GetKeyForPassword(key));
    public static byte[] Encrypt(byte[] plaintext, byte[] key)
    {
        ArgumentNullException.ThrowIfNull(plaintext);

        if (key is null || key.Length != KeyLength)
            throw new ArgumentException($"Key must be exactly {KeyLength} bytes.", nameof(key));

        byte[] nonce = RandomNumberGenerator.GetBytes(NonceLength);
        byte[] ciphertext = new byte[plaintext.Length];
        byte[] tag = new byte[TagLength];

        using var cipher = new ChaCha20Poly1305(key);
        cipher.Encrypt(nonce, plaintext, ciphertext, tag);

        // [12-byte nonce][16-byte tag][ciphertext]
        byte[] result = new byte[NonceLength + TagLength + ciphertext.Length];

        Buffer.BlockCopy(nonce, 0, result, 0, NonceLength);
        Buffer.BlockCopy(tag, 0, result, NonceLength, TagLength);
        Buffer.BlockCopy(ciphertext, 0, result, NonceLength + TagLength, ciphertext.Length);

        return result;
    }

    public static string DecryptToString(byte[] encryptedData, byte[] key) => StringEncoding.GetString(Decrypt(encryptedData, key));
    public static string DecryptToString(string encryptedData, byte[] key) => DecryptToString(Convert.FromBase64String(encryptedData), key);
    public static string DecryptToString(string encryptedData, string key) => DecryptToString(encryptedData, GetKeyForPassword(key));
    public static byte[] Decrypt(byte[] encryptedData, string key) => Decrypt(encryptedData, GetKeyForPassword(key));
    public static byte[] Decrypt(string encryptedData, string key) => Decrypt(Convert.FromBase64String(encryptedData), GetKeyForPassword(key));
    public static byte[] Decrypt(byte[] encryptedData, byte[] key)
    {
        ArgumentNullException.ThrowIfNull(encryptedData);

        if (encryptedData.Length < (NonceLength + TagLength))
            throw new ArgumentException("Encrypted data is too short.", nameof(encryptedData));

        if (key == null || key.Length != KeyLength)
            throw new ArgumentException($"Key must be exactly {KeyLength} bytes.", nameof(key));
        // [12-byte nonce][16-byte tag][ciphertext]
        byte[] nonce = encryptedData[..NonceLength];
        byte[] tag = encryptedData[NonceLength..(NonceLength + TagLength)];
        byte[] ciphertext = encryptedData[(NonceLength + TagLength)..];

        byte[] plaintext = new byte[ciphertext.Length];

        using var cipher = new ChaCha20Poly1305(key);

        // Throws AuthenticationTagMismatchException
        // if the ciphertext/tag/key was modified or incorrect.
        cipher.Decrypt(nonce, ciphertext, tag, plaintext);

        return plaintext;
    }

    public static byte[] GenerateKey()
    {
        return RandomNumberGenerator.GetBytes(KeyLength);
    }

    public static byte[] GetKeyForPassword(string password)
    {
        //Need always a 32bit (256bit) key. 
        var inputBytes = Encoding.UTF8.GetBytes(password);
        var key = SHA256.HashData(inputBytes);
        if (key.Length != KeyLength)
            throw new ArgumentException($"Key must be exactly {KeyLength} bytes.", nameof(password));
        return key;
    }
}
