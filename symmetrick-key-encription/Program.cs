using symmetrick_key_encription;


Encryptor encryption = new Encryptor();

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("Passphrase: ");
Console.ResetColor();

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
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("1: Safely store message\n2: Read message\n0: Exit");
    Console.ResetColor();
    try
    {
        int? option = Int32.Parse(Console.ReadLine() ?? string.Empty);

        if (option == 1)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Message: ");
            Console.ResetColor();
            var message = Console.ReadLine();

            if (message != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Filename: ");
                Console.ResetColor();
                var filename = Console.ReadLine();
                if (filename == string.Empty)
                {
                    filename = null;
                }
                encryption.Encrypt(message, pass, filename: filename);
            }
            else
            {
                Console.WriteLine("Message can't be empty");
            }
        }
        else if (option == 2)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Filename: ");
            Console.ResetColor();
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
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(e.Message);
        Console.ResetColor();
    } 
    Console.WriteLine("----------------------------------");
}