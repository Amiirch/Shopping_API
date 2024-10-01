using System.ComponentModel.DataAnnotations;

namespace Shopping_API.Extensions.Dto.User;

public class UpdateUserRequest
{
    [Required]
    public int Id { get; set; }
    
    [Required(ErrorMessage = " name is required")]
    [StringLength(50, ErrorMessage = " name must be between 1 and 50 characters", MinimumLength = 5)]
    public string FullName { get; set; }
    
    [Required(ErrorMessage = "username is required")]
    [StringLength(50, ErrorMessage = "username must be between 5 and 50 characters", MinimumLength = 5)]
    public string UserName { get; set; }
    
    [Required(ErrorMessage = "email is required")]
    [EmailAddress(ErrorMessage = "email is not valid")]
    public string Email { get; set; }
    
    public string Address { get; set; }
    
    [Required(ErrorMessage = "phoneNumber is required")]
    [Phone(ErrorMessage = "phonenumber is not valid ")]
    public string PhoneNumber { get; set; }
}