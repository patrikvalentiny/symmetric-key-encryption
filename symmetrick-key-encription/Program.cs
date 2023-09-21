using System.Text;
using symmetrick_key_encription;
Encryption encryption = new Encryption();
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
        int? option = Int32.Parse(Console.ReadLine());

        if (option == 1)
        {
            string? message = Console.ReadLine();
            var passKey = Encoding.UTF8.GetBytes(pass);
            encryption.Encrypt(message, passKey);
        }
        else if (option == 2)
        {
            
        } 
        else if (option == 0)
        {
            break;
        } 
        else
        {
            throw new Exception();
        }
        
    } catch (Exception e)
    {
        Console.WriteLine("Invalid option");
        Console.WriteLine(e.Message);
    } 
    Console.WriteLine("----------------------------------");
}