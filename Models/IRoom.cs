namespace Models;

public interface IRoom
{
    public int RoomId { get; set; }

    public string RoomName { get; set; }
    public int RoomLevel { get; set; }
    public IBuilding Building { get; set; }

}


