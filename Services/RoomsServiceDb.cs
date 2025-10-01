using Microsoft.Extensions.Logging;

using Models;
using Models.DTO;
using DbRepos;

namespace Services;

public class RoomsServiceDb : IRoomsService
{
    private readonly RoomsDbRepos _repo = null;
    private readonly ILogger<RoomsServiceDb> _logger = null;

    public RoomsServiceDb(RoomsDbRepos repo)
    {
        _repo = repo;
    }
    public RoomsServiceDb(RoomsDbRepos repo, ILogger<RoomsServiceDb> logger) : this(repo)
    {
        _logger = logger;
    }

    //Simple 1:1 calls in this case, but as Services expands, this will no longer need to be the case
    public Task<ResponsePageDto<IRoom>> ReadRoomsAsync(bool flat, string filter, int pageNumber, int pageSize) => _repo.ReadRoomsAsync(flat, filter, pageNumber, pageSize);
    public Task<ResponseItemDto<IRoom>> ReadRoomAsync(int id, bool flat) => _repo.ReadRoomAsync(id, flat);
    public Task<ResponseItemDto<IRoom>> DeleteRoomAsync(int id) => _repo.DeleteRoomAsync(id);
    public Task<ResponseItemDto<IRoom>> UpdateRoomAsync(RoomCuDto item) => _repo.UpdateRoomAsync(item);
    public Task<ResponseItemDto<IRoom>> CreateRoomAsync(RoomCuDto item) => _repo.CreateRoomAsync(item);
}

