using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MiApp.Application.Behaviors;
using MiApp.Application.Services;

namespace MiApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        // Registrar HttpClient
        services.AddHttpClient<IApiHttpClientService, ApiHttpClientService>();

        return services;
    }
}
