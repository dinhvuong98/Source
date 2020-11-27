using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity.Common
{
    [Table("bys_area", Schema = "bys_main")]
    public class Areas
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("name")]
        public string Name { get; set; }

        public decimal? Area { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("created_by")]
        public Guid? CreatedBy { get; set; }

        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }

        [Column("modified_by")]
        public Guid? ModifiedBy { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [InverseProperty("Area")]
        public ICollection<FarmingLocation> FarmingLocation { get; set; }
    }
}
