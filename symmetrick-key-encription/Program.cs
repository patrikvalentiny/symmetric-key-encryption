using System.Text;
using symmetrick_key_encription;
Encryptor encryption = new Encryptor();
Console.WriteLine("Passphrase: ");

var pass = string.Empty;
ConsoleKey key;
do
{
    var keyInfo = Console.ReadKey(intercept: true);
    key = keyInfo.Key;

    if (key == ConsoleKey.Backspace && pass.Length > 0)
    {
        Console.Write("\b \b");
        pass = pass[0..^1];
    }
    else if (!char.IsControl(keyInfo.KeyChar))
    {
        Console.Write("*");
        pass += keyInfo.KeyChar;
    }
} while (key != ConsoleKey.Enter);


Console.WriteLine();
Console.WriteLine("----------------------------------");


while (true)
{
    Console.WriteLine("1: Safely store message\n2: Read message\n0: Exit");
    try
    {
        int? option = Int32.Parse(Console.ReadLine() ?? string.Empty);

        if (option == 1)
        {
            Console.Write("Message: ");
            var message = Console.ReadLine();

            if (message != null)
            {
                encryption.Encrypt(message, pass);
            }
            else
            {
                Console.WriteLine("Message is empty");
            }
        }
        else if (option == 2)
        {
            string? filename = Console.ReadLine();
            Console.WriteLine(encryption.Decrypt(pass, filename ?? ""));
        } 
        else if (option == 0)
        {
            break;
        } 
        else
        {
            Console.WriteLine("Invalid option");
        }
        
    } catch (Exception e)
    {
        Console.WriteLine(e.Message);
    } 
    Console.WriteLine("----------------------------------");
}