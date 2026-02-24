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

        return Ok(result.Value);
    }
}
