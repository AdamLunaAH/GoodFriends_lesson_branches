using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using Models.DTO;
using DbModels;
using DbContext;

namespace DbRepos;

public class BuildingsDbRepos
{
    private ILogger<BuildingsDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public BuildingsDbRepos(ILogger<BuildingsDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }

    public async Task<ResponseItemDto<IBuilding>> ReadBuildingAsync(int id, bool flat)
    {
        if (!flat)
        {
            //make sure the model is fully populated, try without include.
            //remove tracking for all read operations for performance and to avoid recursion/circular access
            var query = _dbContext.Buildings.AsNoTracking()
                .Include(i => i.RoomDbM)
                .Where(i => i.BuildingId == id);

            return new ResponseItemDto<IBuilding>()
            {
#if DEBUG
                ConnectionString = _dbContext.dbConnection,
#endif
                Item = await query.FirstOrDefaultAsync<IBuilding>()
            };
        }
        else
        {
            //Not fully populated, compare the SQL Statements generated
            //remove tracking for all read operations for performance and to avoid recursion/circular access
            var query = _dbContext.Buildings.AsNoTracking()
                .Where(i => i.BuildingId == id);

            var resp = await query.FirstOrDefaultAsync<IBuilding>();
            return new ResponseItemDto<IBuilding>()
            {
#if DEBUG
                ConnectionString = _dbContext.dbConnection,
#endif
                Item = resp
            };
        }
    }

    public async Task<ResponsePageDto<IBuilding>> ReadBuildingsAsync(bool flat, string filter, int pageNumber, int pageSize)
    {
        filter ??= "";
        IQueryable<BuildingDbM> query;
        if (flat)
        {
            query = _dbContext.Buildings.AsNoTracking();
        }
        else
        {
            query = _dbContext.Buildings.AsNoTracking()
                .Include(i => i.RoomDbM);
        }

        var ret = new ResponsePageDto<IBuilding>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            DbItemsCount = await query

            //Adding filter functionality
            .Where(i => i.BuildingName.ToLower().Contains(filter) ||
                            i.BuildingNumber.ToString().Contains(filter)).CountAsync(),

            PageItems = await query

            //Adding filter functionality
            .Where(i => i.BuildingName.ToLower().Contains(filter) ||
                            i.BuildingNumber.ToString().Contains(filter))

            //Adding paging
            .Skip(pageNumber * pageSize)
            .Take(pageSize)

            .ToListAsync<IBuilding>(),

            PageNr = pageNumber,
            PageSize = pageSize
        };
        return ret;
    }

    public async Task<ResponseItemDto<IBuilding>> DeleteBuildingAsync(int id)
    {
        //Find the instance with matching id
        var query1 = _dbContext.Buildings
            .Where(i => i.BuildingId == id);
        var item = await query1.FirstOrDefaultAsync<BuildingDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"Item {id} is not existing");

        //delete in the database model
        _dbContext.Buildings.Remove(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();
        return new ResponseItemDto<IBuilding>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

    public async Task<ResponseItemDto<IBuilding>> UpdateBuildingAsync(BuildingCuDto itemDto)
    {
        //Find the instance with matching id and read the navigation properties.
        var query1 = _dbContext.Buildings
            .Where(i => i.BuildingId == itemDto.BuildingId);
        var item = await query1
            .Include(i => i.RoomDbM)
            .FirstOrDefaultAsync<BuildingDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"Item {itemDto.BuildingId} is not existing");

        //transfer any changes from DTO to database objects
        //Update individual properties
        item.UpdateFromDTO(itemDto);

        //Update navigation properties
        await navProp_BuildingCUdto_to_BuildingDbM(itemDto, item);

        //write to database model
        _dbContext.Buildings.Update(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadBuildingAsync(item.BuildingId, false);
    }

    public async Task<ResponseItemDto<IBuilding>> CreateBuildingAsync(BuildingCuDto itemDto)
    {
        if (itemDto.BuildingId != null)
            throw new ArgumentException($"{nameof(itemDto.BuildingId)} must be null when creating a new object");

        //transfer any changes from DTO to database objects
        //Update individual properties Building
        var item = new BuildingDbM(itemDto);

        //Update navigation properties
        await navProp_BuildingCUdto_to_BuildingDbM(itemDto, item);

        //write to database model
        _dbContext.Buildings.Add(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadBuildingAsync(item.BuildingId, false);
    }

    //from all Guid relationships in _itemDtoSrc finds the corresponding object in the database and assigns it to _itemDst
    //as navigation properties. Error is thrown if no object is found corresponing to an id.
    private async Task navProp_BuildingCUdto_to_BuildingDbM(BuildingCuDto itemDtoSrc, BuildingDbM itemDst)
    {
        if (itemDtoSrc.RoomsId != null && itemDtoSrc.RoomsId.Any())
        {
            itemDst.RoomDbM = await _dbContext.Rooms
                .Where(r => itemDtoSrc.RoomsId.Contains(r.RoomId))
                .ToListAsync();
        }
        else
        {
            itemDst.RoomDbM = new List<RoomDbM>();
        }
    }
}

