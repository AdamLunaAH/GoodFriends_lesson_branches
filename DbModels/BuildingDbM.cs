using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;
using Models.DTO;

namespace DbModels;

[Table("Buildings", Schema = "sql-roomfinder")]
[Index(nameof(BuildingName), nameof(BuildingNumber))]
[Index(nameof(BuildingNumber), nameof(BuildingName))]
sealed public class BuildingDbM : Building
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override int BuildingId { get; set; }

    [Required]
    public override string BuildingName { get; set; }

    [Required]
    public override int BuildingNumber { get; set; }




    #region implementing entity Navigation properties when model is using interfaces in the relationships between models

    [NotMapped]
    public override List<IRoom> Rooms => RoomDbM?.ToList<IRoom>() ?? new();

    [JsonIgnore]
    public List<RoomDbM> RoomDbM { get; set; } = new();

    #endregion



    #region Update from DTO
    public BuildingDbM UpdateFromDTO(BuildingCuDto org)
    {
        BuildingName = org.BuildingName;
        BuildingNumber = org.BuildingNumber;
        return this;
    }
    #endregion

    #region constructors
    public BuildingDbM() { }
    public BuildingDbM(BuildingCuDto org) => UpdateFromDTO(org);
    #endregion
}

