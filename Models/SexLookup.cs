using System.ComponentModel.DataAnnotations;

namespace SAIS.Models
{
    public class SexLookup
    {
        [Key]
        public int SexID { get; set; }

        [Required]
        [StringLength(50)]
        public string? SexName { get; set; }

        [Required]
        [StringLength(10)]
        public string? SexCode { get; set; }
    }
}
