using Data.Entity.Account;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Enums;

namespace Data.Entity.Common
{
    [Table("bys_notification", Schema = "bys_main")]
    public class Notification
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("factor_name")]
        public string FactorName { get; set; }

        [Column("farming_location_name")]
        public string FarmingLocationName { get; set; }

        [Column("execution_time")]
        public DateTime? ExecutionTime { get; set; }

        [Column("shrimp_crop_name")]
        public string ShrimpCropName { get; set; }

        [Column("from_date")]
        public DateTime? FromDate { get; set; }

        [Column("to_date")]
        public DateTime? ToDate { get; set; }

        [ForeignKey("User")]
        [Column("user_id")]
        public Guid? UserId { get; set; }

        [Column("status")]
        public string Status { get; set; } = NotificationStatus.New.ToString();

        [Column("frequency")]
        public string Frequency { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [ForeignKey("Work")]
        [Column("work_id")]
        public Guid? WorkId { get; set; }

        [ForeignKey("ManagementFactor")]
        [Column("management_factor_id")]
        public Guid? ManagementFactorId { get; set; }

        [ForeignKey("FarmingLocation")]
        [Column("farming_location_id")]
        public Guid? FarmingLocationId { get; set; }

        [ForeignKey("ShrimpCrop")]
        [Column("shrimp_crop_id")]
        public Guid? ShrimpCropId { get; set; }

        [ForeignKey("ShrimpCropManagementFactor")]
        [Column("shrimp_crop_management_factor_id")]

        public Guid? ShrimpCropManagementFactorId { get; set; }
        public virtual Work Work { get; set; }
        public virtual ManagementFactor ManagementFactor { get; set; }
        public virtual FarmingLocation FarmingLocation { get; set; }
        public virtual ShrimpCrop ShrimpCrop { get; set; }
        public virtual ShrimpCropManagementFactor ShrimpCropManagementFactor { get; set; }
        public virtual User User { get; set; }
    }
}
