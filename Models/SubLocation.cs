using DocumentFormat.OpenXml.Spreadsheet;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAIS.Models
{
    public class SubLocation
    {
        [Key]
        public int SubLocationID { get; set; }

        [Required]
        [StringLength(20)]
        public string? SubLocationCode { get; set; }

        [Required]
        [StringLength(100)]
        public string? SubLocationName { get; set; }

        [ForeignKey("Location")]
        public int LocationID { get; set; }
        public Location? Location { get; set; }

        public ICollection<Village>? Villages { get; set; }
    }
}
