using System.Security.Claims;
using System.Linq;
using IdentityModel;
using IdentityService;
using IdentityService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TempApp.Pages;

namespace Temp.Pages.Register;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly UserManager<User> _userManager;

    public Index(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [BindProperty]
    public RegisterViewModel Input { get; set; }

    [BindProperty]
    public bool RegisterSuccess { get; set; }

    [BindProperty]
    public string Button { get; set; }
    public IActionResult OnGet(string returnUrl)
    {
        Input = new RegisterViewModel
        {
            ReturnUrl = returnUrl
        };
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if(Button != "Register")
        {
            return Redirect("~/");
        }

        if(!ModelState.IsValid)
        {
            return Page();
        }

        var user = new User
        {
            UserName = Input.UserName,
            Email = Input.Email,
            Name = Input.FullName,
            EmailConfirmed = true,
        };

        var result = await _userManager.CreateAsync(user, Input.Password);

        if(result.Succeeded)
        {
            await _userManager.AddClaimsAsync(user, new Claim[] {
                new Claim(JwtClaimTypes.Name, Input.FullName),
            });
            RegisterSuccess = true;
        }
        else
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return Page();
    }
}