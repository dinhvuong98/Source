using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Entity.Common
{
    [Table("bys_farming_location", Schema="bys_main")]
    public class FarmingLocation
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("area_id")]
        [ForeignKey("Areas")]
        public Guid? AreaId { get; set; }

        [Column("area")]
        public Decimal? Area { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("attachment")]
        public string Attachment { get; set; }

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

        public virtual Areas Areas { get; set; }

        [InverseProperty("FarmingLocation")]
        public ICollection<ShrimpCrop> ShrimpCrops { get; set; }

    }
}
