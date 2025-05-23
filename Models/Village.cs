using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAIS.Models
{
    public class Village
    {
        [Key]
        public int VillageID { get; set; }

        [Required]
        [StringLength(20)]
        public string? VillageCode { get; set; }

        [Required]
        [StringLength(100)]
        public string? VillageName { get; set; }

        [ForeignKey("SubLocation")]
        public int SubLocationID { get; set; }
        public SubLocation? SubLocation { get; set; }

        public ICollection<Applicant>? Applicants { get; set; }
    }
}
