using System.Security.Cryptography;
using System.Text;

namespace symmetrick_key_encription;

public class Encryption
{
    public void Encrypt(string message, byte[] key)
    {
        using var aes = new AesGcm(key, AesGcm.TagByteSizes.MaxSize);
        byte[] size32key = new byte[32];
        Array.Copy(key, size32key, key.Length);
        var iv = new byte[AesGcm.NonceByteSizes.MaxSize]; // MaxSize = 12
        RandomNumberGenerator.Fill(iv);
        
        byte[] plaintextBytes = Encoding.Unicode.GetBytes(message);
        byte[] encrypted = new byte[plaintextBytes.Length];
        var tag = new byte[AesGcm.TagByteSizes.MaxSize]; // MaxSize = 16
        
        aes.Encrypt(iv, plaintextBytes, encrypted, tag);

        Console.WriteLine($"Encrypted message: {Convert.ToBase64String(encrypted)}");
    }
}