using Chat.BL.Abstractions.Data;
using Chat.BL.Abstractions.Results;
using Chat.BL.Entities;
using Chat.BL.Errors;
using Chat.BL.Requests;
using Microsoft.EntityFrameworkCore;

namespace Chat.BL.Services.UserService;

public sealed class UserService(
    IDbContext db,
    IUnityOfWork unityOfWork)
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

    public async Task<Result<User>> CreateUserAsync(UserRequest.Create request)
    {
        var user = await db.Set<User>()
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user is not null) return Result<User>.Failure(UserError.AlreadyExists);

        user = User.Create(
            request.Username,
            request.FirstName,
            request.LastName);

        await db.AddAsync(user);
        await unityOfWork.SaveChangesAsync();

        return Result<User>.Success(user);
    }

    public async Task<Result<User>> UpdateUserAsync(Guid userId, UserRequest.Update request)
    {
        var user = await db.Set<User>()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null) return Result<User>.Failure(UserError.NotFound);

        if (string.IsNullOrWhiteSpace(request.Username)) return Result<User>.Failure(UserError.UsernameRequired);

        var userByUsername = await db.Set<User>()
            .FirstOrDefaultAsync(u => u.Username == request.Username && u.Id != userId);
        
        if (userByUsername is not null) return Result<User>.Failure(UserError.AlreadyExists);
        
        user.Username = request.Username;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        db.Update(user);
        await unityOfWork.SaveChangesAsync();
        
        return Result<User>.Success(user);
    }

    public async Task<Result> RemoveUserAsync(Guid userId)
    {
        var user = await db.Set<User>()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null) return Result<User>.Failure(UserError.NotFound);
        
        db.Remove(user);
        await unityOfWork.SaveChangesAsync();
        
        return Result.Success();
    }
}