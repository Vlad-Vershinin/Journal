using API.DTOs;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/admin/groups")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminGroupController : ControllerBase
{
    private readonly AdminGroupService _groupService;

    public AdminGroupController(AdminGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDto groupDto)
    {
        var result = await _groupService.CreateGroup(groupDto.GroupName);

        if (result.IsFailure)
        {
            return BadRequest(result.Messages);
        }

        var response = new ResponseCreateGroup
        {
            Id = result.Value!.Id,
            GroupName = result.Value!.Name,
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGroupWithUsers(int id)
    {
        var result = await _groupService.GetGroup(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Messages);
        }
        var group = result.Value!;
        return Ok(group);
    }

    [HttpGet]
    public async Task<IActionResult> GetGroupsNames()
    {
        var result = await _groupService.GetGroupsName();
        if (result.IsFailure)
        {
            return BadRequest(result.Messages);
        }
        var response = result.Value!.Select(g => new ResponseGetGroup
        {
            Id = g.Id,
            GroupName = g.Name,
        }).ToList();
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteGroup([FromQuery] int id)
    {
        var result = await _groupService.DeleteGroup(id);

        if (result.IsFailure)
        {
            return BadRequest(result.Messages);
        }

        return Ok();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> RenameGroup(int id, [FromBody] RenameGroupDto groupDto)
    {
        var result = await _groupService.RenameGroup(id, groupDto.Name);

        if (result.IsFailure)
        {
            return BadRequest(result.Messages);
        }

        return Ok();
    }

    [HttpPatch("{groupId}/{userId}")]
    public async Task<IActionResult> AddUserToGroup(int groupId, int userId)
    {
        var result = await _groupService.AddUserToGroup(groupId, userId);
        if (result.IsFailure)
        {
            return BadRequest(result.Messages);
        }
        return Ok();
    }
}
