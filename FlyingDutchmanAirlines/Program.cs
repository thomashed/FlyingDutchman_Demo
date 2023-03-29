using FlyingDutchmanAirlines;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

// using top-level statements, we're already in our Main, while some "usings" have been defined globally

InitializeHost();

public static partial class Program
{
    private static void InitializeHost() =>
        Host.CreateDefaultBuilder().ConfigureWebHostDefaults(builder =>
        {
            builder.UseStartup<Startup>();
            builder.UseUrls("http://0.0.0.0:8081");
        }).Build().Run();
} 








 