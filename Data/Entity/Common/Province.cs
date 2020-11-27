using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity.Common
{
    [Table("bys_province", Schema = "bys_sc_common")]
    public class Province
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Country")]
        [Column("country_id")]
        public int CountryId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("province_code")]
        public string ProvinceCode { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("modified_at")]
        public DateTime ModifiedAt { get; set; }

        public virtual Country Country { get; set; }
        
        [InverseProperty("Province")]
        public virtual ICollection<District> Districts { get; set; }
    }
}
