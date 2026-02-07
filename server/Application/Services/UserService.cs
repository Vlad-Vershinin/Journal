using Domain.DTOs;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IPasswordService _passwordService;

    public UserService(
        IUserRepository repository, 
        IPasswordService passwordService)
    {
        _repository = repository;
        _passwordService = passwordService;
    }

    public async Task CreateUser(User user)
    {
        user.PasswordHash = _passwordService.HashPassword(user.PasswordHash);
        await _repository.AddAsync(user);
    }

    public async Task<bool> Login(LoginDto dto)
    {
        var user = await _repository.GetByLoginAsync(dto.Login);

        if (user is null)
            return false;

        return _passwordService.ValidatePassword(dto.Password, user.PasswordHash);
    }
}
