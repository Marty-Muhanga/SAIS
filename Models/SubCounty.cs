using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAIS.Models
{
    public class SubCounty
    {
        [Key]
        public int SubCountyID { get; set; }

        [Required]
        [StringLength(20)]
        public string? SubCountyCode { get; set; }

        [Required]
        [StringLength(100)]
        public string? SubCountyName { get; set; }

        [ForeignKey("County")]
        public int CountyID { get; set; }
        public County? County { get; set; }

        public ICollection<Location>? Locations { get; set; }

    }
}
