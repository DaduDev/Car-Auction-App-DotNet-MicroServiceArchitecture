using System.Security.Claims;
using IdentityModel;
using IdentityService.Data;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityService;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();
        
        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        if(!userMgr.Users.Any())
        {
            Console.WriteLine("Seeding users");

            var bob = new User
            {
                UserName = "bob",
                Email = "bob@example.com",
                Name = "Bob",
                Role = "User"
            };
            var bobRes = userMgr.CreateAsync(bob, "Pass123$").Result;
            if (!bobRes.Succeeded)
            {
                throw new Exception(bobRes.Errors.First().Description);
            }
            userMgr.AddClaimsAsync(bob, new Claim[]
            {
               new Claim(JwtClaimTypes.Name, bob.Name),
               new Claim(JwtClaimTypes.Role, bob.Role) 
            }).Wait();

            Console.WriteLine("Bob User created");

            var tom = new User
            {
                UserName = "tom",
                Email = "tom@example.com",
                Name = "Tom",
                Role = "User"
            };

            var tomRes = userMgr.CreateAsync(tom, "Pass123$").Result;

            if(!tomRes.Succeeded)
            {
                throw new Exception(tomRes.Errors.First().Description);
            }

            userMgr.AddClaimsAsync(tom, new Claim[]
            {
               new Claim(JwtClaimTypes.Name, tom.Name),
               new Claim(JwtClaimTypes.Role, tom.Role) 
            }).Wait();

            Console.WriteLine("Tom User created");

            var alice = new User
            {
                UserName = "alice",
                Email = "alice@example.com",
                Name = "Alice",
                Role = "User"
            };

            var aliceRes = userMgr.CreateAsync(alice, "Pass123$").Result;

            if(!aliceRes.Succeeded)
            {
                throw new Exception(aliceRes.Errors.First().Description);
            }

            userMgr.AddClaimsAsync(alice, new Claim[]
            {
               new Claim(JwtClaimTypes.Name, alice.Name),
               new Claim(JwtClaimTypes.Role, alice.Role) 
            }).Wait();

            Console.WriteLine("Alice User created");
        } else
        {
            Console.WriteLine("Users already exists");
        }
        
    }
}