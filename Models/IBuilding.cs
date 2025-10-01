namespace Models;

public interface IBuilding
{
    public int BuildingId { get; set; }

    public string BuildingName { get; set; }
    public int BuildingNumber { get; set; }
    public List<IRoom> Rooms { get; set; }

}


