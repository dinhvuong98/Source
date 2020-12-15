using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity.Common
{
    [Table("bys_temp_upload_file", Schema = "bys_sc_temp")]
    public class TempUploadFile
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("org_file_name")]
        public string OrgFileName { get; set; }

        [Column("org_file_extension")]
        public string OrgFileExtension { get; set; }

        [Column("file_url")]
        public string FileUrl { get; set; }

        [Column("container")]
        public string Container { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }
    }
}
