using Chat.BL.Abstractions.Data;
using Chat.BL.Entities;
using Chat.BL.Errors;
using Chat.BL.Requests;
using Chat.BL.Services.UserService;
using Moq;

namespace Chat.UnitTests;

public sealed class UserServiceTests
{
    [Fact]
    public void CanUpdateUser_UserIsNull_ReturnsNotFound()
    {
        var request = new UserRequest.UpdateUser("newusername",
            "John",
            "Doe");

        var userService = SetupUserService();
        
        var result = userService.CanUpdateUser(null, null, request);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(UserError.NotFound, result.Error);
    }

    [Fact]
    public void CanUpdateUser_UsernameIsNullOrWhitespace_ReturnsUsernameRequired()
    {
        var currentUser = User.Create("Bobby");
        var request = new UserRequest.UpdateUser("");

        var userService = SetupUserService();
        
        var result = userService.CanUpdateUser(currentUser, null, request);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(UserError.UsernameRequired, result.Error);
    }

    [Fact]
    public void CanUpdateUser_UserFromDbExists_ReturnsAlreadyExists()
    {
        var currentUser = User.Create("Bobby");
        var userFromDb = new User { Username = "Bobby" };
        var request = new UserRequest.UpdateUser("Bobby");

        var userService = SetupUserService();
        
        var result = userService.CanUpdateUser(currentUser, userFromDb, request);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(UserError.AlreadyExists, result.Error);
    }

    [Fact]
    public void CanUpdateUser_ValidRequest_ReturnsSuccess()
    {
        var currentUser = new User();
        var request = new UserRequest.UpdateUser("newusername",
            "John",
            "Doe");

        var userService = SetupUserService();
        
        var result = userService.CanUpdateUser(currentUser, null, request);
        
        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
    }

    private static UserService SetupUserService()
    {
        var dbContextMock = new Mock<IDbContext>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var userService = new UserService(dbContextMock.Object, unitOfWorkMock.Object);
        return userService;
    }
}