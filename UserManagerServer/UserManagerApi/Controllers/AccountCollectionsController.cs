using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IdentityData.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserManagerCore.Dtos;
using UserManagerCore.Dtos.Account;
using UserManagerCore.Entities;
using UserManagerCore.Repositories;

namespace UserManagerApi.Controllers
{
    [Authorize("isAdmin")]
    [Route("api/groups/{groupId}/accounts")]
    [ApiController]
    public class AccountCollectionsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AccountCollectionsController(
            IMapper mapper,
            ILogger<AccountCollectionsController> logger,
            UserManager<AppUser> userManager,
            IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccounts(int groupId)
        {
            _logger.LogInformation("Get accounts in group with id: {id}", groupId);

            var accountsFromDb = await _accountRepository
                .GetWhereAsync(s => s.GroupId == groupId, lazyLoading: true);

            var accountForDisplay = _mapper.Map<List<AccountForDisplay>>(accountsFromDb);

            return Ok(accountForDisplay);
        }
    }
}