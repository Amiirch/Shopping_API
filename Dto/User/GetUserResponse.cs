namespace Shopping_API.Extensions.Dto.User;

public class GetUserResponse
{
    public GetUserResponse(int id,string fullName,string userName,string[]roles,string email,string address,string phoneNumber)
    {
        Id = id;
        FullName = fullName;
        UserName = userName;
        Roles = roles;
        Email = email;
        Address = address;
        PhoneNumber = phoneNumber;
    }
    public int Id { get; set; }
    
    public string FullName { get; set; }

    public string UserName { get; set; }

    public string[] Roles { get; set; }
    
    public string Email { get; set; }
    
    public string Address { get; set; }
    
    public string PhoneNumber { get; set; }
}