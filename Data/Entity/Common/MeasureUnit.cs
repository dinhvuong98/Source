using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity.Common
{
    [Table("bys_measure_unit", Schema = "bys_sc_common")]
    public class MeasureUnit
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }

        [Column("basic_unit_id")]
        public Guid? BasicUnitId { get; set; }

        [Column("convert_rate")]
        public float? ConvertRate { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Desciption { get; set; }

        [Column("category")]
        public string Category { get; set; }

        [InverseProperty("MeasureUnit")]
        public ICollection<ManagementFactor> ManagementFactor { get; set; }
    }
}
