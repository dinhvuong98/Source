using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity.Common
{
    [Table("bys_numbering", Schema = "bys_sc_common")]
    public class Numbering
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("prefix")]
        public string Prefix { get; set; }

        [Column("length")]
        public decimal Length { get; set; }

        [Column("start")]
        public long Start { get; set; }

        [Column("description")]
        public string description { get; set; }

        [Column("prefix_have_year")]
        public bool PrefixHaveYear { get; set; }
    }
}
