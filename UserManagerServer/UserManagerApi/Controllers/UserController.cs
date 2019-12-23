using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using IdentityData.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserManagerCore.Dtos;
using UserManagerCore.Dtos.User;

namespace UserManagerApi.Controllers
{
    [Authorize("isAdmin")]
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserController(
            ILogger<UserController> logger,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserForCreate model)
        {
            var email = model.Email;

            _logger.LogInformation("Create new user, email: {email}", email);

            var existedUser = await _userManager.FindByEmailAsync(email);
            if (existedUser != null)
            {
                ModelState.AddModelError(nameof(model.Email), "Email already exist");

                return new UnprocessableEntityObjectResult(ModelState);
            }

            var roleFromDb = await _roleManager.FindByNameAsync(model.RoleName);
            if(roleFromDb == null)
            {
                ModelState.AddModelError(nameof(model.RoleName), "Unavailable role");

                return new UnprocessableEntityObjectResult(ModelState);
            }

            var userForCreate = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(userForCreate, model.Password);
            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return new UnprocessableEntityObjectResult(ModelState);
            }

            await _userManager.AddClaimsAsync(userForCreate, new Claim[]{
                new Claim(JwtClaimTypes.Role, model.RoleName),
                new Claim(JwtClaimTypes.Email, model.Email)
            });

            return Ok();
        }
    }
}