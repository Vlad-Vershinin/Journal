using Application.Abstractions;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class AdminGroupService
{
    private readonly IGroupRepository _repository;
    private readonly ILogger<AdminGroupService> _logger;

    public AdminGroupService(
        IGroupRepository gradeRepository,
        ILogger<AdminGroupService> logger)
    {
        _repository = gradeRepository;
        _logger = logger;
    }

    public async Task<Result<Group>> CreateGroup(string groupName)
    {
        _logger.LogInformation("Создание группы: {GroupName}", groupName);

        try
        {
            var groups = await _repository.GetByNameAsync(groupName);
            if (groups.Any())
            {
                _logger.LogInformation("Группа {GroupName} уже создана", groupName);
                return Result<Group>.Failure(ErrorCode.NotCreated, "Такая группа уже существует.");
            }

            var new_group = new Group { Name = groupName };
            await _repository.AddAsync(new_group);

            if (new_group.Id == 0)
            {
                _logger.LogError("Ошибка БД: Не удалось создать группу {GroupName}", groupName);
                return Result<Group>.Failure(ErrorCode.NotCreated, "Не удалось создать группу");
            }

            _logger.LogInformation("Создана новая группа {GroupName} с ID {GroupId}", new_group.Name, new_group.Id);
            return Result<Group>.Success(new_group);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании группы {GroupName}", groupName);
            return Result<Group>.Failure(ErrorCode.NotCreated, "Ошибка при создании группы.");
        }
    }

    public async Task<Result> DeleteGroup(int groupId)
    {
        _logger.LogInformation("Удаление группы: {GroupId}", groupId);

        try
        {
            var group = await _repository.GetWithUsersAsync(groupId);
            if (group == null)
            {
                _logger.LogInformation("Группа с ID {GroupId} не найдена", groupId);
                return Result.Failure(ErrorCode.NotFound, "Группа не найдена.");
            }

            if (group.Users != null && group.Users.Any())
            {
                _logger.LogWarning("Нельзя удалить группу {GroupId} - в ней есть пользователи", groupId);
                return Result.Failure(ErrorCode.Conflict, "Нельзя удалить группу: в ней есть пользователи.");
            }

            await _repository.DeleteAsync(groupId);

            _logger.LogInformation("Группа с ID {GroupId} успешно удалена", groupId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении группы {GroupId}", groupId);
            return Result.Failure(ErrorCode.NotCreated, "Ошибка при удалении группы.");
        }
    }

    public async Task<Result<Group>> RenameGroup(int groupId, string newName)
    {
        _logger.LogInformation("Переименование группы {GroupId} -> {NewName}", groupId, newName);

        try
        {
            var existing = await _repository.GetByNameAsync(newName);
            if (existing.Any(g => g.Id != groupId))
            {
                _logger.LogInformation("Группа с именем {NewName} уже существует", newName);
                return Result<Group>.Failure(ErrorCode.Conflict, "Такая группа уже существует.");
            }

            var group = await _repository.GetByIdAsync(groupId);
            if (group == null)
            {
                _logger.LogInformation("Группа с ID {GroupId} не найдена", groupId);
                return Result<Group>.Failure(ErrorCode.NotFound, "Группа не найдена.");
            }

            group.Name = newName;
            await _repository.UpdateAsync(group);

            _logger.LogInformation("Группа с ID {GroupId} переименована в {NewName}", groupId, newName);
            return Result<Group>.Success(group);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при переименовании группы {GroupId}", groupId);
            return Result<Group>.Failure(ErrorCode.NotCreated, "Ошибка при переименовании группы.");
        }
    }
}
