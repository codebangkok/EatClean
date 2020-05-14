using System.Collections.Generic;
using System.Threading.Tasks;
using EatClean.MobileAppService.Data;
using EatClean.MobileAppService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using EatClean.MobileAppService.Helpers;
using System.IO;
using Microsoft.Extensions.Options;

namespace EatClean.MobileAppService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserManager<ApplicationUser> userMan;
        SignInManager<ApplicationUser> signinMan;
        ApplicationDbContext db;
        AzureStorageConfig storageConfig;
        IConfiguration config;

        public UserController(UserManager<ApplicationUser> userMan, SignInManager<ApplicationUser> signinMan, ApplicationDbContext db, IOptions<AzureStorageConfig> configOption, IConfiguration config)
        {
            this.userMan = userMan;
            this.signinMan = signinMan;
            this.db = db;
            this.storageConfig = configOption.Value;
            this.config = config;
        }

        [HttpGet]
        public async Task<List<ApplicationUser>> GetAll()
        {
            return await userMan.Users.ToListAsync();
        }

        [HttpGet]
        public async Task<ApplicationUser> GetById(string id)
        {
            return await userMan.FindByIdAsync(id);
        }

        [HttpGet]
        public async Task<ApplicationUser> GetByUsername(string username)
        {
            return await userMan.FindByNameAsync(username);
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> Register([FromForm]string username, [FromForm]string password)
        {
            try
            {
                var user = new ApplicationUser { UserName = username, Name = username, Photo = "person.png" };
                var result = await userMan.CreateAsync(user, password);
                if (result != IdentityResult.Success) return BadRequest(result.ToString());
                
                var signInResult = await signinMan.PasswordSignInAsync(user, password, false, true);
                if (signInResult != Microsoft.AspNetCore.Identity.SignInResult.Success) return BadRequest(signInResult.ToString());

                //return GetToken(user);
                return user;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<string>> Update([FromForm]string userId, [FromForm]string name, [FromForm]IFormFile file)
        {
            try
            {
                var user = await userMan.FindByIdAsync(userId);
                if (user == null) return BadRequest("Incorrect Username or Password");

                user.Name = name;
                user.Photo = "person.png";

                if (file != null && file.Length > 0)
                {
                    var index = file.FileName.LastIndexOf('.');
                    var fileName = $"{Guid.NewGuid()}{file.FileName.Substring(index)}";
                    
                    using (var stream = file.OpenReadStream())
                    {
                        if (await StorageHelper.UploadFileToStorage(stream, fileName, storageConfig, "profile"))
                        {
                            user.Photo = fileName;
                        }
                    }
                }                
                
                await userMan.UpdateAsync(user);
                return user.Photo;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> Login([FromForm]string username, [FromForm]string password)
        {
            try
            {
                var user = await userMan.FindByNameAsync(username);
                if (user == null) return BadRequest("Incorrect Username or Password");
                
                var signInResult = await signinMan.PasswordSignInAsync(user, password, false, true);
                if (signInResult != Microsoft.AspNetCore.Identity.SignInResult.Success) return BadRequest(signInResult.ToString());

                //return GetToken(user);
                return user;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GetToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),                
                new Claim("UserId", user.Id),
                new Claim("Name", user.Name),
                new Claim("Photo", user.Photo)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddSeconds(Convert.ToDouble(config["Jwt:ExpireSeconds"]));

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}