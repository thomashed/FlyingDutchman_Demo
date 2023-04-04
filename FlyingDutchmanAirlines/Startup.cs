using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace FlyingDutchmanAirlines;

public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers()); // configure endpoints, scan for controllers in the app 

        app.UseSwagger();
        app.UseSwaggerUI(swagger =>
            swagger.SwaggerEndpoint("/swagger/v1/swagger.json",
                "Flying Dutchman Airlines"));
    }

    // Host first calls ConfigureServices, giving us a chance to register any services we want to use in our
    // app in our case, controllers. If this is skipped, MapControllers wouldn't find any controllers 
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddTransient(typeof(FlightService),typeof(FlightService));
        services.AddTransient(typeof(BookingService), typeof(BookingService));
        services.AddTransient(typeof(FlightRepository), typeof(FlightRepository));
        services.AddTransient(typeof(AirportRepository), typeof(AirportRepository));
        services.AddTransient(typeof(CustomerRepository), typeof(CustomerRepository));
        services.AddTransient(typeof(BookingRepository), typeof(BookingRepository));
        services.AddDbContext<FlyingDutchmanAirlinesContext>(ServiceLifetime.Transient);
        services.AddTransient(typeof(FlyingDutchmanAirlinesContext), typeof(FlyingDutchmanAirlinesContext));

        services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1",
                        new OpenApiInfo
                        {
                            Title = "",
                            Version = "v1"
                        }
                    );
                    var filePath = Path.Combine(System.AppContext.BaseDirectory, "MyApi.xml");
                    options.IncludeXmlComments(filePath);
                }
            );

        
    }

}


