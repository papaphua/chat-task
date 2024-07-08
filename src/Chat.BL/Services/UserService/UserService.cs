using Chat.BL.Abstractions.Data;
using Chat.BL.Abstractions.Results;
using Chat.BL.Entities;
using Chat.BL.Errors;
using Chat.BL.Requests;
using Microsoft.EntityFrameworkCore;

namespace Chat.BL.Services.UserService;

public sealed class UserService(
    IDbContext db,
    IUnitOfWork unitOfWork)
    : IUserService
{
    public async Task<Result<User>> GetUserAsync(Guid userId)
    {
        var user = await db.Set<User>()
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user is null
            ? Result<User>.Failure(UserError.NotFound)
            : Result<User>.Success(user);
    }

    public async Task<Result<User>> CreateUserAsync(UserRequest.CreateUser request)
    {
        var user = await db.Set<User>()
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user is not null) return Result<User>.Failure(UserError.AlreadyExists);

        user = User.Create(
            request.Username,
            request.FirstName,
            request.LastName);

        await db.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        return Result<User>.Success(user);
    }

    public async Task<Result<User>> UpdateUserAsync(Guid userId, UserRequest.UpdateUser request)
    {
        var user = await db.Set<User>()
            .FirstOrDefaultAsync(u => u.Id == userId);

        var userByUsername = await db.Set<User>()
            .FirstOrDefaultAsync(u => u.Username == request.Username && u.Id != userId);

        var canUpdateResult = CanUpdateUser(user, userByUsername, request);

        if (!canUpdateResult.IsSuccess) return Result<User>.Failure(canUpdateResult.Error!);

        user.Username = request.Username;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        db.Update(user);
        await unitOfWork.SaveChangesAsync();

        return Result<User>.Success(user);
    }

    public async Task<Result> RemoveUserAsync(Guid userId)
    {
        var user = await db.Set<User>()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null) return Result<User>.Failure(UserError.NotFound);

        db.Remove(user);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public Result CanUpdateUser(User? currentUser, User? userFromDb, UserRequest.UpdateUser request)
    {
        if (currentUser is null) return Result.Failure(UserError.NotFound);

        if (string.IsNullOrWhiteSpace(request.Username)) return Result.Failure(UserError.UsernameRequired);

        if (userFromDb is not null) return Result.Failure(UserError.AlreadyExists);

        return Result.Success();
    }
}