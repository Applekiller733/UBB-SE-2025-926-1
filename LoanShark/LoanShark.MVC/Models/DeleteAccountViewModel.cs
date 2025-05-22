using System.ComponentModel.DataAnnotations;

namespace LoanShark.MVC.Models
{
    public class DeleteAccountViewModel
    {
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }
    }
}
