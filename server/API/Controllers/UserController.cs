using Domain.DTOs;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        await _userService.CreateUser(user);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _userService.Login(dto);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Messages);
        }

        return Ok();
    }
}
