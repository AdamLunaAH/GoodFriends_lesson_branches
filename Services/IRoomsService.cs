using Models;
using Models.DTO;

namespace Services;

public interface IRoomsService
{
    public Task<ResponsePageDto<IRoom>> ReadRoomsAsync(bool flat, string filter, int pageNumber, int pageSize);
    public Task<ResponseItemDto<IRoom>> ReadRoomAsync(int id, bool flat);
    public Task<ResponseItemDto<IRoom>> DeleteRoomAsync(int id);
    public Task<ResponseItemDto<IRoom>> UpdateRoomAsync(RoomCuDto item);
    public Task<ResponseItemDto<IRoom>> CreateRoomAsync(RoomCuDto item);
}


