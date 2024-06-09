using Common.DTOModels.Admin;
using System.ComponentModel.DataAnnotations;

namespace Common.DTOModels
{
    public class UserDTO
    {
        [Required]
        [Display(Name = "User Id")]
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Is Admin")]
        public bool IsAdmin { get; set; }

        public ButtonDTO ButtonDTO { get { return new ButtonDTO(Id); } }
        public TokenDTO Token { get; set; }
    }
}
