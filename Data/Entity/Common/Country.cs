using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity.Common
{
    [Table("bys_country", Schema = "bys_sc_common")]
    public class Country
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("country_code")]
        public string CountryCode { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("modified_at")]
        public DateTime ModifiedAt { get; set; }

        [InverseProperty("Country")]
        public virtual ICollection<Province> Provinces { get; set; }
    }
}
