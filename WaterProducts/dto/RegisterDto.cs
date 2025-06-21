using System.ComponentModel.DataAnnotations;

namespace WaterProducts.dto
{
    public class RegisterDto
    {
        [Required]
        [DataType(DataType.Text)]
        public string userName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}
