using Domain.Models;
using Domain.Repositories;
using Domain.Services;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository userRepository)
    {
        _repository = userRepository;
    }

    public async Task CreateUser(User user)
    {
        await _repository.AddAsync(user);
    }
}
