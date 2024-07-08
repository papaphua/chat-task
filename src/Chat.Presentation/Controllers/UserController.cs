using Chat.BL.Requests;
using Chat.BL.Services.UserService;
using Chat.Presentation.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Presentation.Controllers;

[ApiController]
[Route("api/user")]
public sealed class UserController(IUserService userService)
{
    [HttpGet("{userId:guid}")]
    public async Task<IResult> GetUser(Guid userId)
    {
        var result = await userService.GetUserAsync(userId);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    [HttpPost]
    public async Task<IResult> CreateUser(UserRequest.CreateUser request)
    {
        var result = await userService.CreateUserAsync(request);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    [HttpPut("{userId:guid}")]
    public async Task<IResult> UpdateUser(Guid userId, UserRequest.UpdateUser request)
    {
        var result = await userService.UpdateUserAsync(userId, request);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    [HttpDelete("{userId:guid}")]
    public async Task<IResult> RemoveUser(Guid userId)
    {
        var result = await userService.RemoveUserAsync(userId);

        return result.IsSuccess
            ? Results.Ok()
            : result.ToProblemDetails();
    }
}