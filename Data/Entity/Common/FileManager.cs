using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity.Common
{
    [Table("bys_file_manager", Schema = "bys_sc_common")]
    public class FileManager
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("org_file_name")]
        public string OrgFileName { get; set; }

        [Column("org_file_extension")]
        public string OrgFileExtension { get; set; }

        [Column("create_at")]
        public DateTime CreateAt { get; set; }
    }
}
