using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity.Common
{
    [Table("bys_district", Schema = "bys_sc_common")]
    public class District
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Province")]
        [Column("province_id")]
        public int ProvinceId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("district_code")]
        public string DistrictCode { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("modified_at")]
        public DateTime ModifiedAt { get; set; }

        public virtual Province Province { get; set; }

        [InverseProperty("District")]
        public virtual ICollection<Commune> Communes { get; set; }
    }
}
