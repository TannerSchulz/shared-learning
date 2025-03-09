using FLP.Client.Services.UserAccount;
using FLP.Data;
using FLP.Data.Entities;
using FLP.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FLP.Services.UserAccount
{
    public class ServerUserAccountService(SignInManager<ApplicationUser> _signInManager, UserManager<ApplicationUser> _userManager, IConfiguration _configuration) : IUserAccountService
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

        public async Task<Status> Login(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.email);
            if(user == null)
            {
                return new Status { Code = "401", Message = "Invalid Username or Password" };
            }
            try
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.password, false, false);
                if (!result.Succeeded)
                {
                    return new Status { Code = "401", Message = "Invalid Username or Password" };
                }
                var token = GenerateJwtToken(user);
                return new Status { Code = "200", Message = token };
            }
            catch(Exception ex)
            {
                return new Status { Code = "400", Message = "Login Failure" };
            }
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
