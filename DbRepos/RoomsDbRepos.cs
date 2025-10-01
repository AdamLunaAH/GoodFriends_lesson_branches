using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using Models.DTO;
using DbModels;
using DbContext;

namespace DbRepos;

public class RoomsDbRepos
{
    private ILogger<RoomsDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public RoomsDbRepos(ILogger<RoomsDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }

    public async Task<ResponseItemDto<IRoom>> ReadRoomAsync(int id, bool flat)
    {
        if (!flat)
        {
            //make sure the model is fully populated, try without include.
            //remove tracking for all read operations for performance and to avoid recursion/circular access
            var query = _dbContext.Rooms.AsNoTracking()
                .Include(i => i.BuildingDbM)
                .Where(i => i.RoomId == id);

            return new ResponseItemDto<IRoom>()
            {
#if DEBUG
                ConnectionString = _dbContext.dbConnection,
#endif
                Item = await query.FirstOrDefaultAsync<IRoom>()
            };
        }
        else
        {
            //Not fully populated, compare the SQL Statements generated
            //remove tracking for all read operations for performance and to avoid recursion/circular access
            var query = _dbContext.Rooms.AsNoTracking()
                .Where(i => i.RoomId == id);

            return new ResponseItemDto<IRoom>()
            {
#if DEBUG
                ConnectionString = _dbContext.dbConnection,
#endif
                Item = await query.FirstOrDefaultAsync<IRoom>()
            };
        }
    }

    public async Task<ResponsePageDto<IRoom>> ReadRoomsAsync(bool flat, string filter, int pageNumber, int pageSize)
    {
        filter ??= "";
        IQueryable<RoomDbM> query;
        if (flat)
        {
            query = _dbContext.Rooms.AsNoTracking();
        }
        else
        {
            query = _dbContext.Rooms.AsNoTracking()
                .Include(i => i.BuildingDbM);
        }

        var ret = new ResponsePageDto<IRoom>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            DbItemsCount = await query

            //Adding filter functionality
            .Where(i => i.RoomName.ToLower().Contains(filter) ||
                            i.RoomLevel.ToString().Contains(filter)
                            ).CountAsync(),

            PageItems = await query

            //Adding filter functionality
            .Where(i => i.RoomName.ToLower().Contains(filter) ||
                            i.RoomLevel.ToString().Contains(filter))

            //Adding paging
            .Skip(pageNumber * pageSize)
            .Take(pageSize)

            .ToListAsync<IRoom>(),

            PageNr = pageNumber,
            PageSize = pageSize
        };
        return ret;
    }

    public async Task<ResponseItemDto<IRoom>> DeleteRoomAsync(int id)
    {


        var query1 = _dbContext.Rooms
            .Where(i => i.RoomId == id);

        var item = await query1.FirstOrDefaultAsync<RoomDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"Item {id} is not existing");

        //delete in the database model
        _dbContext.Rooms.Remove(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();
        return new ResponseItemDto<IRoom>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

    public async Task<ResponseItemDto<IRoom>> UpdateRoomAsync(RoomCuDto itemDto)
    {
        var query1 = _dbContext.Rooms
            .Where(i => i.RoomId == itemDto.RoomId);
        var item = await query1
                .Include(i => i.BuildingDbM)
                .FirstOrDefaultAsync<RoomDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"Item {itemDto.RoomId} is not existing");

        //transfer any changes from DTO to database objects
        //Update individual properties
        item.UpdateFromDTO(itemDto);

        //Update navigation properties
        await navProp_RoomCUdto_to_RoomDbM(itemDto, item);

        //write to database model
        _dbContext.Rooms.Update(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadRoomAsync(item.RoomId, false);
    }

    public async Task<ResponseItemDto<IRoom>> CreateRoomAsync(RoomCuDto itemDto)
    {
        if (itemDto.RoomId != null)
            throw new ArgumentException($"{nameof(itemDto.RoomId)} must be null when creating a new object");

        //transfer any changes from DTO to database objects
        //Update individual properties
        var item = new RoomDbM(itemDto);

        //Update navigation properties
        await navProp_RoomCUdto_to_RoomDbM(itemDto, item);

        //write to database model
        _dbContext.Rooms.Add(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadRoomAsync(item.RoomId, false);
    }

    private async Task navProp_RoomCUdto_to_RoomDbM(RoomCuDto itemDtoSrc, RoomDbM itemDst)
    {
        //update owner, i.e. navigation property BuildingDbM
        var owner = await _dbContext.Buildings.FirstOrDefaultAsync(
            a => (a.BuildingId == itemDtoSrc.BuildingId));

        if (owner == null)
            throw new ArgumentException($"Item id {itemDtoSrc.BuildingId} not existing");

        itemDst.BuildingDbM = owner;
    }
}
