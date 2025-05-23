using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAIS.Models
{
    public class Location
    {
        [Key]
        public int LocationID { get; set; }

        [Required]
        [StringLength(20)]
        public string? LocationCode { get; set; }

        [Required]
        [StringLength(100)]
        public string? LocationName { get; set; }

        [ForeignKey("SubCounty")]
        public int SubCountyID { get; set; }
        public SubCounty? SubCounty { get; set; }

        public ICollection<SubLocation>? SubLocations { get; set; }
    }
}
