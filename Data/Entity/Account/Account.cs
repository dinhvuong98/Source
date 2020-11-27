using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Entity.Account
{
    [Table("bys_account", Schema = "bys_sc_account")]
    public class Account
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("User")]
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("username")]
        public string UserName { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }

        [Column("token")]
        public string Token { get; set; }

        [Column("token_expired_time")]
        public DateTime? TokenExpiredTime { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("created_by")]
        public Guid? CreatedBy { get; set; }

        [Column("updated_by")]
        public Guid? UpdateBy { get; set; }

        public virtual User User { get; set; }
    }
}
