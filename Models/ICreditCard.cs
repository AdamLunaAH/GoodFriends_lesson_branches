namespace Models;

public enum CardIssuer
{
    Visa,
    MasterCard,
    AmericanExpress,
    DinersClub
}

public interface ICreditCard
{
    public Guid CreditCardId { get; set; }
    public CardIssuer Issuer { get; set; }
    // public string IssuerString { get; set; }
    public String CardNumber { get; set; }
    public string ExpiryMonth { get; set; }
    public string ExpiryYear { get; set; }

}