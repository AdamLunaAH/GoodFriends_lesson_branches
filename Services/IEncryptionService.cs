using Models;
namespace Services;

public interface IEncryptionService
{
    IEncryptedCard EncryptCardData(ICreditCard creditCard);
}