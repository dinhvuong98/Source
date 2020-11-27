using Data.Entity.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity.Common
{
    [Table("bys_management_factor", Schema = "bys_main")]
    public class ManagementFactor
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("data_type")]
        public string DataType { get; set; }

        [Column("sample_value")]
        public string SampleValue { get; set; }

        [Column("factor_group")]
        public string FactorGroup { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("created_by")]
        public Guid? CreatedBy { get; set; }

        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }

        [Column("modified_by")]
        public Guid? ModifiedBy { get; set; }

        [ForeignKey("MeasureUnit")]
        [Column("unit_id")]
        public Guid? UnitId { get; set; }

        [Column("status")]
        public string Status { get; set; }

        public virtual MeasureUnit MeasureUnit { get; set; }
    }
}
