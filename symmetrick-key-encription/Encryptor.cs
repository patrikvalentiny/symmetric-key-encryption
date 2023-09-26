using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using System.Text.Json;

namespace symmetrick_key_encription;

public class Encryptor
{
    private const string DocPath = "D:\\SchoolWork\\symmetrick-key-encription\\encryptedmessages";

    public void Encrypt(string message, string key, string? filename = null)
    {
        // hash the password
        byte[] salt = RandomNumberGenerator.GetBytes(256 / 8);
        
        byte[] hashedKey = HashPassword(key, salt);
        
       
        // encrypt the message
        using var aes = new AesGcm(hashedKey, AesGcm.TagByteSizes.MaxSize);
        
        var iv = new byte[AesGcm.NonceByteSizes.MaxSize]; // MaxSize = 12
        
        RandomNumberGenerator.Fill(iv);
        
        byte[] plaintextBytes = Encoding.Unicode.GetBytes(message);
        byte[] encrypted = new byte[plaintextBytes.Length];
        var tag = new byte[AesGcm.TagByteSizes.MaxSize]; // MaxSize = 16
        
        aes.Encrypt(iv, plaintextBytes, encrypted, tag);
        
        Message outMessage = new Message()
        {
            Salt = Convert.ToBase64String(salt),
            IV = Convert.ToBase64String(iv),
            CipherText = Convert.ToBase64String(encrypted),
            Tag = Convert.ToBase64String(tag)
        };
        
        // write to file
        var outFile = JsonSerializer.Serialize(outMessage); 
        File.WriteAllText($"{DocPath}\\{filename ?? DateTime.UtcNow.ToString("yyMMddHHmmss")}.json", outFile);
    }
    
    public string Decrypt(string key, string filename)
    {
        // read from file
        string json = File.ReadAllText($"{DocPath}\\{filename}.json");
        Message? inMessage = JsonSerializer.Deserialize<Message>(json);
        
        byte[] salt = Convert.FromBase64String(inMessage.Salt);
        byte[] iv = Convert.FromBase64String(inMessage.IV);
        byte[] cipherText = Convert.FromBase64String(inMessage.CipherText);
        byte[] tag = Convert.FromBase64String(inMessage.Tag);
        
        // hash the password
        byte[] hashedKey = HashPassword(key, salt);
        
        // decrypt the message
        using var aes = new AesGcm(hashedKey, AesGcm.TagByteSizes.MaxSize);
        
        byte[] decrypted = new byte[cipherText.Length];
        
        aes.Decrypt(iv, cipherText, tag, decrypted);

        return Encoding.Unicode.GetString(decrypted);
    }
    
    private byte[] HashPassword(string password, byte[] salt)
    {
        return KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 1000000,
            numBytesRequested: 256 / 8);
    }
}