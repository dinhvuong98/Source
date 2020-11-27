using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Entity.Common
{
    [Table("bys_master_data", Schema = "bys_sc_common")]
    public class MasterData
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("key")]
        public string Key { get; set; }

        [Column("value")]
        public string Value { get; set; }

        [Column("group_name")]
        public string GroupName { get; set; }

        [Column("text")]
        public string Text { get; set; }
    }   
}
