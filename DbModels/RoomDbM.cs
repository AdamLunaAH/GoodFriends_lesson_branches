using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;
using Models.DTO;

namespace DbModels;

[Table("Rooms", Schema = "supusr")]
[Index(nameof(RoomName), nameof(RoomLevel))]
[Index(nameof(RoomLevel), nameof(RoomName))]
sealed public class RoomDbM : Room
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override int RoomId { get; set; }

    [Required]
    public override string RoomName { get; set; }

    [Required]
    public override int RoomLevel { get; set; }

    [JsonIgnore]
    public int? BuildingId { get; set; }

    #region implementing entity Navigation properties when model is using interfaces in the relationships between models
    [NotMapped]
    public override IBuilding Building { get => BuildingDbM; set => new NotImplementedException(); }

    [JsonIgnore]
    [ForeignKey("BuildingId")]
    public BuildingDbM BuildingDbM { get; set; } = null!;

    #endregion



    #region Update from DTO
    public RoomDbM UpdateFromDTO(RoomCuDto org)
    {
        RoomName = org.RoomName;
        RoomLevel = org.RoomLevel;
        BuildingId = org.BuildingId;
        return this;
    }
    #endregion

    #region constructors
    public RoomDbM() { }
    public RoomDbM(RoomCuDto org) => UpdateFromDTO(org);
    #endregion
}

