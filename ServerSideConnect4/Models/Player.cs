using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerSideConnect4.Models
{
    public class Player
    {
        [Key]
        [Required(ErrorMessage = "Please enter an ID number.")]
        [Range(1, 1000, ErrorMessage = "The ID must be between 1 and 1000.")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; } = default!;

        [Required(ErrorMessage = "Please enter the first name.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The first name must be at least 2 characters.")]
        public string FirstName { get; set; } = default!;

        [Required(ErrorMessage = "Please enter the cellphone number.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "insert a valid phone number")]
        public string Phone { get; set; } = default!;

        [Required(ErrorMessage = "Please enter the country.")]
        public string Country { get; set; } = default!;
    }
}
