using Models;
using Models.DTO;

namespace Services;

public interface IBuildingsService
{
    public Task<ResponsePageDto<IBuilding>> ReadBuildingsAsync(bool flat, string filter, int pageNumber, int pageSize);
    public Task<ResponseItemDto<IBuilding>> ReadBuildingAsync(int id, bool flat);
    public Task<ResponseItemDto<IBuilding>> DeleteBuildingAsync(int id);
    public Task<ResponseItemDto<IBuilding>> UpdateBuildingAsync(BuildingCuDto item);
    public Task<ResponseItemDto<IBuilding>> CreateBuildingAsync(BuildingCuDto item);
}


