using System.ComponentModel.DataAnnotations;

namespace SAIS.Models
{
    public class ApplicationStatusLookup
    {
        [Key]
        public int StatusID { get; set; }

        [Required]
        [StringLength(50)]
        public string? StatusName { get; set; }

        [Required]
        [StringLength(10)]
        public string? StatusCode { get; set; }
    }
}
