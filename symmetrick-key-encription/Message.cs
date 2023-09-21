namespace symmetrick_key_encription;

public class Message
{
    public string Salt {set; get;}
    public string IV {set; get;}
    public string CipherText {set; get;}
    public string Tag {set; get;}
    
}