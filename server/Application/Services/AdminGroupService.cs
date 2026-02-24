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
}
