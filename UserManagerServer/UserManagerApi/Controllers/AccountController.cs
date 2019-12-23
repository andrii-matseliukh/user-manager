using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdentityData.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserManagerApi.Helper;
using UserManagerCore.Dtos;
using UserManagerCore.Dtos.Account;
using UserManagerCore.Entities;
using UserManagerCore.Repositories;
using UserManagerCore.Services.Interfaces;

namespace UserManagerApi.Controllers
{
    
    [Route("api/accounts")]
    [Authorize]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        private readonly JsonPatchManager _jsonPatchManager;
        private readonly UserManager<AppUser> _userManager;

        private readonly IAccountRepository _accountRepository;
        private readonly IAccountService _accountService;

        public AccountController(
            IMapper mapper,
            ILogger<AccountController> logger,
            JsonPatchManager jsonPatchManager,
            UserManager<AppUser> userManager,
            IAccountService accountService,
            IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _logger = logger;
            _jsonPatchManager = jsonPatchManager;
            _userManager = userManager;
            _accountService = accountService;
        }

        
        [HttpGet]
        [Authorize(Policy = "isAdmin")]
        public async Task<IActionResult> GetAccounts()
        {
            _logger.LogInformation("Get all accounts");
            var userclaims = User.Claims.ToList();
            var accountsFromDb = await _accountRepository.GetAllAsync();
            var accountForDisplays = _mapper.Map<List<AccountForDisplay>>(accountsFromDb);

            return Ok(accountForDisplays);
        }

        [HttpGet("personal", Name = "GetMyAccount")]
        public async Task<IActionResult> GetAccount()
        {
            var userEmail = User.Claims.FirstOrDefault(s => s.Type == JwtClaimTypes.Email).Value;

            _logger.LogInformation("Get account, with email: {email}", userEmail);

            var account = (await _accountRepository
                    .GetWhereAsync(s => s.Email == userEmail, lazyLoading: true)).SingleOrDefault();

            var userRole = User.Claims.FirstOrDefault(s => s.Type == JwtClaimTypes.Role).Value;

            if (account.Email != userEmail && userRole != "admin")
            {
                return Forbid();
            }

            var accountForDisplay = _mapper.Map<AccountForDisplay>(account);

            return Ok(accountForDisplay);
        }

        [HttpGet("{id}", Name = "GetAccount")]
        public async Task<IActionResult> GetAccount(int? id)
        {
            _logger.LogInformation("Get account, with id: {id}", id);

            var account = await _accountRepository.GetByIdAsync(id.Value, lazyLoading: true);

            if (account == null)
            {
                return BadRequest("No account found for this id");
            }

            var accountForDisplay = _mapper.Map<AccountForDisplay>(account);

            return Ok(accountForDisplay);
        }

        
        [HttpPost]
        [Authorize("isAdmin")]
        public async Task<IActionResult> CreateAccount(AccountForCreate model)
        {
            _logger.LogInformation("Create new account");

            var userFromDb = _userManager.FindByEmailAsync(model.Email);
            if(userFromDb == null)
            {
                ModelState.AddModelError(nameof(model.Email), $"User with email: {model.Email} not found");

                return new UnprocessableEntityObjectResult(ModelState);
            }

            var accountEntity = await _accountService.CreateAccount(model);

            var accountToReturn = _mapper.Map<AccountForDisplay>(accountEntity);

            return CreatedAtRoute(
                "GetAccount",
                new { id = accountToReturn.Id }, accountToReturn);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateAccount(int id, 
            [FromBody] JsonPatchDocument<AccountForUpdate> model)
        {
            _logger.LogInformation("Partial update account, id: {id}", id);

            var accountFromDb = await _accountRepository.GetByIdAsync(id);
            if(accountFromDb == null)
            {
                return BadRequest("No account found for this id");
            }

            var userEmail = User.Claims.FirstOrDefault(s => s.Type == JwtClaimTypes.Email).Value;
            var userRole = User.Claims.FirstOrDefault(s => s.Type == JwtClaimTypes.Role).Value;

            if (userEmail != accountFromDb.Email && userRole != "admin")
            {
                return Forbid();
            }

            var accountToPatch = _jsonPatchManager.Convert(model, accountFromDb);

            if (!TryValidateModel(accountToPatch))
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            _mapper.Map(accountToPatch, accountFromDb);

            await _accountService.UpdateAccount(accountFromDb);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize("isAdmin")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            _logger.LogInformation("Delete account, id: {id}", id);

            var accountFromDb = await _accountRepository.GetByIdAsync(id);
            if(accountFromDb == null)
            {
                return NotFound("No account found for this id");
            }

            await _accountService.DeleteAccount(accountFromDb);

            var userFromDb = await _userManager.FindByEmailAsync(accountFromDb.Email);
            await _userManager.DeleteAsync(userFromDb);

            return NoContent();
        }
    }
}