using System.ComponentModel.DataAnnotations;

namespace Pariqas.Http.Requests;

public sealed class LoginRequest
{
    public LoginRequest()
    {
        
    }

    public LoginRequest(string email, string password)
    {
        Email = email;
        Password = password;
    }
    
    [DataType(DataType.EmailAddress)]
    [Required]
    public string Email { get; set; }
    
    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; }
}