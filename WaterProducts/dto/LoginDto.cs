using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;

namespace WaterProducts.dto
{
    public class LoginDto
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        public String Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public String Password { get; set; }
    }
}
