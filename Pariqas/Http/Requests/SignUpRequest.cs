using System.ComponentModel.DataAnnotations;

namespace Pariqas.Http.Requests;

public sealed class SignUpRequest
{
    [DataType(DataType.EmailAddress)]
    [Required]
    public string Email { get; set; }
    
    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; }
    
    [Compare(nameof(Password))]
    [DataType(DataType.Password)]
    [Required]
    public string ConfirmPassword { get; set; }
}