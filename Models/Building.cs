using Seido.Utilities.SeedGenerator;

namespace Models;

public class Building : IBuilding
{

    public virtual int BuildingId { get; set; }

    public virtual string BuildingName { get; set; }
    public virtual int BuildingNumber { get; set; }

    // Model relationships
    public virtual List<IRoom> Rooms { get; set; } = null;



    #region contructors
    public Building() { }

    public Building(Building org)
    {
        this.BuildingId = org.BuildingId;
        this.BuildingName = org.BuildingName;
        this.BuildingNumber = org.BuildingNumber;
        this.Rooms = (org.Rooms != null) ? org.Rooms.Select(p => new Room((Room)p)).ToList<IRoom>() : null;
    }
    #endregion

    #region randomly seed this instance
    // public bool Seeded { get; set; } = false;

    // public virtual Building Seed(SeedGenerator sgen)
    // {
    //     Seeded = true;
    //     BuildingId = Guid.NewGuid();
    //     FirstName = sgen.FirstName;
    //     LastName = sgen.LastName;
    //     Email = sgen.Email(FirstName, LastName);
    //     Birthday = (sgen.Bool) ? sgen.DateAndTime(1970, 2000) : null;

    //     return this;
    // }
    #endregion
}

