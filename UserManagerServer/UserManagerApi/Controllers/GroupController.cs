using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using IdentityData.Entities;

using UserManagerApi.Helper;
using UserManagerCore.Dtos.Group;
using UserManagerCore.Entities;
using UserManagerCore.Repositories;
using UserManagerCore.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace UserManagerApi.Controllers
{
   
    [Route("api/groups")]
    [ApiController]
    [Authorize("isAdmin")]
    public class GroupController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAsyncRepository<Group> _groupRepository;
        private readonly JsonPatchManager _jsonPatchManager;
        private readonly IGroupService _groupService;

        public GroupController(
            IMapper mapper,
            ILogger<GroupController> logger,
            JsonPatchManager jsonPatchManager,
            UserManager<AppUser> userManager,
            IGroupService groupService,
            IAsyncRepository<Group> groupRepository)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
            _logger = logger;
            _jsonPatchManager = jsonPatchManager;
            _groupService = groupService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetGroups()
        {
            _logger.LogInformation("Get all groups");
            
            var groupsEntityies = await _groupRepository.GetAllAsync();

            var groupsForDisplay = _mapper.Map<List<GroupForDisplay>>(groupsEntityies);

            return Ok(groupsForDisplay);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroup(int id)
        {
            _logger.LogInformation("Get group, id: {id}");

            var groupsEntity = await _groupRepository.GetByIdAsync(id);
            if(groupsEntity == null)
            {
                return BadRequest("No group found for this id");
            }

            var groupForDisplay = _mapper.Map<GroupForDisplay>(groupsEntity);

            return Ok(groupForDisplay);
        }

        [HttpPost]
        public async Task<IActionResult> AddGroup(GroupForCreate model)
        {
            _logger.LogInformation("Add new group");

            var createdGroup = await _groupService.Create(model);

            var groupToReturn = _mapper.Map<GroupForDisplay>(createdGroup);

            return CreatedAtRoute(
                "GetAccount",
                new { id = groupToReturn.Id }, groupToReturn);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateGroup(int id,
            [FromBody] JsonPatchDocument<GroupForUpdate> model)
        {
            _logger.LogInformation("Partial update group, id: {id}", id);

            var groupFromDb = await _groupRepository.GetByIdAsync(id);
            if (groupFromDb == null)
            {
                return BadRequest("No group found for this id");
            }

            var groupToPatch = _jsonPatchManager.Convert(model, groupFromDb);

            if (!TryValidateModel(groupToPatch))
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            _mapper.Map(groupToPatch, groupFromDb);

            await _groupService.UpdateGroup(groupFromDb);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            _logger.LogInformation("Delete group, id: {id}", id);

            var groupFromDb = await _groupRepository.GetByIdAsync(id);
            if (groupFromDb == null)
            {
                return NotFound("No group found for this id");
            }

            await _groupService.DeleteGroup(groupFromDb);

            return NoContent();
        }


    }
}