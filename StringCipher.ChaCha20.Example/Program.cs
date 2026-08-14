using StringCipher.ChaCha20;



string password = "Random password";

//String example with string password
string stringPayload = "Hallo World!";
//Encrypt a string into a new encrypted string
var encryptedText = ChaChaCipher.EncryptStringToString(stringPayload, password);
Console.WriteLine(encryptedText);

//Decrypt string 
var decryptedText = ChaChaCipher.DecryptToString(encryptedText, password);

Console.WriteLine("Same text: " + (decryptedText == stringPayload));
Console.WriteLine("Decrypted text " + decryptedText);

//Byte[] example with string password

byte[] bytesPayload = Guid.NewGuid().ToByteArray();
byte[] encryptedBytes = ChaChaCipher.Encrypt(bytesPayload, password);
byte[] decryptedBytes = ChaChaCipher.Decrypt(encryptedBytes, password);

Console.WriteLine("Same payload: " + bytesPayload.SequenceEqual(decryptedBytes));

//String example with byte[] key

byte[] key = ChaChaCipher.GenerateKey(); // The key must always be 32 bytes long. 

encryptedBytes = ChaChaCipher.EncryptString(stringPayload, key);

decryptedText = ChaChaCipher.DecryptToString(encryptedBytes, key);
Console.WriteLine("Same text: " + (decryptedText == stringPayload));
Console.WriteLine("Decrypted text " + decryptedText);




