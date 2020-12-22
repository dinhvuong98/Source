using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Enums;

namespace Data.Entity.Account
{
    [Table("bys_group", Schema = "bys_sc_account")]
    public class Group
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("created_by")]
        public Guid CreatedBy { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }

        [Column("updated_by")]
        public Guid? UpdatedBy { get; set; }

        [Column("is_default")]
        public bool isDefault { get; set; }

        [Column("status")]
        public string Status { get; set; } = EntityStatus.Alive.ToString();

        [InverseProperty("Group")]
        public virtual ICollection<GroupFeature> GroupFeatures { get; set; }

        [InverseProperty("Group")]
        public virtual ICollection<UserGroup> UserGroups { get; set; }
    }
}
