using Chat.BL.Abstractions.Results;
using Chat.BL.Entities;
using Chat.BL.Requests;

namespace Chat.BL.Services.UserService;

public interface IUserService
{
    Task<Result<User>> GetUserAsync(Guid userId);

    Task<Result<User>> CreateUserAsync(UserRequest.CreateUser request);

    Task<Result<User>> UpdateUserAsync(Guid userId, UserRequest.UpdateUser request);

    Task<Result> RemoveUserAsync(Guid userId);
}