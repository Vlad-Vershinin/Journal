using API.DTOs;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        var result = await _userService.CreateUser(user);

        if (result.IsFailure)
        {
            return BadRequest(result.Messages);
        }

        return Ok(result.Value);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _userService.Login(dto.Login, dto.Password);

        if (result.IsFailure)
        {
            return BadRequest(result.Messages);
        }

        return Ok(result.Value);
    }
}
