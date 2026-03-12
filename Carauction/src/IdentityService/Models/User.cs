using Microsoft.AspNetCore.Identity;

namespace IdentityService.Models;

public class User : IdentityUser
{
    public string Name { get; set; }
    public string Role { get; set; }
}