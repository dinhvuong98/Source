using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Enums;

namespace Data.Entity.Account
{
    [Table("bys_users_groups", Schema = "bys_sc_account")]
    public class UserGroup
    {
        [Key]
        [Column("user_id")]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Key]
        [Column("group_id")]
        [ForeignKey("Group")]
        public Guid GroupId { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }

        [Column("status")]
        public string Status { get; set; } = EntityStatus.Alive.ToString();

        public virtual User User { get; set; }

        public virtual Group Group { get; set; }
    }
}
