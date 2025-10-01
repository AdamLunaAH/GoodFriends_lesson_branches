using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

using Models;
using Models.DTO;
using Services;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BuildingsController : Controller
    {
        readonly IBuildingsService _service = null;
        readonly ILogger<BuildingsController> _logger = null;

        [HttpGet()]
        [ActionName("Read")]
        [ProducesResponseType(200, Type = typeof(ResponsePageDto<IBuilding>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Read(string flat = "true",
            string filter = null, string pageNr = "0", string pageSize = "10")
        {
            try
            {
                bool flatArg = bool.Parse(flat);
                int pageNrArg = int.Parse(pageNr);
                int pageSizeArg = int.Parse(pageSize);

                _logger.LogInformation($"{nameof(Read)}: {nameof(flatArg)}: {flatArg}, " +
                    $"{nameof(pageNrArg)}: {pageNrArg}, {nameof(pageSizeArg)}: {pageSizeArg}");

                var resp = await _service.ReadBuildingsAsync(flatArg, filter?.Trim().ToLower(), pageNrArg, pageSizeArg);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Read)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        //GET: api/Buildings/readitem
        [HttpGet()]
        [ActionName("ReadItem")]
        [ProducesResponseType(200, Type = typeof(IBuilding))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> ReadItem(string id, string flat = "false")
        {
            try
            {
                if (!int.TryParse(id, out int idArg))
                    throw new ArgumentException($"Invalid id: {id}");
                bool flatArg = bool.Parse(flat);

                _logger.LogInformation($"{nameof(ReadItem)}: {nameof(idArg)}: {idArg}, {nameof(flatArg)}: {flatArg}");

                var item = await _service.ReadBuildingAsync(idArg, flatArg);
                if (item == null) throw new ArgumentException($"Item with id {id} does not exist");

                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadItem)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        //DELETE: api/Buildings/deleteitem/id
        [HttpDelete("{id}")]
        [ActionName("DeleteItem")]
        [ProducesResponseType(200, Type = typeof(IBuilding))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> DeleteItem(string id)
        {
            try
            {
                if (!int.TryParse(id, out int idArg))
                    throw new ArgumentException($"Invalid id: {id}");

                _logger.LogInformation($"{nameof(DeleteItem)}: {nameof(idArg)}: {idArg}");

                var item = await _service.DeleteBuildingAsync(idArg);
                if (item == null) throw new ArgumentException($"Item with id {id} does not exist");

                _logger.LogInformation($"item {idArg} deleted");
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(DeleteItem)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        //GET: api/Buildings/readitemdto
        [HttpGet()]
        [ActionName("ReadItemDto")]
        [ProducesResponseType(200, Type = typeof(BuildingCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> ReadItemDto(string id = null)
        {
            try
            {
                if (!int.TryParse(id, out int idArg))
                    throw new ArgumentException($"Invalid id: {id}");

                _logger.LogInformation($"{nameof(ReadItemDto)}: {nameof(idArg)}: {idArg}");

                var item = await _service.ReadBuildingAsync(idArg, false);
                if (item == null) throw new ArgumentException($"Item with id {id} does not exist");

                return Ok(
                    new ResponseItemDto<BuildingCuDto>()
                    {
#if DEBUG
                        ConnectionString = item.ConnectionString,
#endif
                        Item = new BuildingCuDto(item.Item)
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadItemDto)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        //PUT: api/Buildings/updateitem/id
        //Body: csBuildingCUdto in Json
        [HttpPut("{id}")]
        [ActionName("UpdateItem")]
        [ProducesResponseType(200, Type = typeof(IBuilding))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateItem(string id, [FromBody] BuildingCuDto item)
        {
            try
            {
                if (!int.TryParse(id, out int idArg))
                    throw new ArgumentException($"Invalid id: {id}");

                _logger.LogInformation($"{nameof(UpdateItem)}: {nameof(idArg)}: {idArg}");

                if (item.BuildingId != idArg) throw new ArgumentException("Id mismatch");

                var _item = await _service.UpdateBuildingAsync(item);
                _logger.LogInformation($"item {idArg} updated");

                return Ok(_item);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(UpdateItem)}: {ex.Message}");
                return BadRequest($"Could not update. Error {ex.Message}");
            }
        }

        //POST: api/Buildings/createitem
        //Body: csBuildingCUdto in Json
        [HttpPost()]
        [ActionName("CreateItem")]
        [ProducesResponseType(200, Type = typeof(IBuilding))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateItem([FromBody] BuildingCuDto item)
        {
            try
            {
                _logger.LogInformation($"{nameof(CreateItem)}:");

                var _item = await _service.CreateBuildingAsync(item);
                _logger.LogInformation($"item {_item.Item.BuildingId} created");

                return Ok(_item);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(CreateItem)}: {ex.Message}");
                return BadRequest($"Could not create. Error {ex.Message}");
            }
        }

        public BuildingsController(IBuildingsService service, ILogger<BuildingsController> logger)
        {
            _service = service;
            _logger = logger;
        }
    }
}

