namespace Chat.Presentation.Hubs;

public interface IChatHub
{
    Task DisbandGroupAsync(string chatId);
}