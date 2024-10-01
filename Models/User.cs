using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Shopping_API.Models;

// [Index(nameof(email), IsUnique = true)]
// [Index(nameof(userName), IsUnique = true)]
public class User
{
    public int Id { get; set; }
    
    public string fullName { get; set; }

    public string userName { get; set; }
    
    [JsonIgnore]
    public string password { get; set; }

    public string[] roles { get; set; }
    
    public string email { get; set; }
    
    public string address { get; set; }
    
    public string phoneNumber { get; set; }
    
}