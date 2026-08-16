# ChaCha20 string cipher 

Lightweight encrypting and decrypting solution for strings and data (byte[]). It uses latest ChaCha20-Poly1305 encryption.

The data is encoded in a Unicode-compatible format. 

#### Requires .NET 8 or higher

Nuget: https://www.nuget.org/packages/StringCipher.ChaCha20
```
dotnet add package StringCipher.ChaCha20 --version 1.0.0
```

### How to use?

#### Import namespace
```csharp
using StringCipher.ChaCha20;
```
#### String example
```csharp
string password = "Random password";
string stringPayload = "Hallo World!";
//Encrypt a string into a new encrypted string
string encryptedText = ChaChaCipher.EncryptStringToString(stringPayload, password);
//Decrypt string 
string decryptedText = ChaChaCipher.DecryptToString(encryptedText, password);
```

#### Byte[] example with string password
```csharp
byte[] bytesPayload = Guid.NewGuid().ToByteArray();
byte[] encryptedBytes = ChaChaCipher.Encrypt(bytesPayload, password);
byte[] decryptedBytes = ChaChaCipher.Decrypt(encryptedBytes, password);
```

#### String example with byte[] key
```csharp
byte[] key = ChaChaCipher.GenerateKey(); // The key must always be 32 bytes long. 
byte[] encryptedBytes = ChaChaCipher.EncryptString(stringPayload, key);
string decryptedText = ChaChaCipher.DecryptToString(encryptedBytes, key);
```

#### Using Extension Methods 
```csharp
string encryptedText = stringPayload.EncryptString(password);
string decryptedText = encryptedText.DecryptString(password);
```