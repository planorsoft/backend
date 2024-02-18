using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Planor.Application.Common.Interfaces;
using Planor.Application.Currencies.Queries.GetCurrencies;
using Planor.Application.Customers.Queries;
using Planor.Application.Customers.Queries.GetCustomerById;
using Planor.Application.Duties.Queries.GetDutyById;
using Planor.Application.Events.Queries;
using Planor.Application.Projects.Queries.GetProjectById;
using Planor.Application.Tags.Queries.GetTagsWithPagination;
using Planor.Application.Users.Queries.GetUser;
using Planor.Domain.Entities;
using Planor.Infrastructure.Persistence;
using Planor.WebUI.Services;
using Serilog;

namespace Planor.WebUI;

public static class ConfigureServices
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();
        
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        var modelBuilder = new ODataConventionModelBuilder();
        modelBuilder.EntitySet<UserDto>("users");
        modelBuilder.EntitySet<CustomerDto>("customers");
        modelBuilder.EntitySet<CurrencyDto>("currencies");
        modelBuilder.EntitySet<DutyDto>("duties");
        modelBuilder.EntitySet<ProjectDto>("projects");
        modelBuilder.EntitySet<TagDto>("tags");
        modelBuilder.EntitySet<EventDto>("events");

        modelBuilder.EnableLowerCamelCase();
        services.AddControllers().AddOData(options => options
                .EnableQueryFeatures()
                .Select()
                .Filter()
                .OrderBy()
                .Count()
                .Expand()
                .AddRouteComponents(
                    "api/odata", modelBuilder.GetEdmModel()
                )
        );
        
        return services;
    }

}
