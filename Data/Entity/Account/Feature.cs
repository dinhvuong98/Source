using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity.Account
{
    [Table("bys_feature", Schema = "bys_sc_account")]
    public class Feature
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("modified_at")]
        public DateTime ModifiedAt { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [InverseProperty("Feature")]
        public virtual ICollection<GroupFeature> GroupFeatures { get; set; }
    }
}
