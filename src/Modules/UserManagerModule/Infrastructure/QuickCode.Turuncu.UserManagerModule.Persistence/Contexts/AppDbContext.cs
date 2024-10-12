using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace QuickCode.Turuncu.UserManagerModule.Persistence.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : 
    IdentityDbContext<ApiUser>(options)
{
}


public class ApiUser : IdentityUser
{
    public string? FirstName { get; set; } 
    public string? LastName { get; set; } 
    public int? PermissionGroupId { get; set; }
}