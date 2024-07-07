using Chat.BL.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Presentation.Controllers;

[ApiController]
[Route("api/user")]
public sealed class UserController(IUserService userService)
{
}