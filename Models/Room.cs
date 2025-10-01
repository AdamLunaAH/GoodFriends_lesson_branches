using Seido.Utilities.SeedGenerator;

namespace Models;

public class Room : IRoom
{

    public virtual int RoomId { get; set; }

    public virtual string RoomName { get; set; }
    public virtual int RoomLevel { get; set; }

    // Model relationships
    public virtual IBuilding Building { get; set; } = null;




    #region contructors
    public Room() { }

    public Room(Room org)
    {
        this.RoomId = org.RoomId;
        this.RoomName = org.RoomName;
        this.RoomLevel = org.RoomLevel;
        // this.Building = (org.Building != null) ? new Building((Building)org.Building) : null;

    }
    #endregion

    #region randomly seed this instance
    // public bool Seeded { get; set; } = false;

    // public virtual Room Seed(SeedGenerator sgen)
    // {
    //     Seeded = true;
    //     RoomId = Guid.NewGuid();
    //     FirstName = sgen.FirstName;
    //     LastName = sgen.LastName;
    //     Email = sgen.Email(FirstName, LastName);
    //     Birthday = (sgen.Bool) ? sgen.DateAndTime(1970, 2000) : null;

    //     return this;
    // }
    #endregion
}

