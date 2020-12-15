using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Enums;

namespace Data.Entity.Common
{
    [Table("bys_work_picture", Schema = "bys_main")]
    public class WorkPicture
    {
        [Key]
        [Column("file_id")]
        public Guid FileId { get; set; }

        [ForeignKey("Work")]
        [Column("work_id")]
        public Guid WorkId { get; set; }

        [Column("org_file_name")]
        public string OrgFileName { get; set; }

        [Column("org_file_extension")]
        public string OrgFileExtension { get; set; }

        [Column("status")]
        public string status { get; set; } = PictureStatus.Alive.ToString();

        public virtual Work Work { get; set; }
    }
}
