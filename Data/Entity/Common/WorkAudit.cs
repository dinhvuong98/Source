using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Enums;

namespace Data.Entity.Common
{
    [Table("bys_work_audit", Schema = "bys_main")]
    public class WorkAudit
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("work_id")]
        [ForeignKey("Work")]
        public Guid WorkId { get; set; }

        [Column("field_name")]
        public string FieldName { get; set; }

        [Column("old_value")]
        public string OldValue { get; set; }

        [Column("new_value")]
        public string NewValue { get; set; }

        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }

        [Column("modified_by")]
        public Guid? ModifiedBy { get; set; }

        [Column("status")]
        public string Status { get; set; } = EntityStatus.Alive.ToString();

        public virtual Work Work { get; set; }
    }
}
