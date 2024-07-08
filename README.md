### Within Chat.DAL

```powershell
dotnet ef migrations add NAME --startup-project "../Chat.App"
dotnet ef migrations remove --startup-project "../Chat.App"
dotnet ef database update --startup-project "../Chat.App"
dotnet ef database drop --startup-project "../Chat.App"
```