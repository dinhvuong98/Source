using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Enums;

namespace Data.Entity.Account
{
    [Table("bys_groups_features", Schema = "bys_sc_account")]
    public class GroupFeature
    {
        [Key]
        [Column("group_id")]
        [ForeignKey("Group")]
        public Guid GroupId { get; set; }

        [Key]
        [Column("feature_id")]
        [ForeignKey("Feature")]
        public Guid FeatureId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("modified_at")]
        public DateTime ModifiedAt { get; set; }

        [Column("status")]
        public string Status { get; set; } = EntityStatus.Alive.ToString();

        public virtual Feature Feature { get; set; }

        public virtual Group Group { get; set; }
    }
}
