namespace Models.DTO;

//DTO is a DataTransferObject, can be instanstiated by the controller logic
//and represents a, fully instantiable, subset of the Database models
//for a specific purpose.

//These DTO are simplistic and used to Update and Create objects
public class FriendCuDto
{
    public virtual Guid? FriendId { get; set; }

    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }

    public virtual string Email { get; set; }

    public DateTime? Birthday { get; set; } = null;

    public virtual Guid? AddressId { get; set; } = null;

    public virtual List<Guid> PetsId { get; set; } = null;

    public virtual List<Guid> QuotesId { get; set; } = null;

    public FriendCuDto() { }
    public FriendCuDto(IFriend org)
    {
        FriendId = org.FriendId;
        FirstName = org.FirstName;
        LastName = org.LastName;
        Email = org.Email;
        Birthday = org.Birthday;

        AddressId = org?.Address?.AddressId;
        PetsId = org.Pets?.Select(i => i.PetId).ToList();
        QuotesId = org.Quotes?.Select(i => i.QuoteId).ToList();
    }
}

public class AddressCuDto
{
    public virtual Guid? AddressId { get; set; }

    public virtual string StreetAddress { get; set; }
    public virtual int ZipCode { get; set; }
    public virtual string City { get; set; }
    public virtual string Country { get; set; }

    public virtual List<Guid> FriendsId { get; set; } = null;

    public AddressCuDto() { }
    public AddressCuDto(IAddress org)
    {
        AddressId = org.AddressId;
        StreetAddress = org.StreetAddress;
        ZipCode = org.ZipCode;
        City = org.City;
        Country = org.Country;

        FriendsId = org.Friends?.Select(i => i.FriendId).ToList();
    }
}

public class PetCuDto
{
    //cannot be nullable as a Pets has to have an owner even when created
    public virtual Guid FriendId { get; set; }

    public virtual Guid? PetId { get; set; }

    public virtual AnimalKind Kind { get; set; }
    public virtual string Name { get; set; }
    public AnimalMood Mood { get; set; }

    public PetCuDto() { }
    public PetCuDto(IPet org)
    {
        FriendId = org.Friend.FriendId;

        PetId = org.PetId;
        Kind = org.Kind;
        Name = org.Name;
        Mood = org.Mood;
    }
}

public class QuoteCuDto
{
    public virtual Guid? QuoteId { get; set; }
    public virtual string Quote { get; set; }
    public virtual string Author { get; set; }

    public virtual List<Guid> FriendsId { get; set; } = null;


    public QuoteCuDto() { }
    public QuoteCuDto(IQuote org)
    {
        QuoteId = org.QuoteId;

        Quote = org.QuoteText;
        Author = org.Author;

        FriendsId = org.Friends?.Select(i => i.FriendId).ToList();
    }
}


// namespace Models.DTO;
public class BuildingCuDto
{
    public virtual int? BuildingId { get; set; }

    public virtual string BuildingName { get; set; }
    public virtual int BuildingNumber { get; set; }

    // A building can have many rooms
    public virtual List<int> RoomsId { get; set; } = new();

    public BuildingCuDto() { }

    public BuildingCuDto(IBuilding org)
    {
        BuildingId = org.BuildingId;
        BuildingName = org.BuildingName;
        BuildingNumber = org.BuildingNumber;
        RoomsId = org.Rooms?.Select(i => i.RoomId).ToList() ?? new List<int>();
    }
}


public class RoomCuDto
{
    public virtual int? RoomId { get; set; }

    public virtual string RoomName { get; set; }
    public virtual int RoomLevel { get; set; }

    // foreign key to Building
    public virtual int? BuildingId { get; set; }

    public RoomCuDto() { }

    public RoomCuDto(IRoom org)
    {
        RoomId = org.RoomId;
        RoomName = org.RoomName;
        RoomLevel = org.RoomLevel;
        BuildingId = org?.Building?.BuildingId;
    }
}
