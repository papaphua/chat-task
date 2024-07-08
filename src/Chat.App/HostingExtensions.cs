using Chat.App.Startup;
using Chat.Presentation;
using Chat.Presentation.Hubs;

namespace Chat.App;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.AddControllers()
            .AddApplicationPart(AssemblyReference.Assembly);

        builder.Services.AddSignalR();

        builder.Services.AddSwaggerGen();

        builder.Services.AddEfCore(configuration);

        builder.Services.AddServices();

        builder.Services.AddSingleton<IChatHub, ChatHub>();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.MapControllers();

        app.MapHub<ChatHub>("chat-hub");

        return app;
    }
}