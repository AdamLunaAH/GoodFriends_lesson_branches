using Microsoft.Extensions.Logging;
using Configuration;
using Models;
using Seido.Utilities.SeedGenerator;

namespace Services;

public class EncryptionService : IEncryptionService
{
    private readonly Encryptions _encryptions;
    private readonly ILogger<EncryptionService> _logger;

    public EncryptionService(Encryptions encryptions, ILogger<EncryptionService> logger)
    {
        _encryptions = encryptions;
        _logger = logger;
    }

    public IEncryptedCard EncryptCardData(ICreditCard creditCard)
    {
        if (creditCard == null)
            throw new ArgumentNullException(nameof(creditCard));

        try
        {
            // Encrypt credit card
            string encryptedData = _encryptions.AesEncryptToBase64(creditCard);

            // Return encrypted card data
            return new EncryptedCard
            {
                EncryptedCardData = encryptedData
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while encrypting credit card data");
            throw;
        }
    }
}
