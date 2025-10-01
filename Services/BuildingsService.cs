using Microsoft.Extensions.Logging;

using Models;
using Models.DTO;
using DbRepos;

namespace Services;

public class BuildingsServiceDb : IBuildingsService
{
    private readonly BuildingsDbRepos _repo = null;
    private readonly ILogger<BuildingsServiceDb> _logger = null;

    public BuildingsServiceDb(BuildingsDbRepos repo)
    {
        _repo = repo;
    }
    public BuildingsServiceDb(BuildingsDbRepos repo, ILogger<BuildingsServiceDb> logger) : this(repo)
    {
        _logger = logger;
    }

    //Simple 1:1 calls in this case, but as Services expands, this will no longer need to be the case
    public Task<ResponsePageDto<IBuilding>> ReadBuildingsAsync(bool flat, string filter, int pageNumber, int pageSize) => _repo.ReadBuildingsAsync(flat, filter, pageNumber, pageSize);
    public Task<ResponseItemDto<IBuilding>> ReadBuildingAsync(int id, bool flat) => _repo.ReadBuildingAsync(id, flat);
    public Task<ResponseItemDto<IBuilding>> DeleteBuildingAsync(int id) => _repo.DeleteBuildingAsync(id);
    public Task<ResponseItemDto<IBuilding>> UpdateBuildingAsync(BuildingCuDto item) => _repo.UpdateBuildingAsync(item);
    public Task<ResponseItemDto<IBuilding>> CreateBuildingAsync(BuildingCuDto item) => _repo.CreateBuildingAsync(item);
}

