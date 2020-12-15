using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Entity.Account
{
    [Table("bys_user", Schema = "bys_sc_account")]
    public class User
    {
        [Column("id")]
        [Key]
        public Guid Id { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }

        [Column("fullname")]
        public string FullName { get; set; }

        [Column("avatar")]
        public string Avatar { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("last_time_read_notification")]
        public DateTime? LastTimeReadNotification { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("district_id")]
        public int? DistrictId { get; set; }

        [Column("commune_id")]
        public int? CommuneId { get; set; }

        [Column("province_id")]
        public int? ProvinceId { get; set; }

        [Column("created_by")]
        public Guid? CreatedBy { get; set; }

        [Column("updated_by")]
        public Guid? UpdateBy { get; set; }

        [InverseProperty("User")]
        public ICollection<Account> Account { get; set; }
    }
}
