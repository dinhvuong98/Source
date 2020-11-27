using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity.Common
{
    [Table("bys_shrimp_breed", Schema = "bys_main")]
    public class ShrimpBreed
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("name")]
        public string Name { get; set; }

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

        [InverseProperty("ShrimpBreed")]
        public virtual ICollection<ShrimpCrop> ShrimpCrop { get; set; }
    }
}