using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FlyingDutchmanAirlines;

public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers()); // configure endpoints, scan for controllers in the app 
    }

    // Host first calls ConfigureServices, giving us a chance to register any services we want to use in our
    // app in our case, controllers. If this is skipped, MapControllers wouldn't find any controllers 
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddTransient(typeof(FlightService),typeof(FlightService));
        services.AddTransient(typeof(FlightRepository), typeof(FlightRepository));
        services.AddTransient(typeof(AirportRepository), typeof(AirportRepository));
        services.AddTransient(typeof(FlyingDutchmanAirlinesContext), typeof(FlyingDutchmanAirlinesContext));
    }

}


