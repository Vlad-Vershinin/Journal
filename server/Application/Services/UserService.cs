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

    public async Task<Result<User>> CreateUser(User user)
    {
        user.PasswordHash = _passwordService.HashPassword(user.PasswordHash);
        await _repository.AddAsync(user);
        await _repository.SaveChangesAsync();

        var new_user = await _repository.GetByLoginAsync(user.Login);

        if (new_user is null)
        {
            return Result<User>.Failure(ErrorCode.NotCreated, "Не удалось создать пользователя.");
        }
        
        return Result<User>.Success(new_user);
    }

    public async Task<Result> Login(LoginDto dto)
    {
        var user = await _repository.GetByLoginAsync(dto.Login);

        if (user is null)
            return Result.Failure(ErrorCode.Unauthorized, "Неверный логин или пароль.");

        if (!_passwordService.ValidatePassword(dto.Password, user.PasswordHash))
            return Result.Failure(ErrorCode.Unauthorized, "Неверный логин или пароль.");

        return Result.Success();
    }
}
