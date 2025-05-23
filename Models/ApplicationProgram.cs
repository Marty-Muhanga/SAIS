using System.ComponentModel.DataAnnotations;

namespace SAIS.Models
{
    public class Program_
    {
        [Key]
        public int ProgramID { get; set; }

        [Required]
        [StringLength(20)]
        public string? ProgramCode { get; set; }

        [Required]
        [StringLength(100)]
        public string? ProgramName { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public ICollection<ApplicantProgram>? ApplicantPrograms { get; set; }
    }
}
