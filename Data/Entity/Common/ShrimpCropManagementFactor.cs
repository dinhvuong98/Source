using Data.Entity.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utilities.Enums;

namespace Data.Entity.Common
{
    [Table("bys_shrimp_crop_management_factor", Schema = "bys_main")]
    public  class ShrimpCropManagementFactor
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [ForeignKey("ShrimpCrop")]
        [Column("shrimp_crop_id")]
        public Guid? ShrimpCropId { get; set; }

        [ForeignKey("ManagementFactor")]
        [Column("management_factor_id")]
        public Guid? ManagementFactorId { get; set; }

        [ForeignKey("User")]
        [Column("curator")]
        public Guid? Curator { get; set; }

        [Column("frequency")]
        public string Frequency { get; set; }

        [Column("execution_time")]
        public DateTime? ExecutionTime { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("created_by")]
        public Guid? CreatedBy { get; set; }

        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }

        [Column("modified_by")]
        public Guid? ModifiedBy { get; set; }

        [Column("from_date")]
        public DateTime? FromDate { get; set; }

        [Column("to_date")]
        public DateTime? ToDate { get; set; }

        [Column("status")]
        public string Status { get; set; } = CropFactorStatus.New.ToString();

        public virtual ManagementFactor ManagementFactor { get; set; }

        public virtual ShrimpCrop ShrimpCrop { get; set; }

        public virtual User User { get; set; }

        [InverseProperty("ShrimpCropManagementFactor")]
        public virtual ICollection<Work> Works { get; set; }
    }
}
