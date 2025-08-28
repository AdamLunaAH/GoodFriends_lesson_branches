using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Configuration;

public class Encryptions
{
    private readonly AesEncryptionOptions _aesOption;

    // Logger 1
    // readonly ILogger<Encryptions> _loggerEncryption;

    // public Encryptions(IOptions<AesEncryptionOptions> aesOptions)
    // {
    //     _aesOption = aesOptions.Value;
    //     _aesOption.HashKeyIv(Pbkdf2HashToBytes);
    // }




    // Logger 2
    readonly ILogger<Encryptions> _logger;
    public Encryptions(IOptions<AesEncryptionOptions> aesOptions, ILogger<Encryptions> logger)
    {
        _aesOption = aesOptions.Value;
        _aesOption.HashKeyIv(Pbkdf2HashToBytes);
        _logger = logger;
    }

    public string AesEncryptToBase64<T>(T sourceToEncrypt)
    {
        try
        {
            // throw new Exception("Just testing error logging");
            string stringToEncrypt = JsonConvert.SerializeObject(sourceToEncrypt);
            byte[] dataset = System.Text.Encoding.Unicode.GetBytes(stringToEncrypt);

            //Encrypt using AES
            byte[] encryptedBytes;
            using (SymmetricAlgorithm algorithm = Aes.Create())
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor(_aesOption.KeyHash, _aesOption.IvHash))
            {
                encryptedBytes = encryptor.TransformFinalBlock(dataset, 0, dataset.Length);
            }
            // Logger 1
            // _loggerEncryption.LogInformation($"AesEncryptToBase64 has converted {typeof(T).Name} to {encryptedBytes.Length} bytes");

            // Logger 2
            _logger.LogInformation($"{nameof(AesEncryptToBase64)} Invoked");

            return Convert.ToBase64String(encryptedBytes);
        }
        catch (Exception ex)
        {
            // Logger 1
            // _loggerEncryption.LogError($"Error in {nameof(AesEncryptToBase64)}: {ex.Message}");

            // Logger 2
            _logger.LogError($"Error in {nameof(AesEncryptToBase64)}: {ex.Message}");

            throw;

        }
    }

    public T AesDecryptFromBase64<T>(string encryptedBase64)
    {
        try
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedBase64);

            byte[] decryptedBytes;
            using (SymmetricAlgorithm algorithm = Aes.Create())
            using (ICryptoTransform decryptor = algorithm.CreateDecryptor(_aesOption.KeyHash, _aesOption.IvHash))
            {
                decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            }

            string decryptedString = System.Text.Encoding.Unicode.GetString(decryptedBytes);
            T decryptedObject = JsonConvert.DeserializeObject<T>(decryptedString);

            // Logger 1
            // _loggerEncryption.LogInformation($"AesDecryptFromBase64 has decrypted {typeof(T).Name} from {encryptedBytes.Length} bytes");

            // Logger 2
            _logger.LogInformation($"{nameof(AesDecryptFromBase64)} Invoked");
            return decryptedObject;
        }
        catch (Exception ex)
        {
            // Logger 1
            // _loggerEncryption.LogError($"Error in {nameof(AesDecryptFromBase64)}: {ex.Message}");

            // Logger 2
            _logger.LogError($"Error in {nameof(AesDecryptFromBase64)}: {ex.Message}");
            throw;
        }
    }

    public byte[] Pbkdf2HashToBytes(int nrBytes, string password)
    {
        byte[] registeredPasswordKeyDerivation = KeyDerivation.Pbkdf2(
            password: password,
            salt: Encoding.UTF8.GetBytes(_aesOption.Salt),
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: _aesOption.Iterations,
            numBytesRequested: nrBytes);

        return registeredPasswordKeyDerivation;
    }

    public string EncryptPasswordToBase64(string password)
    {
        //Hash a password using salt and streching
        byte[] encrypted = Pbkdf2HashToBytes(64, password);
        return Convert.ToBase64String(encrypted);
    }

    // Logger 1
    // public Encryptions(ILogger<Encryptions> loggerEncryption, IOptions<AesEncryptionOptions> aesOptions)
    // {
    //     _loggerEncryption = loggerEncryption;
    //     _aesOption = aesOptions.Value;
    //     _aesOption.HashKeyIv(Pbkdf2HashToBytes);
    // }
}