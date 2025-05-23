using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAIS.Models
{
    public class County
    {
        [Key]
        public int CountyID { get; set; }

        [Required]
        [StringLength(20)]
        public string? CountyCode { get; set; }

        [Required]
        [StringLength(100)]
        public string? CountyName { get; set; }

        public ICollection<SubCounty>? SubCounties { get; set; }
    }
}
