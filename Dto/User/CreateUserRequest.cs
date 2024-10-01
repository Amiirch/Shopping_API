using System.ComponentModel.DataAnnotations;

namespace Shopping_API.Dto.User;

public class CreateUserRequest
{
    [Required(ErrorMessage = "product name is required")]
    [StringLength(50, ErrorMessage = "product name must be between 1 and 50 characters", MinimumLength = 5)]
    public string FullName { get; set; }
    
    [Required(ErrorMessage = "username is required")]
    [StringLength(50, ErrorMessage = "user name must be between 5 and 50 characters", MinimumLength = 5)]
    public string UserName { get; set; }

    [Required(ErrorMessage = "password is required")]
    [StringLength(50, ErrorMessage = "password must be between 10 and 50 characters", MinimumLength = 10)]
    public string Password { get; set; }

    [Required(ErrorMessage = "role is required")]
    public string[] Roles { get; set; }
    
    [Required(ErrorMessage = "email is required")]
    [EmailAddress(ErrorMessage = "email is not valid")]
    public string Email { get; set; }
    
    public string Address { get; set; }
    
    [Required(ErrorMessage = "phoneNumber is required")]
    [Phone(ErrorMessage = "phonenumber is not valid ")]
    public string PhoneNumber { get; set; }
}