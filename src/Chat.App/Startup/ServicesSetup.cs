﻿using Chat.BL;

namespace Chat.App.Startup;

public static class ServicesSetup
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(AssemblyReference.Assembly)
            .AddClasses(classes => classes
                .Where(type => type.IsClass && type.Name.EndsWith("Service")))
            .AsMatchingInterface()
            .WithScopedLifetime());

        return services;
    }
}