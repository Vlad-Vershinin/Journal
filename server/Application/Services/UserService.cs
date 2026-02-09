using Domain.DTOs;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IPasswordService _passwordService;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository repository, 
        IPasswordService passwordService,
        ILogger<UserService> logger)
    {
        _repository = repository;
        _passwordService = passwordService;
        _logger = logger;
    }

    public async Task<Result<User>> CreateUser(User user)
    {
        _logger.LogInformation("Создание пользователя: {Login}", user.Login);

        try
        {
            user.PasswordHash = _passwordService.HashPassword(user.PasswordHash);
            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();

            var new_user = await _repository.GetByLoginAsync(user.Login);

            if (new_user is null)
            {
                _logger.LogError("Ошибка БД: Не полулось получить пользователя {Login} после создания.", user.Login);
                return Result<User>.Failure(ErrorCode.NotCreated, "Не удалось создать пользователя.");
            }

            _logger.LogInformation("Пользоваель {Login} создан с ID {UserId}.", new_user.Login, new_user.Id);
            return Result<User>.Success(new_user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании пользователя {Login}", user.Login);
            return Result<User>.Failure(ErrorCode.NotCreated, "Ошибка при создании пользователя.");
        }
    }

    public async Task<Result> Login(LoginDto dto)
    {
        var user = await _repository.GetByLoginAsync(dto.Login);

        if (user is null)
        {
            _logger.LogWarning("Попытка входа: пользователь {Login} не найден", dto.Login);
            return Result.Failure(ErrorCode.Unauthorized, "Неверный логин или пароль.");
        }

        if (!_passwordService.ValidatePassword(dto.Password, user.PasswordHash))
        {
            _logger.LogWarning("Попытка входа: неверный пароль для {Login}", dto.Login);
            return Result.Failure(ErrorCode.Unauthorized, "Неверный логин или пароль.");
        }

        _logger.LogInformation("Пользователь {Login} успешно авторизован.", user.Login);
        return Result.Success();
    }
}
