using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstatePipeline.Model
{
    public class Client_Info
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InquiryID { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        public string InquiryDetails { get; set; } 
    }
}
