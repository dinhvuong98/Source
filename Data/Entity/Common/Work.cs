using Data.Entity.Account;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Enums;

namespace Data.Entity.Common
{
    [Table("bys_work", Schema = "bys_main")]
    public class Work
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("execution_time")]
        public DateTime ExecutionTime { get; set; }

        [ForeignKey("ShrimpCropManagementFactor")]
        [Column("shrimp_crop_management_factor_id")]
        public Guid ShrimpCropManagementFactorId { get; set; }

        [Column("value")]
        public string Value { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("created_by")]
        public Guid? CreatedBy { get; set; }

        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }

        [Column("modified_by")]
        public Guid? ModifiedBy { get; set; }

        [ForeignKey("FarmingLocation")]
        [Column("farming_location_id")]
        public Guid? FarmingLocationId { get; set; }

        [ForeignKey("ShrimpBreed")]
        [Column("shrimp_breed_id")]
        public Guid? ShrimpBreedId { get; set; }

        [ForeignKey("User")]
        [Column("curator")]
        public Guid? Curator { get; set; }

        [Column("status")]
        public string Status { get; set; } = EntityStatus.Alive.ToString();

        public virtual User User { get; set; }

        public virtual FarmingLocation FarmingLocation { get; set; }

        public virtual ShrimpBreed ShrimpBreed { get; set; }

        public virtual ShrimpCropManagementFactor ShrimpCropManagementFactor { get; set; }

        [InverseProperty("Work")]
        public virtual ICollection<WorkAudit> WorkAudits { get; set; }

        [InverseProperty("Work")]
        public virtual ICollection<WorkPicture> WorkPictures { get; set; }
    }
}
