using System.ComponentModel.DataAnnotations;

namespace Model.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        public string Name { get; set; }

    }
}
