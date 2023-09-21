using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using System.Text.Json;

namespace symmetrick_key_encription;

public class Encryption
{
    public void Encrypt(string message, string key)
    {
        // hash the password
        byte[] salt = RandomNumberGenerator.GetBytes(256 / 8);
        
        byte[] hashedKey = KeyDerivation.Pbkdf2(
            password: key,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8);
        
        Console.Out.WriteLine("hashedKey = {0}", Convert.ToBase64String(hashedKey));
        
        // encrypt the message
        using var aes = new AesGcm(hashedKey, AesGcm.TagByteSizes.MaxSize);
        
        var iv = new byte[AesGcm.NonceByteSizes.MaxSize]; // MaxSize = 12
        
        RandomNumberGenerator.Fill(iv);
        
        byte[] plaintextBytes = Encoding.Unicode.GetBytes(message);
        byte[] encrypted = new byte[plaintextBytes.Length];
        var tag = new byte[AesGcm.TagByteSizes.MaxSize]; // MaxSize = 16
        
        aes.Encrypt(iv, plaintextBytes, encrypted, tag);

        // Console.WriteLine($"Encrypted message: {Convert.ToBase64String(encrypted)}");

        Message outMessage = new Message()
        {
            Salt = Convert.ToBase64String(salt),
            IV = Convert.ToBase64String(iv),
            CipherText = Convert.ToBase64String(encrypted),
            Tag = Convert.ToBase64String(tag)
        };
        
        // Console.WriteLine($"Salt: {outMessage.Salt}");
        // Console.WriteLine($"IV: {outMessage.IV}");
        // Console.WriteLine($"CipherText: {outMessage.CipherText}");
        // Console.WriteLine($"Tag: {outMessage.Tag}");

        var outFile = JsonSerializer.Serialize(outMessage);
        string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        File.WriteAllText("D:\\SchoolWork\\symmetrick-key-encription\\out.json", outFile);
    }
}