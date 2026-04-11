using Application.Abstractions;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class UserService
{
    private readonly IUserRepository _repository;
    private readonly IPasswordService _passwordService;
    private readonly ILogger<UserService> _logger;
    private readonly IJwtService _jwtService;

    public UserService(
        IUserRepository repository, 
        IPasswordService passwordService,
        ILogger<UserService> logger,
        IJwtService jwtService)
    {
        _repository = repository;
        _passwordService = passwordService;
        _logger = logger;
        _jwtService = jwtService;
    }

    public async Task<Result<User>> CreateUser(User user)
    {
        _logger.LogInformation("Создание пользователя: {Login}", user.Login);

        try
        {
            user.PasswordHash = _passwordService.HashPassword(user.PasswordHash);
            var users = await _repository.GetAllAsync();
            if (users.Any(u => u.Login.Equals(user.Login, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogError("Ошибка БД: Не полулось создать пользователя {Login}, такой логин уже используется.", user.Login);
                return Result<User>.Failure(ErrorCode.NotCreated, "Не удалось создать пользователя.");
            }

            await _repository.AddAsync(user);

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
    public async Task<Result<string>> DeleteUser(int id)
    {
        var userDelete = await _repository.GetByIdAsync(id);
        if (userDelete is null)
        {
            _logger.LogWarning("Попытка удаления: пользователь {Id} не найден", id);
            return Result<string>.Failure(ErrorCode.Unauthorized, "Не удалось найти пользователя");
        }
        await _repository.DeleteAsync(id);
        _logger.LogInformation("Пользователь {Id} успешно удалён.", userDelete.Id);
        return Result<string>.Success("Пользователь удален");
    }
    public async Task<Result<string>> Login(string login, string password)
    {
        var user = await _repository.GetByLoginAsync(login);

        if (user is null)
        {
            _logger.LogWarning("Попытка входа: пользователь {Login} не найден", login);
            return Result<string>.Failure(ErrorCode.Unauthorized, "Неверный логин или пароль.");
        }

        if (!_passwordService.ValidatePassword(password, user.PasswordHash))
        {
            _logger.LogWarning("Попытка входа: неверный пароль для {Login}", login);
            return Result<string>.Failure(ErrorCode.Unauthorized, "Неверный логин или пароль.");
        }

        var jwtToken = _jwtService.GenerateJwt(user);

        _logger.LogInformation("Пользователь {Login} успешно авторизован.", user.Login);
        return Result<string>.Success(jwtToken);
    }
}
