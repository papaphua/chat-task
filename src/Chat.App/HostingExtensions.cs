using Chat.App.Startup;

namespace Chat.App;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.AddControllers()
            .AddApplicationPart(Presentation.AssemblyReference.Assembly);

        builder.Services.AddEfCore(configuration);

        builder.Services.AddServices();

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

        return app;
    }
}