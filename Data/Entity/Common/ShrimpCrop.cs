using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Enums;

namespace Data.Entity.Common
{
    [Table("bys_shrimp_crop", Schema = "bys_main")]
    public class ShrimpCrop
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("from_date")]
        public DateTime? FromDate { get; set; }

        [Column("to_date")]
        public DateTime? ToDate { get; set; }

        [ForeignKey("FarmingLocation")]
        [Column("farming_location_id")]
        public Guid? FarmingLocationId { get; set; }

        [ForeignKey("ShrimpBreed")]
        [Column("shrimp_breed_id")]
        public Guid? ShrimpBreedId { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("created_by")]
        public Guid? CreatedBy { get; set; }

        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }

        [Column("modified_by")]
        public Guid? ModifiedBy { get; set; }

        [Column("status")]
        public string Status { get; set; } = EntityStatus.Alive.ToString();

        [InverseProperty("ShrimpCrop")]
        public virtual ICollection<ShrimpCropManagementFactor> ShrimpCropManagementFactors { get; set; }

        public virtual FarmingLocation FarmingLocation { get; set; }

        public virtual ShrimpBreed ShrimpBreed { get; set; }
    }
}
