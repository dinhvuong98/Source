using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity.Common
{
    [Table("bys_commune", Schema = "bys_sc_common")]
    public class Commune
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("District")]
        [Column("district_id")]
        public int DistrictId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("commune_code")]
        public string CommuneCode { get; set; }

        [Column("latitude")]
        public decimal Latitude { get; set; }

        [Column("longitude")]
        public decimal Longitude { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("modified_at")]
        public DateTime ModifiedAt { get; set; }

        public virtual District District { get; set; }
    }
}
