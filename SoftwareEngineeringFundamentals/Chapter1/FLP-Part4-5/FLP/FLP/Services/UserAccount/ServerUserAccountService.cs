using FLP.Client.Services.UserAccount;
using FLP.Data;
using FLP.Data.Entities;
using FLP.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace FLP.Services.UserAccount
{
    public class ServerUserAccountService(SignInManager<ApplicationUser> _signInManager, UserManager<ApplicationUser> _userManager) : IUserAccountService
    {
        public async Task<Status> Register(RegisterModel model)
        {
            if(model.password != model.confirmPassword)
            {
                return new Status { Code = "400", Message = "Passwords do not match" };
            }
            var user = new ApplicationUser { UserName = model.email, Email = model.email };
            user.UserName = user.Email.Substring(0, user.Email.IndexOf('@'));
            var result = await _userManager.CreateAsync(user, model.password);
            if(!result.Succeeded)
            {
                var s = new Status { Code = "400" };
                foreach(var error in result.Errors)
                {
                    s.Message += error.Description + "\n";
                }
                return s;
            }
            return new Status { Code = "200", Message = $"Registration of {model.email} was Successful" };
        }
    }
}
